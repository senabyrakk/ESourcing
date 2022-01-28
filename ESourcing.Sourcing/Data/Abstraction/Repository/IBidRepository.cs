using ESourcing.Sourcing.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ESourcing.Products.Data.Abstraction.Repository
{
    public interface IBidRepository
    {
        Task SendBid(Bid bid);
        Task<List<Bid>> GetBidsByAucitonId(string id);
        Task<Bid> GetWinnerBid(string id);
    }
}
