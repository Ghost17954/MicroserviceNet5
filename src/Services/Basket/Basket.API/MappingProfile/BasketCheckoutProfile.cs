using AutoMapper;
using Basket.API.Entities;
using EventBus.Messages.Events;

namespace Basket.API.MappingProfile
{
    public class BasketCheckoutProfile:Profile
    {
        public BasketCheckoutProfile()
        {
            CreateMap<BasketChekout,BasketCheckoutEvent>().ReverseMap();
        }
    }
}
