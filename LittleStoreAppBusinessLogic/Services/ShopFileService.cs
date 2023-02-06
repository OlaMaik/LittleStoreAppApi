using Microsoft.AspNetCore.Http;
using LittleStoreAppBusinessLogic.Models;
using System.Collections.Generic;
using System.Text;
using System.IO;
using LittleStoreAppBusinessLogic.Interfaces;
using System.Linq;
using LittleStoreAppBusinessLogic.Common;
using System;

namespace LittleStoreAppBusinessLogic.Services
{
    public class ShopFileService : IShopFileService
    {
        private const string VALID_ASSORTIMENT_FILE_HEADER = "PRODUCT|PRICE";
        private const string VALID_ORDER_FILE_HEADER = "PRODUCT|AMOUNT";

        private readonly IStreamReaderDecorator _streamReader;
        public ShopFileService(IStreamReaderDecorator streamReader)
        {
            _streamReader = streamReader;
        }

        public IEnumerable<Product> DecodeShopAssortmentFile(IFormFile file)
        {
            var result = new List<Product>();
            using (var reader = _streamReader.GetStreamReader(file))
            {
                var firstLine = reader.ReadLine();
                if (firstLine != VALID_ASSORTIMENT_FILE_HEADER)
                    throw new WrongFileException("Seems like wrong file was send as the assortment file");

                while (reader.Peek() >= 0)
                {
                    var line = reader.ReadLine();
                    string[] subs = line.Split('|');
                    var type = GetProductType(subs[0]);
                    if (result.Any(x => x.Type == type)) continue;
                    Product newProduct = new Product(type, decimal.Parse(subs[1]));
                    result.Add(newProduct);
                }
            }
            return result;
        }

        public IEnumerable<Order> DecodeOrderFile(IFormFile file)
        {
            var result = new List<Order>();
            using (var reader = _streamReader.GetStreamReader(file))
            {
                var firstLine = reader.ReadLine();
                if (firstLine != VALID_ORDER_FILE_HEADER)
                    throw new WrongFileException("Seems like wrong file was send as the order file");

                while (reader.Peek() >= 0)
                {
                    var line = reader.ReadLine();
                    string[] subs = line.Split('|');
                    var type = GetProductType(subs[0]);
                    var existingOrder = result.SingleOrDefault(x => x.Product == type);
                    if (existingOrder != null)
                    {
                        existingOrder.Amount += int.Parse(subs[1]);
                        continue;
                    }
                    Order newProduct = new Order(type, int.Parse(subs[1]));
                    result.Add(newProduct);
                }
            }
            return result;
        }

        private ProductType GetProductType(string stringType)
        {
            if (!Enum.TryParse<ProductType>(stringType, true, out ProductType type)) throw new WrongTypeOfProduct($"Product of type {stringType} wasn`t found");
            return type;
        }
    }
}
