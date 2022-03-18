using Dapper;
using Discount.Grpc.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Threading.Tasks;

namespace Discount.Grpc.Repository
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IConfiguration _configuration;

        public DiscountRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var result = await connection.ExecuteAsync("Insert into coupon(ProductName,Description,Amount) values (@ProductName,@Description,@Amount)", new { ProductName = coupon.ProductName,Description=coupon.Description,Amount=coupon.Amount });

            if(result==0)
                return false;

            return true;
        }

        public async Task<bool> DeleteDiscount(string productName)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var result = await connection.ExecuteAsync("Delete from coupon where ProductName=@ProductName",new {ProductName=productName});

            if (result == 0)
                return false;

            return true;
        }

        public async Task<Coupon> GetCoupon(string productName)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>("select * from coupon where productname=@ProductName",new {ProductName=productName});

            if(coupon == null)
                return new Coupon { ProductName="NO Discount",Description="No Discount discription",Amount=0};

            return coupon;
        }

        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var result = await connection.ExecuteAsync("Update coupon set ProductName=@ProductName,Description=@Description,Amount=@Amount where Id=@Id", new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount,Id=coupon.ID });

            if (result == 0)
                return false;

            return true;
        }
    }
}
