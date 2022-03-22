using Basket.API.Entities;
using Basket.API.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Net;
using Basket.API.GrpcService;
using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _repository;
        private readonly DiscountGrpcService _discountGrpcService;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;

        public BasketController(IBasketRepository repository, DiscountGrpcService discountGrpcService, IMapper mapper,IPublishEndpoint publishEndpoint)
        {
            _repository = repository;
            _discountGrpcService = discountGrpcService;
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet("{userName}",Name ="GetBasket")]
        [ProducesResponseType(typeof(ShoppingCart),(int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
        {
            var basket= await _repository.GetBasket(userName);
            return Ok(basket ?? new ShoppingCart(userName));
        }
        
        [HttpPost]
        [ProducesResponseType(typeof (ShoppingCart),(int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart basket)
        {
            foreach (var item in basket.Items)
            {
                var coupon = await _discountGrpcService.GetDiscount(item.ProductName);
                item.Price -= coupon.Amount;
            }

            return Ok(await _repository.UpdateBasket(basket));
        }

        [HttpDelete("{userName}",Name ="DeleteBasket")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> DeleteBasket(string userName)
        {
            await _repository.DeleteBasket(userName);
            return Ok();
        }

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Checkout([FromBody] BasketChekout basketChekout)
        {
            var basket=await _repository.GetBasket(basketChekout.UserName);
            if (basket == null)
                return BadRequest();

            var eventMessage = _mapper.Map<BasketCheckoutEvent>(basketChekout);
            eventMessage.TotalPrice = basket.TotalPrice;
            await _publishEndpoint.Publish(eventMessage);

            await _repository.DeleteBasket(basketChekout.UserName);
            return Accepted();
        }
    }
}
