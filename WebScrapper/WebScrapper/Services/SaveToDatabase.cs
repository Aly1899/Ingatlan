
//namespace WebScrapper.Services
//{
//    public class SaveToDatabase
//    {
//        public async void SaveRealEstate()
//        {
//            using var context = new AdContext();
//            await GetAllExistingRealEstate(context);
//        }
//        private async Task<IReadOnlyList<AdModel>> GetAllExistingRealEstate(AdContext context)
//        {

//            context.Database.EnsureCreated();
//            var adsFromWeb = new GetDataFromWebpage();
//            var re = await context.AdModels.ToListAsync();

//            Console.WriteLine($"We have {re.Count} car(s).");
//            return re;
//        }

//    }
//}
