using ESourcing.Products.Data.Abstraction.Repository;
using ESourcing.Sourcing.Entities;
using EventBusRabbitMq.Core;
using EventBusRabbitMq.Events;
using EventBusRabbitMq.Producer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ESourcing.Sourcing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuctionController : ControllerBase
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly IBidRepository _bidRepository;
        private readonly EventBusRabbitMqProducer _eventBus; 
        private ILogger<AuctionController> _logger;
        public AuctionController(IAuctionRepository auctionRepository, ILogger<AuctionController> logger, IBidRepository bidRepository, EventBusRabbitMqProducer eventBus)
        {
            _auctionRepository = auctionRepository;
            _logger = logger;
            _bidRepository = bidRepository;
            _eventBus = eventBus;

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Auction>>> Get()
        {
            var Auctions = await _auctionRepository.GetAll();
            return Ok(Auctions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Auction>>> Get(string id)
        {
            var Auction = await _auctionRepository.Get(id);

            if (Auction == null)
            {
                _logger.LogError($"Auction witdh id {id},hasnt bees found in database");
                return NotFound();
            }

            return Ok(Auction);
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<Auction>>> Post(Auction Auction)
        {
            await _auctionRepository.Create(Auction);

            return CreatedAtRoute("Get", new { id = Auction.Id }, Auction);
        }

        [HttpPut]
        public async Task<ActionResult<IEnumerable<Auction>>> Put(Auction Auction)
        {
            await _auctionRepository.Update(Auction);

            return CreatedAtRoute("Get", new { id = Auction.Id }, Auction);
        }

        [HttpDelete]
        public async Task<ActionResult<IEnumerable<Auction>>> Delete(string id)
        {
            var result = await _auctionRepository.Delete(id);

            if (!result)
            {
                _logger.LogError($"Auction witdh id {id},cant delete");
                return BadRequest();
            }

            return Ok();
        }

        [HttpPost("CompleteAuction")]
        public async Task<ActionResult> CompleteAuction(string id)
        {
            Auction auction = await _auctionRepository.Get(id);

            if (auction == null) return NotFound();

            if (auction.Status != (int)Status.Active)
            {
                _logger.LogError("Auction can not be completed");
                return BadRequest();
            }

            Bid bid = await _bidRepository.GetWinnerBid(auction.Id);

            if (bid == null) return NotFound();


            OrderCreateEvent eventMessage = new OrderCreateEvent()
            {
                Price = bid.Price,
                SellerUserName = bid.SellerUserName,
                ProductId = auction.ProductId,
                AuctionId = auction.Id,
                CreatedAt = bid.CreatedAt,
                Quantity = auction.Quantity,
            };

            auction.Status = (int)Status.Closed;

            bool result = await _auctionRepository.Update(auction);
            if (!result)
            {
                _logger.LogError("Auction can not updated");
                return BadRequest();
            }

            try
            {
                _eventBus.Publish(EventBusConstants.OrderCreateQueue, eventMessage);
            }
            catch (System.Exception ex)
            {
                _logger.LogError("Error publishing integration event");
                throw;
            }

            return Accepted();
            
        }

        [HttpPost("TestEvent")]
        public ActionResult<OrderCreateEvent> TestEvent() 
        {
            OrderCreateEvent message = new OrderCreateEvent()
            {
                AuctionId = "test1",
                ProductId = "test1_product_1",
                Price = 10,
                Quantity = 100,
                SellerUserName = "sena@gmail.com"
            };

            try
            {
                _eventBus.Publish(EventBusConstants.OrderCreateQueue, message);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest();
            }

            return Accepted(message);

        }
    }
}
