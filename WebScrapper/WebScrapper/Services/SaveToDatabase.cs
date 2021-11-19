using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScrapper.Context;
using WebScrapper.Models;

namespace WebScrapper.Services
{
    public class SaveToDatabase
    {
        public async void SaveRealEstate()
        {
            using var context = new RealEstateContext();
            await GetAllExistingRealEstate(context);
        }
        private async Task<IReadOnlyList<RealEstate>> GetAllExistingRealEstate(RealEstateContext context)
        {
            
            context.Database.EnsureCreated();
            var re = await context.RealEstates.ToListAsync();

            Console.WriteLine($"We have {re.Count} car(s).");
            return re;
        }

    }
}
