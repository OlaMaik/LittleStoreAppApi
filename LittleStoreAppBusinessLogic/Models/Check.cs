using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LittleStoreAppBusinessLogic.Models
{
    public class Check
    {
        public List<string> Notes { get; set; }
        public decimal Sum { get { return Items.Sum(x => x.TotalPrice); } }
        public List<OrderInCheck> Items { get; set; }

        public Check()
        {
            Items = new List<OrderInCheck>();
            Notes = new List<string>();
        }

        public void FormItemsList(IEnumerable<Order> orders, IEnumerable<Product> products)
        {
            foreach (var order in orders)
            {
                var requiredProduct = products.SingleOrDefault(x => x.Type == order.Product);
                if (requiredProduct == null)
                {
                    Notes.Add(($"Sorry, {order.Product.ToString()} is not available"));
                    return;
                }

                var existingItem = Items.SingleOrDefault(x => x.Product == order.Product);
                if (existingItem != null)
                {
                    existingItem.TotalPrice += requiredProduct.Calculate(order.Amount);
                    existingItem.Amount += order.Amount;
                    return;
                }

                Items.Add(new OrderInCheck(order.Product, order.Amount, requiredProduct.Calculate(order.Amount)));
            }
        }

        public void ApplyOffers(List<Offer> offers)
        {
            for (int i = 0; i < offers.Count(); i++)
            {
                if (offers[i].Condition(Items))
                {
                    offers[i].Action(Items);
                    if (i == 0) Notes.Add("OFFERS");
                    Notes.Add(offers[i].Title);
                }
            }
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.AppendLine("CHECK");
            foreach (var item in Items)
            {
                var line = $"{item.Product.ToString()}__{item.Amount.ToString()}___{item.TotalPrice.ToString()}$";
                result.AppendLine(line);
            }
            result.AppendLine($"Totals______{Sum}");

            foreach (var note in Notes)
            {
                result.AppendLine(note);
            }
            return result.ToString();
        }
    }
}
