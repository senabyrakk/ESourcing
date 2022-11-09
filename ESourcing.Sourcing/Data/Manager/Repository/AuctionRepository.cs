using ESourcing.Products.Data.Abstraction;
using ESourcing.Products.Data.Abstraction.Repository;
using ESourcing.Sourcing.Entities;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ESourcing.Auctions.Data.Manager.Repository
{
    public class AuctionRepository : IAuctionRepository
    {
        private readonly ISourcingContext _context;
        public AuctionRepository(ISourcingContext context)
        {
            _context = context;
        }

        public async Task Create(Auction Auction)
        {
            await _context.Auctions.InsertOneAsync(Auction);
        }

        public async Task<bool> Delete(string id)
        {
            var filter = Builders<Auction>.Filter.Eq(m => m.Id, id);

            DeleteResult result = await _context.Auctions.DeleteOneAsync(filter);

            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public async Task<Auction> Get(string id)
        {
            return await _context.Auctions.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Auction>> GetAll()
        {
            return await _context.Auctions.Find(p => true).ToListAsync();
        }

        public async Task<IEnumerable<Auction>> GetByName(string name)
        {
            var filter = Builders<Auction>.Filter.ElemMatch(p => p.Name, name);

            return await _context.Auctions.Find(filter).ToListAsync();
        }

        public async Task<bool> Update(Auction Auction)
        {
            var updateResult = await _context.Auctions.ReplaceOneAsync(filter: g => g.Id == Auction.Id, replacement: Auction);
            return  updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }
    }
}
