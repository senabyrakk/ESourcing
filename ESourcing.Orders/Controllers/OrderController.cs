using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Orders.Application.Commands.OrderCreate;
using Orders.Application.Queries;
using Orders.Application.Responses;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESourcing.Orders.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetOrdersByUserName/{userName}")]
        public async Task<ActionResult<IEnumerable<OrderResponse>>> GetOrdersByUserName(string userName)
        {
            var query = new GetOrdersBySellerNameQuery(userName);

            var orders = await _mediator.Send(query);

            if (orders.Count() == decimal.Zero)
                return NotFound();

            return Ok(orders);
        } 

        [HttpPost("OrderCreate")]
        public async Task<ActionResult<OrderResponse>> OrderCreate([FromBody]OrderCreateCommond order)
        {
           var result =  await _mediator.Send(order);

            return Ok(result);
        }
    }
}
