using AutoMapper;
using EventBus.Messages.Events;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;

namespace Ordering.API.MappingProfiles
{
    public class EventBusProfiles:Profile
    {
        public EventBusProfiles()
        {
            CreateMap<BasketCheckoutEvent,CheckoutOrderCommand>().ReverseMap();
        }
    }
}
