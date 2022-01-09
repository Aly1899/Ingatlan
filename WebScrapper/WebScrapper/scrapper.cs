using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebScrapper.Context;
using WebScrapper.Models;

namespace WebScrapper
{
    public class Scrapper
    {
        public static async Task GetAdsData()
        {

            using (var client = new HttpClient())
            {
                using (var context = new AdContext())
                {
                    using var transaction = context.Database.BeginTransaction();

                    string url = "https://ingatlan.com/szukites/elado+lakas+budapest+30-50-mFt+4-szoba-felett";
                    //string url = "https://ingatlan.com/lista/elado+telek+50-mFt-ig";
                    var pages = await GetMaxPage(client, url);

                    var ads = new List<AdModel>();

                    //for (var p = 1; p <= pages; p++)
                    var lastFetchDate = await context.FetchDates.OrderByDescending(f => f.EntryDate).FirstOrDefaultAsync();
                    //var newFetchId = lastFetchDate==null?1:lastFetchDate.Id+1;
                    var newFetchId = Guid.NewGuid();
                    var estateType = Common.Constants.Plot;
                    for (var p = 1; p <= pages; p++)
                    {
                        Console.WriteLine($"Page: {p}");
                        var htmlDoc = await GetAdHtmlDocument(client, url, p);
                        var nodes = htmlDoc.DocumentNode.SelectNodes(".//div[contains(@class,'listing ')]");
                        for (int i = 0; i < nodes.Count; i++)
                        {
                            await FillAd(estateType, context, nodes[i], newFetchId);

                        }
                    }
                    var t = context.ChangeTracker.HasChanges();
                    context.ChangeTracker.DetectChanges();

                    if (context.ChangeTracker.HasChanges())
                    {
                        var fetchDate = new FetchDate()
                        {
                            Id = newFetchId,
                            EntryDate = DateTime.UtcNow,
                            EstateType = estateType
                        };
                        context.Add(fetchDate);
                    }
                    await context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }

            }
        }
        private static async Task FillAd(string estateType, AdContext reContext, HtmlNode node, Guid newFetchId)
        {
            var ad = new AdModel();
            var adId = node.GetAttributeValue("data-id", "").ToString();
            var address = node.SelectSingleNode(".//div[@class = 'listing__address']");
            var findAd = reContext.AdModels.FirstOrDefault(r => r.OrigAdId == adId && r.Address == address.InnerText.Trim());
            if (findAd == null)
            {
                ad.AdId = Guid.NewGuid();
                ad.OrigAdId = adId;

                ad.AdType = estateType;
                if (address != null)
                {
                    ad.Address = address.InnerText.Trim();
                    var commaIndex = address.InnerText.LastIndexOf(",");
                    if (commaIndex == -1)
                    {
                        ad.City = address.InnerText.Trim();

                    }
                    else
                    {
                        ad.City = address.InnerText.Substring(commaIndex + 1).Trim();
                    }
                }

                var price = node.SelectSingleNode(".//div[@class = 'price']");
                if (price != null)
                {
                    var newPrince = new AdPriceModel()
                    {
                        AdPriceId = new Guid(),
                        AdId = ad.AdId,
                        OrigAdId = ad.OrigAdId,
                        Price = Convert.ToDecimal(price.InnerText.Split(" ")[1]),
                        EntryDate = DateTime.UtcNow,
                    };
                    reContext.Add(newPrince);
                    await reContext.SaveChangesAsync();
                }

                var area = node.SelectSingleNode(".//div[contains(@class , 'listing__data--area-size')]");
                if (area != null)
                {
                    ad.Area = Convert.ToDecimal(area.InnerText.Split(" ")[1]);
                }

                var plotSize = node.SelectSingleNode(".//div[contains(@class , 'listing__data--plot-size')]");
                if (plotSize != null)
                {
                    var plotSizeTxt = plotSize.InnerText.ToString();
                    ad.PlotSize = Convert.ToInt32(plotSizeTxt.Substring(0, plotSizeTxt.Count() - 9).Replace(" ", ""));
                }

                //ad.PricePerSqm = Math.Round((ad.Price.Value / ad.Area.Value) * 1000000, 2);

                var balcony = node.SelectSingleNode(".//div[contains(@class,'listing__data--balcony-size')]");
                if (balcony != null)
                {
                    ad.Balcony = balcony.InnerText;
                }
                var leasing = node.SelectSingleNode(".//div[contains(@class , 'label--alert')]");
                ad.LeaseRights = leasing == null ? false : true;
                ad.Date = DateTime.UtcNow;
                reContext.AdModels.Add(ad);
                var e = reContext.ChangeTracker.Entries();
            }
            else
            {
                var adPrice = reContext.AdPriceModels
                    .Where(a => a.AdId == findAd.AdId)
                    .OrderByDescending(a => a.EntryDate)
                    .Take(1)
                    .ToList();

                var price = node.SelectSingleNode(".//div[@class = 'price']");
                if (price != null)
                {
                    var actualPrice = Convert.ToDecimal(price.InnerText.Split(" ")[1]);
                    if (adPrice[0].Price != actualPrice)
                    {
                        var newPrice = new AdPriceModel()
                        {
                            AdPriceId = new Guid(),
                            AdId = findAd.AdId,
                            OrigAdId = findAd.OrigAdId,
                            Price = adPrice[0].Price,
                            EntryDate = DateTime.UtcNow,
                        };
                        Console.WriteLine($"Price update of : {findAd.AdId}");
                        reContext.Add(newPrice);
                    }
                }

            }

        }
        public static async Task<HtmlDocument> GetAdHtmlDocument(HttpClient client, string url, int page)
        {
            var fullUrl = $"{url}?page={page}";
            var response = await client.GetAsync(fullUrl);
            if (!response.IsSuccessStatusCode)
            {
                return new HtmlDocument();
            }
            var htmlBody = await response.Content.ReadAsStringAsync();
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlBody);
            return htmlDoc;
        }
        public static async Task<int> GetMaxPage(HttpClient client, string url)
        {
            var htmlDocForPages = await GetAdHtmlDocument(client, url, 1);
            var pagesTxt = htmlDocForPages.DocumentNode.SelectSingleNode(".//*[contains(@class,'pagination__page-number')]").InnerText;
            int maxPage = Convert.ToInt32(pagesTxt.Split(" ")[3]);
            return maxPage;
        }

        public static void DisplayStates(IEnumerable<EntityEntry> entries)
        {
            foreach (var entry in entries)
            {
                Console.WriteLine($"Entity: {entry.Entity.GetType().Name},State: { entry.State.ToString()}");
            }
        }
    }
}
