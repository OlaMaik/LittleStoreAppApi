using LittleStoreAppBusinessLogic.Common;

namespace LittleStoreAppBusinessLogic.Models
{
    public class Product
    {
        public ProductType Type { get; set; }
        public decimal Price { get; set; }

        public Product(ProductType type, decimal price)
        {
            Type = type;
            Price = price;
        }

        public virtual decimal Calculate(int amount)
        {
            return Price * amount;
        }
    }
}
