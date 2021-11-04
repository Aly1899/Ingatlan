using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using restate.Context;
using restate.WireUp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace restate.Extentions
{
    public static class ServiceExtentions
    {
        public static void ConfigureCors(this IServiceCollection service)
        {
            service.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                builder => builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            });
        }
        public static void ConfigureIISIntegration(this IServiceCollection service)
        {
            service.Configure<IISOptions>(options =>
            {

            });
        }

        public static void ConfigureSqlServerContext(this IServiceCollection service, IConfiguration config)
        {
            var connectionSring = config["ConnectionStrings:SqlServer"];
            service.AddDbContext<RealEstateContext>(o => o.UseSqlServer(connectionSring));
        }
    }

}
