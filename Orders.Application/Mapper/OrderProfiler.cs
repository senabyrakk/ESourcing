using AutoMapper;
using Orders.Application.Commands.OrderCreate;
using Orders.Application.Responses;
using Orders.Domain.Entities;

namespace Orders.Application.Mapper
{
    public class OrderProfiler : Profile
    {
        public OrderProfiler()
        {
            CreateMap<Order, OrderCreateCommond>().ReverseMap();
            CreateMap<Order, OrderResponse>().ReverseMap();
        }
    }
}
