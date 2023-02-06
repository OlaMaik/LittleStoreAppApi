using LittleStoreAppBusinessLogic.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace LittleStoreAppBusinessLogic.Interfaces
{
    public interface IShopFileService
    {
        IEnumerable<Product> DecodeShopAssortmentFile(IFormFile file);
        IEnumerable<Order> DecodeOrderFile(IFormFile file);
    }
}
