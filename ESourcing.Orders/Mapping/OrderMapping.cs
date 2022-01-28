using AutoMapper;
using EventBusRabbitMq.Events;
using Orders.Application.Commands.OrderCreate;

namespace ESourcing.Orders.Mapping
{
    public class OrderMapping : Profile
    {
        public OrderMapping()
        {
            CreateMap<OrderCreateEvent, OrderCreateCommond>().ReverseMap();
        }
    }
}
