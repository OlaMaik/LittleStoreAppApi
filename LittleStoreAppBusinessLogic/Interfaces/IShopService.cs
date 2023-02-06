using Microsoft.AspNetCore.Http;

namespace LittleStoreAppBusinessLogic.Interfaces
{
    public interface IShopService
    {
        string Purchase(IFormFile assortimentFile, IFormFile orderFile);
    }
}
