using LittleStoreAppBusinessLogic.Common;
using LittleStoreAppBusinessLogic.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Unit_tests.Check_class_tests
{
    [TestClass]
    public class ToStringTests
    {
        [TestMethod]
        public void ToStrind_ItemsListEmpty_OnlyHeaderAndTotalsAdded()
        {
            //Arrange
            Check check = new Check();
            //Act
            var result = check.ToString();
            //Assert
            Assert.AreEqual("CHECK\r\nTotals______0\r\n", result);
        }

        [TestMethod]
        public void ToStrind_CheckContainsItems_ItemsWereAdded()
        {
            //Arrange
            Check check = new Check();
            var type = ProductType.Carrot;
            var price = 0.3m;
            var amount = 3;
            check.Items.Add(new OrderInCheck(type, amount, price));
            //Act
            var result = check.ToString();
            //Assert
            Assert.AreEqual($"CHECK\r\n{type.ToString()}__{amount.ToString()}___{price.ToString()}$\r\nTotals______{price.ToString()}\r\n", result);
        }

        [TestMethod]
        public void ToStrind_CheckContainsNotes_NotesWereAdded()
        {
            //Arrange
            Check check = new Check();
            var type = ProductType.Carrot;
            var price = 0.3m;
            var amount = 3;
            var note = "Important note";
            check.Items.Add(new OrderInCheck(type, amount, price));
            check.Notes.Add(note);
            //Act
            var result = check.ToString();
            //Assert
            Assert.AreEqual($"CHECK\r\n{type.ToString()}__{amount.ToString()}___{price.ToString()}$\r\nTotals______{price.ToString()}\r\n{note}\r\n", result);
        }
    }
}
