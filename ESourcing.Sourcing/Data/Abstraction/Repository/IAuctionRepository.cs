using ESourcing.Sourcing.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ESourcing.Products.Data.Abstraction.Repository
{
    public interface IAuctionRepository
    {
        Task<IEnumerable<Auction>> GetAll();
        Task<Auction> Get(string id);
        Task<IEnumerable<Auction>> GetByName(string name);
        Task Create(Auction entity);
        Task<bool> Update(Auction entity);
        Task<bool> Delete(string id);
    }
}
