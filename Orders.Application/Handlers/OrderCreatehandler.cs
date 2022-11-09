using AutoMapper;
using MediatR;
using Orders.Application.Commands.OrderCreate;
using Orders.Application.Responses;
using Orders.Domain.Entities;
using Orders.Domain.Repositories;
using Orders.Infrastructure.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Orders.Application.Handlers
{
    public class OrderCreatehandler : IRequestHandler<OrderCreateCommond, OrderResponse>
    {
        private readonly IOrderRepository  _orderRespoitory;
        private readonly IMapper _mapper;
        public OrderCreatehandler(IOrderRepository orderRespoitory,IMapper mapper)
        {
            _orderRespoitory = orderRespoitory;
            _mapper = mapper;
        }
        public async Task<OrderResponse> Handle(OrderCreateCommond request, CancellationToken cancellationToken)
        {
            var orderEntity = _mapper.Map<Order>(request);
            if (orderEntity == null)
                throw new ApplicationException("Entity coul not be mapped!");

            var order = await _orderRespoitory.AddAsync(orderEntity);
            var response = _mapper.Map<OrderResponse>(order);

            return response;
        }
    }
}
