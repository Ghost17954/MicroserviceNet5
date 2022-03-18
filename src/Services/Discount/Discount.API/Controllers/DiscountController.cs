using Discount.API.Entities;
using Discount.API.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace Discount.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountRepository _respository;

        public DiscountController(IDiscountRepository repository)
        {
            _respository = repository;
        }

        [HttpGet("{productName}", Name = "GetCoupon")]
        [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Coupon>> GetCoupon(string productName)
        {
            return Ok(await _respository.GetCoupon(productName));
        }

        [HttpDelete]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> DeleteCoupon(string productName)
        {
            return Ok(await _respository.DeleteDiscount(productName));
        }

        [HttpPost]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> CreateDiscount(Coupon coupon)
        {
            return Ok(await _respository.CreateDiscount(coupon));
        }

        [HttpPut]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> UpdateDiscount(Coupon coupon)
        {
            return Ok(await _respository.UpdateDiscount(coupon));
        }
    }
}
