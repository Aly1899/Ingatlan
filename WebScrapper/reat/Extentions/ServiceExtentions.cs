using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using reat.Persistency;

namespace reat.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigurePostgresSqlContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration["Postgres"];
            services.AddDbContext<AdContext>(o => o.UseNpgsql(connectionString));
        }
    }
}
