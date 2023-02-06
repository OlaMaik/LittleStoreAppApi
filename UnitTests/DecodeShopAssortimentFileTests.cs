using LittleStoreAppBusinessLogic.Common;
using LittleStoreAppBusinessLogic.Interfaces;
using LittleStoreAppBusinessLogic.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.IO;

namespace UnitTests
{
    [TestClass]
    public class DecodeShopAssortimentFileTests
    {
        private readonly Mock<IStreamReaderDecorator> _streamReader = new Mock<IStreamReaderDecorator>();
        private ShopFileService _service;
        private readonly Mock<StreamReader> _reader = new Mock<StreamReader>();

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
            var queueStuff = new Queue<string>();
            queueStuff.Enqueue("Wrong header");
            queueStuff.Enqueue(null);
            _reader.Setup(x => x.ReadLine()).Returns(queueStuff.Dequeue());
            //Act
            _service.DecodeShopAssortimentFile(null);
            //Assert
        }
    }
}
