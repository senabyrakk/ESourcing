using ESourcing.Products.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace ESourcing.Products.Data.Manager
{
    public static class ProductSeed
    {
        public static void SeedData(IMongoCollection<Product> productCollection)
        {
            bool existProduct = productCollection.Find(p => true).Any();
            if (!existProduct)
            {
                productCollection.InsertManyAsync(GetConfigureProducts());
            }
        }

        private static IEnumerable<Product> GetConfigureProducts()
        {
            return new List<Product>() {

               new Product()
               {
                   Name = "Iphone 13",
                   Category = "Smart Phone",
                   Price = 16.000M,
                   Summary = "Akıllı telefon şimdi cok daha akıllı",
                   Description = "Telfonda bataya iyileştirmesi,kamre iyileştirmesi yapıldı.",
                   ImageFile = "photo-1.png"
               },
                new Product()
               {
                   Name = "Iphone 12",
                   Category = "Smart Phone",
                   Price = 16.000M,
                   Summary = "Akıllı telefon şimdi cok daha akıllı",
                   Description = "Telfonda bataya iyileştirmesi,kamre iyileştirmesi yapıldı.",
                   ImageFile = "photo-1.png"
               },
                 new Product()
               {
                   Name = "Iphone 11",
                   Category = "Smart Phone",
                   Price = 16.000M,
                   Summary = "Akıllı telefon şimdi cok daha akıllı",
                   Description = "Telfonda bataya iyileştirmesi,kamre iyileştirmesi yapıldı.",
                   ImageFile = "photo-1.png"
               },
                  new Product()
               {
                   Name = "Iphone X",
                   Category = "Smart Phone",
                   Price = 16.000M,
                   Summary = "Akıllı telefon şimdi cok daha akıllı",
                   Description = "Telfonda bataya iyileştirmesi,kamre iyileştirmesi yapıldı.",
                   ImageFile = "photo-1.png"
               },
           };
        }
    }
}
