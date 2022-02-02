using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebScrapper.Common;
using WebScrapper.Context;
using WebScrapper.Models;
using WebScrapper.Services;

namespace WebScrapper
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddHttpClient();
            services.AddAutoMapper(typeof(AutoMapping));
            var serviceProvider = services.BuildServiceProvider();
            var client = serviceProvider.GetService<HttpClient>();
            var mapper = serviceProvider.GetRequiredService<IMapper>();
            GetDataFromWebpage gData;
            using (var context = new AdContext())
            {

                context.Database.EnsureCreated();
                var re = context.AdModels.ToArray();

                Console.WriteLine($"We have {re.Length} car(s).");
                gData = new GetDataFromWebpage(client, context, mapper);
                await gData.SaveData(Constants.Flat, Constants.FlatUri);
                await gData.SaveFetchDate(Constants.Flat);
                await gData.SaveData(Constants.House, Constants.HouseUri);
                await gData.SaveFetchDate(Constants.House);
                await gData.SaveData(Constants.Plot, Constants.PlotUri);
                await gData.SaveFetchDate(Constants.Plot);
                await gData.SaveData(Constants.PlotOther, Constants.PlotOtherUri);
                await gData.SaveFetchDate(Constants.PlotOther);
                await gData.SetInactiveAds();
            }

            //var result = await gData.GetData("https://ingatlan.com/szukites/elado+lakas+budapest+30-50-mFt+4-szoba-felett");


            Console.WriteLine();
        }


    }

    class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<ScrapData, AdModel>();
        }
    }
}
