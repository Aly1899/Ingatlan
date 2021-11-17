using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebScrapper.Models;
using Serilog;

namespace WebScrapper.Services
{
    public class GetDataFromWebpage
    {
        public async Task GetData(string url)
        {

            using var client = new HttpClient();
            var pages = await GetMaxPage(client, url);

            var scrapData = new ScrapData();
            scrapData.RealEstates = new List<RealEstate>();

            var estateType = Common.Constants.Flat;
            for (var p = 0; p <= pages - 12; p++)
            {
                Console.WriteLine($"Dowloaded page: {p+1}");
                var htmlDoc = await GetAdHtmlDocument(client, url, p + 1);
                var nodes = htmlDoc.DocumentNode.SelectNodes(".//div[contains(@class,'listing ')]");
                for (int i = 0; i < nodes.Count; i++)
                {
                    scrapData.RealEstates.Add(RealEstateMapper(nodes[i], estateType));
                }
            }
        }
        public async Task<HtmlDocument> GetAdHtmlDocument(HttpClient client, string url, int page)
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
        public async Task<int> GetMaxPage(HttpClient client, string url)
        {
            var htmlDocForPages = await GetAdHtmlDocument(client, url, 1);
            var pagesTxt = htmlDocForPages.DocumentNode.SelectSingleNode(".//*[contains(@class,'pagination__page-number')]").InnerText;
            int maxPage = Convert.ToInt32(pagesTxt.Split(" ")[3]);
            return maxPage;
        }
        public RealEstate RealEstateMapper(HtmlNode node, string estateType)
        {
            var re = new RealEstate();
            var reId = node.GetAttributeValue("data-id", "").ToString();
            var address = node.SelectSingleNode(".//div[@class = 'listing__address']");
            re.RealEstateId = Guid.NewGuid();
            re.AdId = reId;
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
            var price =  node.SelectSingleNode(".//div[@class = 'price']");
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

    }
}
