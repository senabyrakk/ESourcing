using AutoMapper;
using MediatR;
using Orders.Application.Queries;
using Orders.Application.Responses;
using Orders.Domain.Repositories;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Orders.Application.Handlers
{
    public class GetOrderByUserNameHandler : IRequestHandler<GetOrdersBySellerNameQuery, IEnumerable<OrderResponse>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public GetOrderByUserNameHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderResponse>> Handle(GetOrdersBySellerNameQuery request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetOrdersBySellerUserName(request.UserName);

            var response = _mapper.Map<IEnumerable<OrderResponse>>(order);

            return response;


        }
    }
}
