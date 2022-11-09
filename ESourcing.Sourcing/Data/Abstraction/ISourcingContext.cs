using ESourcing.Sourcing.Entities;
using MongoDB.Driver;

namespace ESourcing.Products.Data.Abstraction
{
    public interface ISourcingContext
    {
        IMongoCollection<Auction> Auctions { get;}
        IMongoCollection<Bid> Bids { get; }
    }
}
