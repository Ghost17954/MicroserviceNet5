using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Discount.API.Extentions
{
    public static class HostExtention
    {
        
        public static IHost MigrateDatabse<TContxt>(this IHost host,int? retry = 0)
        {
            int retryForAvailability = retry.Value;

            using(var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var configuration=services.GetRequiredService<IConfiguration>();
                var logger = services.GetRequiredService<ILogger<TContxt>>();

                try
                {
                    logger.LogInformation($"Migrating postresql database");

                    var connection = new NpgsqlConnection(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
                    connection.Open();
                    using var command = new NpgsqlCommand
                    {
                        Connection = connection,
                    };

                    command.CommandText = "DROP TABLE IF EXISTS Coupon";
                    command.ExecuteNonQuery();

                    command.CommandText = @"Create table Coupon (Id serial primary key not null,ProductName varchar(24) not null,Description text,Amount int)";
                    command.ExecuteNonQuery();

                    command.CommandText = @"INSERT into coupon(ProductName,Description,Amount) values('IPhone X','IPhone discount',150)";
                    command.ExecuteNonQuery();

                    command.CommandText = @"INSERT into coupon(ProductName,Description,Amount) values('Samsung X','Samsung discount',100)";
                    command.ExecuteNonQuery();

                    logger.LogInformation($"Migration complete");
                }
                catch (NpgsqlException ex)
                {
                    logger.LogError(ex, "An error occured while migration database");

                    if (retryForAvailability < 50)
                    {
                        retryForAvailability++;
                        System.Threading.Thread.Sleep(2000);
                        MigrateDatabse<TContxt>(host, retryForAvailability);
                    }
                }

                return host;
            }
        }
    }
}
