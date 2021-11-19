using System;
using System.Linq;
using System.Threading.Tasks;
using WebScrapper.Context;
using WebScrapper.Services;

namespace WebScrapper
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using (var context = new RealEstateContext())
            {

                context.Database.EnsureCreated();
                var re = context.RealEstates.ToArray();
                
                Console.WriteLine($"We have {re.Length} car(s).");
            }

            var gData = new GetDataFromWebpage();
            await gData.GetData("https://ingatlan.com/szukites/elado+lakas+budapest+30-50-mFt+4-szoba-felett");


            Console.WriteLine();
        }
    }
}
