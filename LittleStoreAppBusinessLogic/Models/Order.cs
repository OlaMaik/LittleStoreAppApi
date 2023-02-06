using LittleStoreAppBusinessLogic.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace LittleStoreAppBusinessLogic.Models
{
    public class Order
    {
        public ProductType Product { get; set; }
        public int Amount { get; set; }

        public Order(ProductType product, int amount)
        {
            Product = product;
            Amount = amount;
        }
    }
}
