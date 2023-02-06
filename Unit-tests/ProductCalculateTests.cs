using LittleStoreAppBusinessLogic.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Unit_tests
{
    [TestClass]
    public class ProductCalculateTests
    {
        [TestMethod]
        public void Calculate_CalculationFlowWorksCorrectly()
        {
            //Arrange
            decimal price = 0.6m;
            int amount = 2;
            Product product = new Product(LittleStoreAppBusinessLogic.Common.ProductType.Banana, price);
            //Act
            var result = product.Calculate(amount);
            //Assert
            Assert.AreEqual(amount * price, result);
        }
    }
}
