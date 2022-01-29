using AutoMapper;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebScrapper.Common;
using WebScrapper.Context;
using WebScrapper.Models;

namespace WebScrapper.Services
{
    public class GetDataFromWebpage
    {
        private readonly HttpClient _httpClient;
        private readonly AdContext _adContext;
        private readonly IMapper _mapper;

        public GetDataFromWebpage(HttpClient httpClient, AdContext adContext, IMapper mapper)
        {
            _httpClient = httpClient;
            _adContext = adContext;
            _mapper = mapper;
        }

        public async Task Init()
        {
            await SaveData(Constants.Flat, Constants.FlatUri);
            await SaveData(Constants.House, Constants.House);
            await SaveData(Constants.Plot, Constants.PlotUri);
            await SaveData(Constants.PlotOther, Constants.PlotOtherUri);
        }
        public async Task<List<ScrapData>> GetData(string type, string url)
        {
            var pages = await GetMaxPage(_httpClient, url);

            var scrapData = new List<ScrapData>();

            var estateType = type;
            for (var p = 0; p <= 0; p++)
            {
                Console.WriteLine($"Dowloaded page: {p + 1}");
                var htmlDoc = await GetAdHtmlDocument(url, p + 1);
                var nodes = htmlDoc.DocumentNode.SelectNodes(".//div[contains(@class,'listing ')]");
                for (int i = 0; i < nodes.Count; i++)
                {
                    scrapData.Add(RealEstateMapper(nodes[i], estateType));
                }
            }
            return scrapData;
        }
        public async Task<HtmlDocument> GetAdHtmlDocument(string url, int page)
        {
            var fullUrl = $"{url}?page={page}";
            var response = await _httpClient.GetAsync(fullUrl);
            if (!response.IsSuccessStatusCode)
            {
                return new HtmlDocument();
            }
            var htmlBody = await response.Content.ReadAsStringAsync();
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlBody);
            return htmlDoc;
        }
        public async Task<int> GetMaxPage(HttpClient _httpClient, string url)
        {
            var htmlDocForPages = await GetAdHtmlDocument(url, 1);
            var pagesTxt = htmlDocForPages.DocumentNode.SelectSingleNode(".//*[contains(@class,'pagination__page-number')]").InnerText;
            int maxPage = Convert.ToInt32(pagesTxt.Split(" ")[3]);
            return maxPage;
        }
        public ScrapData RealEstateMapper(HtmlNode node, string estateType)
        {
            var re = new ScrapData();
            var reId = node.GetAttributeValue("data-id", "").ToString();
            var address = node.SelectSingleNode(".//div[@class = 'listing__address']");
            re.AdId = Guid.NewGuid();
            re.OrigAdId = reId;
            re.AdType = estateType;
            if (address != null)
            {
                re.Address = address.InnerText.Trim();
                var commaIndex = address.InnerText.LastIndexOf(",");
                if (commaIndex == -1)
                {
                    re.City = address.InnerText.Trim();
                }
                else
                {
                    re.City = address.InnerText.Substring(commaIndex + 1).Trim();
                }
            }

            var price = node.SelectSingleNode(".//div[@class = 'price']");
            if (price != null)
            {
                re.Price = Convert.ToDecimal(price.InnerText.Split(" ")[1]);
            }

            var area = node.SelectSingleNode(".//div[contains(@class , 'listing__data--area-size')]");
            if (area != null)
            {
                re.Area = Convert.ToDecimal(area.InnerText.Split(" ")[1]);
            }
            var plotSize = node.SelectSingleNode(".//div[contains(@class , 'listing__data--plot-size')]");
            if (plotSize != null)
            {
                var plotSizeTxt = plotSize.InnerText.ToString();
                re.PlotSize = Convert.ToInt32(plotSizeTxt.Substring(0, plotSizeTxt.Count() - 9).Replace(" ", ""));
            }
            var balcony = node.SelectSingleNode(".//div[contains(@class,'listing__data--balcony-size')]");
            if (balcony != null)
            {
                re.Balcony = balcony.InnerText;
            }
            var leasing = node.SelectSingleNode(".//div[contains(@class , 'label--alert')]");
            re.LeaseRights = leasing == null ? false : true;
            re.Date = DateTime.UtcNow;
            return re;
        }

        public async Task SaveData(string type, string uri)
        {
            var adsFromWeb = await GetData(type, uri);
            var adsFromDb = await _adContext.AdModels.ToListAsync();
            var priceFromDb = await _adContext.AdPriceModels.ToListAsync();
            await using var transaction = await _adContext.Database.BeginTransactionAsync();

            foreach (var adWeb in adsFromWeb)
            {
                var adExisted = adsFromDb.FirstOrDefault(adDb =>
                    adDb.OrigAdId == adWeb.OrigAdId && adDb.Address == adWeb.Address);
                if (adExisted == null)
                {
                    var newAd = _mapper.Map<AdModel>(adWeb);
                    var newPrice = new AdPriceModel()
                    {
                        AdId = adWeb.AdId,
                        AdPriceId = Guid.NewGuid(),
                        OrigAdId = adWeb.OrigAdId,
                        Price = adWeb.Price,
                        EntryDate = DateTime.UtcNow
                    };
                    _adContext.Add(newAd);
                    _adContext.Add(newPrice);
                }
                else
                {
                    var adPricesExisted = priceFromDb
                        .Where(p => p.AdId == adExisted.AdId)
                        .OrderByDescending(p => p.EntryDate)
                        .ToList();
                    if (adPricesExisted.First().Price == adWeb.Price) continue;
                    var newPrice = new AdPriceModel()
                    {
                        AdId = adExisted.AdId,
                        AdPriceId = Guid.NewGuid(),
                        OrigAdId = adExisted.OrigAdId,
                        Price = adWeb.Price,
                        EntryDate = DateTime.UtcNow
                    };
                    _adContext.Add(newPrice);
                }
            }
            await _adContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
    }
}
