using Basket.API.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Basket.API.Repository
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _distributedCache;
        public BasketRepository(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }
        public async Task DeleteBasket(string userName)
        {
            await _distributedCache.RemoveAsync(userName);
        }

        public async Task<ShoppingCart> GetBasket(string userName)
        {
            var basket= await _distributedCache.GetStringAsync(userName);
            if (basket == null)
                return null;
            return JsonConvert.DeserializeObject<ShoppingCart>(basket);
        }

        public async Task<ShoppingCart> UpdateBasket(ShoppingCart shoppingCart)
        {
            await _distributedCache.SetStringAsync(shoppingCart.UserName, JsonConvert.SerializeObject(shoppingCart));

            return await GetBasket(shoppingCart.UserName);
        }
    }
}
