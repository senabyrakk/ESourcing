using MediatR;
using Orders.Application.Responses;
using System.Collections.Generic;

namespace Orders.Application.Queries
{
    public class GetOrdersBySellerNameQuery : IRequest<IEnumerable<OrderResponse>>
    {
        public string UserName { get; set; }

        public GetOrdersBySellerNameQuery(string userName)
        {
            UserName = userName;
        }


    }
}
