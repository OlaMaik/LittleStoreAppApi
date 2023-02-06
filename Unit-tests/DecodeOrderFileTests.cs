using LittleStoreAppBusinessLogic.Common;
using LittleStoreAppBusinessLogic.Interfaces;
using LittleStoreAppBusinessLogic.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.IO;
using System.Linq;

namespace Unit_tests
{
    [TestClass]
    public class DecodeOrderFileTests
    {
        private readonly Mock<IStreamReaderDecorator> _streamReader = new Mock<IStreamReaderDecorator>();
        private ShopFileService _service;
        private readonly Mock<StreamReader> _reader = new Mock<StreamReader>(new MemoryStream());

        [TestInitialize]
        public void TestSetup()
        {
            _streamReader.Setup(s => s.GetStreamReader(It.IsAny<IFormFile>())).Returns(_reader.Object);
            _service = new ShopFileService(_streamReader.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(WrongFileException))]
        public void DecodeOrderFile_FileHeaderIncorrect_WrongFileExceptionThrown()
        {
            //Arrange
            _reader.Setup(x => x.ReadLine()).Returns("Wrong header");
            //Act
            _service.DecodeOrderFile(null);
            //Assert
        }

        [TestMethod]
        public void DecodeOrderFile_FileHasNoItems_EmptyCollectionReturned()
        {
            //Arrange
            _reader.Setup(x => x.ReadLine()).Returns("PRODUCT|AMOUNT");
            _reader.Setup(x => x.Peek()).Returns(-1);
            //Act
            var result = _service.DecodeOrderFile(null).ToList();
            //Assert
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        [ExpectedException(typeof(WrongTypeOfProduct))]
        public void DecodeOrderFile_FileHasItemWithWrongType_WrongTypeOfProductThrown()
        {
            //Arrange
            _reader.SetupSequence(x => x.ReadLine()).Returns("PRODUCT|AMOUNT").Returns("WrongType|3");
            _reader.Setup(x => x.Peek()).Returns(0);
            //Act
            _service.DecodeOrderFile(null).ToList();
            //Assert
        }

        [TestMethod]
        public void DecodeOrderFile_FileHasOneItem_CollectionWithOneElementReturned()
        {
            //Arrange
            ProductType type = ProductType.Banana;
            int amount = 2;

            _reader.SetupSequence(x => x.ReadLine()).Returns("PRODUCT|AMOUNT").Returns($"{type.ToString()}|{amount.ToString()}");
            _reader.SetupSequence(x => x.Peek()).Returns(0).Returns(-1);
            //Act
            var result = _service.DecodeOrderFile(null).Single();
            //Assert
            Assert.AreEqual(amount, result.Amount);
            Assert.AreEqual(type, result.Product);
        }

        [TestMethod]
        public void DecodeOrderFile_FileHasDublicatedItem_CollectionWithFirstItemReturned()
        {
            //Arrange
            ProductType type = ProductType.Banana;
            int amountFirst = 1;
            int amountSecond = 2;

            _reader.SetupSequence(x => x.ReadLine()).Returns("PRODUCT|AMOUNT")
                .Returns($"{type.ToString()}|{amountFirst.ToString()}")
                .Returns($"{type.ToString()}|{amountSecond.ToString()}");
            _reader.SetupSequence(x => x.Peek()).Returns(1).Returns(0).Returns(-1);
            //Act
            var result = _service.DecodeOrderFile(null).Single();
            //Assert
            Assert.AreEqual(amountSecond + amountFirst, result.Amount);
            Assert.AreEqual(type, result.Product);
        }
    }
}
