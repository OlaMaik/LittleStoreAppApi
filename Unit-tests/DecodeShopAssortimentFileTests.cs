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
    public class DecodeShopAssortimentFileTests
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
        public void DecodeShopAssortimentFile_FileHeaderIncorrect_WrongFileExceptionThrown()
        {
            //Arrange
            _reader.Setup(x => x.ReadLine()).Returns("Wrong header");
            //Act
            _service.DecodeShopAssortmentFile(null);
            //Assert
        }

        [TestMethod]
        public void DecodeShopAssortimentFile_FileHasNoItems_EmptyCollectionReturned()
        {
            //Arrange
            _reader.Setup(x => x.ReadLine()).Returns("PRODUCT|PRICE");
            _reader.Setup(x => x.Peek()).Returns(-1);
            //Act
            var result = _service.DecodeShopAssortmentFile(null).ToList();
            //Assert
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        [ExpectedException(typeof(WrongTypeOfProduct))]
        public void DecodeShopAssortimentFile_FileHasItemWithWrongType_WrongTypeOfProductThrown()
        {
            //Arrange
            _reader.SetupSequence(x => x.ReadLine()).Returns("PRODUCT|PRICE").Returns("WrongType|0,3");
            _reader.Setup(x => x.Peek()).Returns(0);
            //Act
            _service.DecodeShopAssortmentFile(null).ToList();
            //Assert
        }

        [TestMethod]
        public void DecodeShopAssortimentFile_FileHasOneItem_CollectionWithOneElementReturned()
        {
            //Arrange
            ProductType type = ProductType.Banana;
            decimal value = 0.3m;

            _reader.SetupSequence(x => x.ReadLine()).Returns("PRODUCT|PRICE").Returns($"{type.ToString()}|{value.ToString()}");
            _reader.SetupSequence(x => x.Peek()).Returns(0).Returns(-1);
            //Act
            var result = _service.DecodeShopAssortmentFile(null).Single();
            //Assert
            Assert.AreEqual(value, result.Price);
            Assert.AreEqual(type, result.Type);
        }

        [TestMethod]
        public void DecodeShopAssortimentFile_FileHasDublicatedItem_CollectionWithFirstItemReturned()
        {
            //Arrange
            ProductType type = ProductType.Banana;
            decimal value = 0.3m;
            decimal secondItemValue = 0.2m;

            _reader.SetupSequence(x => x.ReadLine()).Returns("PRODUCT|PRICE")
                .Returns($"{type.ToString()}|{value.ToString()}")
                .Returns($"{type.ToString()}|{secondItemValue.ToString()}");
            _reader.SetupSequence(x => x.Peek()).Returns(1).Returns(0).Returns(-1);
            //Act
            var result = _service.DecodeShopAssortmentFile(null).Single();
            //Assert
            Assert.AreEqual(value, result.Price);
            Assert.AreEqual(type, result.Type);
        }
    }
}
