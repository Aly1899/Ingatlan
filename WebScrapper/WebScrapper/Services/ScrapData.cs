using System;

namespace WebScrapper.Services
{
    public class ScrapData
    {
        public Guid AdId { get; set; }
        public string? OrigAdId { get; set; }
        public Decimal? Price { get; set; }
        public string? AdType { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public Decimal? Area { get; set; }
        public int? PlotSize { get; set; }
        public bool LeaseRights { get; set; }
        public string? Balcony { get; set; }
        public DateTime? Created { get; set; }
    }

    //public class Page
    //{
    //    public IList<RealEstate> RealEstates { get; set; }
    //}
}
