using ESourcing.Sourcing.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace ESourcing.Products.Data.Manager
{
    public static class SourceSeed
    {
        public static void SeedData(IMongoCollection<Auction> auctionCollection)
        {
            bool existAuction = auctionCollection.Find(p => true).Any();
            if (!existAuction)
            {
                auctionCollection.InsertManyAsync(GetConfigureAuctions());
            }
        }

        private static IEnumerable<Auction> GetConfigureAuctions()
        {
            return new List<Auction>() {

               new Auction()
               {
                   CreatedAt = DateTime.Now,
                   FinishedAt = DateTime.Now,
                   StartedAt = DateTime.Now,
                   Name = "Apple",
                   Status = (int)Status.Active,
                   Quantity = 5,
                   Description = "Bi dünya markası",
                   IncludedSellers = new List<string>()
                   {
                       "seller1@gmail.com",
                       "seller2@gmail.com",
                       "seller3@gmail.com"
                   },
                   ProductId = "61dc2a65f12a91f6f63181d4",
               },
                  new Auction()
               {
                   CreatedAt = DateTime.Now,
                   FinishedAt = DateTime.Now,
                   StartedAt = DateTime.Now,
                   Name = "Huwai",
                   Status = (int)Status.Active,
                   Quantity = 5,
                   Description = "eh",
                   IncludedSellers = new List<string>()
                   {
                       "seller1@gmail.com",
                       "seller2@gmail.com",
                       "seller3@gmail.com"
                   },
                   ProductId = "61dc2a65f12a91f6f63181d5",
               },
                     new Auction()
               {
                   CreatedAt = DateTime.Now,
                   FinishedAt = DateTime.Now,
                   StartedAt = DateTime.Now,
                   Name = "Samsung",
                   Status = (int)Status.Active,
                   Quantity = 5,
                   Description = "iyi",
                   IncludedSellers = new List<string>()
                   {
                       "seller1@gmail.com",
                       "seller2@gmail.com",
                       "seller3@gmail.com"
                   },
                   ProductId = "61dc2a65f12a91f6f63181d6",
               }
           };
        }

        public static void SeedData(IMongoCollection<Bid> bidCollection)
        {
            bool existBid = bidCollection.Find(p => true).Any();
            if (!existBid)
            {
                bidCollection.InsertManyAsync(GetConfigureBids());
            }
        }

        private static IEnumerable<Bid> GetConfigureBids()
        {
            return new List<Bid>() {

               new Bid()
               {
                   
               }
           };
        }
    }
}
