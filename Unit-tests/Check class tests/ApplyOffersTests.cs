using LittleStoreAppBusinessLogic.Common;
using LittleStoreAppBusinessLogic.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unit_tests.Check_class_tests
{
    [TestClass]
    public class ApplyOffersTests
    {
        [TestMethod]
        public void ApplyOffers_NoOffersFount_NoOffersAdded()
        {
            //Arrange
            Check check = new Check();
            //Act
            check.ApplyOffers(new List<Offer>());
            //Assert
            Assert.IsFalse(check.Notes.Contains("OFFERS"));
        }

        [TestMethod]
        public void ApplyOffers_OffersConditionNotMatch_NoOffersAdded()
        {
            //Arrange
            Check check = new Check();
            Offer offer = new Offer(x => { return false; }, x => { }, "Title");
            //Act
            check.ApplyOffers(new List<Offer> { offer });
            //Assert
            Assert.IsFalse(check.Notes.Contains("OFFERS"));
        }

        [TestMethod]
        public void ApplyOffers_OffersConditionMatch_OfferWasAdded()
        {
            //Arrange
            Check check = new Check();
            string title = "Title";
            Offer offer = new Offer(x => { return true; }, x => { x.Add(new OrderInCheck(ProductType.Banana, 0, 0)); }, title);
            //Act
            check.ApplyOffers(new List<Offer> { offer });
            //Assert
            Assert.IsTrue(check.Notes.Contains("OFFERS"));
            Assert.IsTrue(check.Notes.Contains(title));
            Assert.AreEqual(1, check.Items.Count());
        }
    }
}
