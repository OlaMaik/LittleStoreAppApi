using LittleStoreAppBusinessLogic.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace LittleStoreAppBusinessLogic.Models
{
    public class OrderInCheck : Order
    {
        public decimal TotalPrice { get; set; }

        public OrderInCheck(ProductType type, int amount, decimal price)
            : base(type, amount)
        {
            TotalPrice = price;
        }
    }
}
