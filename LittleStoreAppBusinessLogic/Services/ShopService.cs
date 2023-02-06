using LittleStoreAppBusinessLogic.Interfaces;
using Microsoft.AspNetCore.Http;
using LittleStoreAppBusinessLogic.Models;
using System.Collections.Generic;
using System.Linq;
using LittleStoreAppBusinessLogic.Common;

namespace LittleStoreAppBusinessLogic.Services
{
    public class ShopService : IShopService
    {
        private readonly IShopFileService _decoder;
        private List<Offer> _offers;

        public ShopService(IShopFileService decoder)
        {
            _decoder = decoder;
            SeedOffers();
        }

        public string Purchase(IFormFile assortimentFile, IFormFile orderFile)
        {
            var orders = _decoder.DecodeOrderFile(orderFile);
            var assortiment = _decoder.DecodeShopAssortmentFile(assortimentFile);
            var check = new Check();
            check.FormItemsList(orders, assortiment);
            check.ApplyOffers(_offers);
            return check.ToString();
        }

        private void SeedOffers()
        {
            //In normal flow offers would be created by the separate request and stored in a db.
            //However in this demo version temporar collection will be created instead

            _offers = new List<Offer>
            {
                new Offer(
                    x => x.SingleOrDefault(o => o.Product == ProductType.Banana)?.Amount >= 2,
                    x => { var bananasAmount = x.Single(o => o.Product == ProductType.Banana).Amount;
                        int freeLemonsToAdd = bananasAmount / 2;
                        var lemons = x.SingleOrDefault(l => l.Product == ProductType.Lemon);
                        if(lemons == null) x.Add(new OrderInCheck(ProductType.Lemon, bananasAmount/2, 0));
                        else lemons.Amount += freeLemonsToAdd;
                    },
                    "Buy 2 banana, get lemon as a present"),
                new Offer(
                    x => x.SingleOrDefault(o => o.Product == ProductType.Watermelon)?.Amount >= 3 && x.Any(x => x.Product == ProductType.Carrot),
                    x => {
                        var carrots = x.Single(c => c.Product == ProductType.Carrot);
                        carrots.TotalPrice /= 2;
                    },
                    "Buy 3 watermelons, get 50% discount for carrots")
            };
        }
    }
}

