using ESourcing.Products.Data.Abstraction;
using ESourcing.Sourcing.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace ESourcing.Products.Data.Manager
{
    public class SourcingContext : ISourcingContext
    {
        private readonly IConfiguration _configuration;
        public SourcingContext(IConfiguration configuration)
        {
            _configuration = configuration;

            var client = new MongoClient(_configuration.GetSection("SourcingConnectionString").GetSection("ConnectionStrings").Value);
            var database = client.GetDatabase(_configuration.GetSection("SourcingConnectionString").GetSection("DatabaseName").Value);

            Auctions = database.GetCollection<Auction>("Auction");
            Bids = database.GetCollection<Bid>("Bid");

            SourceSeed.SeedData(Auctions);

        }

        public IMongoCollection<Auction> Auctions { get; }
        public IMongoCollection<Bid> Bids { get; }
    }
}
