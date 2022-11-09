using ESourcing.Products.Data.Abstraction;
using ESourcing.Products.Data.Abstraction.Repository;
using ESourcing.Sourcing.Entities;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESourcing.Bids.Data.Manager.Repository
{
    public class BidRepository : IBidRepository
    {
        private readonly ISourcingContext _context;
        public BidRepository(ISourcingContext context)
        {
            _context = context;
        }

        public async Task<List<Bid>> GetBidsByAucitonId(string id)
        {
            FilterDefinition<Bid> filter = Builders<Bid>.Filter.Eq(a => a.AuctionId, id);

            List<Bid> bids = await _context.Bids.Find(filter).ToListAsync();

            bids = bids.OrderByDescending(a => a.CreatedAt)
                           .GroupBy(a => a.SellerUserName)
                            .Select(a => new Bid
                            {
                                Price = a.FirstOrDefault().Price,
                                AuctionId = a.FirstOrDefault().AuctionId,
                                ProductId = a.FirstOrDefault().ProductId,
                                CreatedAt = a.FirstOrDefault().CreatedAt,
                                SellerUserName = a.FirstOrDefault().SellerUserName,
                                Id = a.FirstOrDefault().Id

                            }).ToList();

            return bids;
        }

        public async Task<Bid> GetWinnerBid(string id)
        {
            List<Bid> bids = await GetBidsByAucitonId(id);
            return bids.OrderByDescending(a => a.Price).FirstOrDefault();
        }

        public async Task SendBid(Bid bid)
        {
            await _context.Bids.InsertOneAsync(bid);
        }
    }
}
