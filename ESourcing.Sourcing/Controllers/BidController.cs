using ESourcing.Products.Data.Abstraction.Repository;
using ESourcing.Sourcing.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ESourcing.Sourcing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BidController : ControllerBase
    {
        private readonly IBidRepository _bidRepository;
        public BidController(IBidRepository bidRepository)
        {
            _bidRepository = bidRepository;
        }

        [HttpPost]
        [Route("SendBind")]
        public async Task<ActionResult> SendBind([FromBody]Bid bid) 
        {
            await _bidRepository.SendBid(bid);
            return Ok();
        }
        
        [HttpGet("GetBidsByAucitonId")]
        public async Task<ActionResult<IEnumerable<Bid>>> GetBidsByAucitonId(string id)
        {
            IEnumerable<Bid> bids = await _bidRepository.GetBidsByAucitonId(id);
            return Ok(bids);
        }

        [HttpGet]
        public async Task<ActionResult<Bid>> GetWinnerBid(string id)
        {
            var bid = await _bidRepository.GetWinnerBid(id);

            return Ok(bid);
        }
    }
}
