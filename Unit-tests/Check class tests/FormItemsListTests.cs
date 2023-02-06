using LittleStoreAppBusinessLogic.Common;
using LittleStoreAppBusinessLogic.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace Unit_tests.Check_class_tests
{
    [TestClass]
    public class FormItemsListTests
    {
        [TestMethod]
        public void FormItemsList_EmptyListOfOrdersPassed_NoItemsAdded()
        {
            //Arrange
            Check check = new Check();
            //Act
            check.FormItemsList(new List<Order>(), new List<Product>());
            //Assert
            Assert.AreEqual(0, check.Items.Count);
        }

        [TestMethod]
        public void FormItemsList_OrderRequiresNotAvailableProduct_NoteAdded()
        {
            //Arrange
            Check check = new Check();
            var type = ProductType.Banana;
            //Act
            check.FormItemsList(new List<Order> { new Order(type, 3) }, new List<Product>());
            //Assert
            Assert.AreEqual(0, check.Items.Count);
            Assert.IsTrue(check.Notes.Contains($"Sorry, {type.ToString()} is not available"));
        }

        [TestMethod]
        public void FormItemsList_TwoOrdersForTheSameProductPassed_OneItemAdded()
        {
            //Arrange
            Check check = new Check();
            var type = ProductType.Banana;
            decimal price = 0.3m;
            decimal calculatedPrice = 0.9m;
            int firstOrderAmount = 3;
            int secondOrderAmount = 1;
            Mock<Product> product = new Mock<Product>(type, price);
            product.Setup(x => x.Calculate(It.IsAny<int>())).Returns(calculatedPrice);
            //Act
            check.FormItemsList(
                new List<Order> {
                    new Order(type, firstOrderAmount),
                    new Order(type, secondOrderAmount)
                },
                new List<Product> { product.Object });
            //Assert
            Assert.AreEqual(1, check.Items.Count);
            Assert.AreEqual(firstOrderAmount + secondOrderAmount, check.Items.Single().Amount);
            Assert.AreEqual(2 * calculatedPrice, check.Items.Single().TotalPrice);
        }

        [TestMethod]
        public void FormItemsList_TwoOrdersForDifferentProductsPassed_TwoItemsAdded()
        {
            //Arrange
            Check check = new Check();
            var type1 = ProductType.Banana;
            var type2 = ProductType.Carrot;
            decimal price1 = 0.3m;
            decimal price2 = 0.2m;
            int amount1 = 3;
            int amount2 = 1;
            Mock<Product> product1 = new Mock<Product>(type1, price1);
            product1.Setup(x => x.Calculate(It.IsAny<int>())).Returns(price1 * amount1);
            Mock<Product> product2 = new Mock<Product>(type2, price2);
            product2.Setup(x => x.Calculate(It.IsAny<int>())).Returns(price2 * amount2);
            //Act
            check.FormItemsList(
                new List<Order> {
                    new Order( type1, amount1),
                    new Order( type2, amount2)
                },
                new List<Product> { product1.Object, product2.Object });
            //Assert
            Assert.AreEqual(price1 * amount1, check.Items.Single(x => x.Product == type1).TotalPrice);
            Assert.AreEqual(price2 * amount2, check.Items.Single(x => x.Product == type2).TotalPrice);
            Assert.AreEqual(amount1, check.Items.Single(x => x.Product == type1).Amount);
            Assert.AreEqual(amount2, check.Items.Single(x => x.Product == type2).Amount);
        }
    }
}
