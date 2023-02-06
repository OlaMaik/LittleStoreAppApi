using LittleStoreAppBusinessLogic.Common;
using LittleStoreAppBusinessLogic.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text;

namespace LittleStoreAppApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private readonly IShopService _shopService;
        public StoreController(IShopService shopService)
        {
            _shopService = shopService;
        }

        [HttpPost("Start")]
        public ActionResult Start(IFormFile assortmentFile, IFormFile orderFile)
        {
            try
            {
                var result = _shopService.Purchase(assortmentFile, orderFile);
                byte[] bytes = Encoding.UTF8.GetBytes(result);
                return File(bytes, "text/plain", "Check.txt");
            }
            catch (WrongFileException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (WrongTypeOfProduct ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
