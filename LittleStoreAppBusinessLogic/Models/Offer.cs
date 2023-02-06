using System;
using System.Collections.Generic;
using System.Text;

namespace LittleStoreAppBusinessLogic.Models
{
    public class Offer
    {
        public string Title { get; set; }
        public virtual Predicate<List<OrderInCheck>> Condition { get; set; }
        public virtual Action<List<OrderInCheck>> Action { get; set; }

        public Offer(Predicate<List<OrderInCheck>> condition, Action<List<OrderInCheck>> action, string title)
        {
            Condition = condition;
            Title = title;
            Action = action;
        }
    }
}
