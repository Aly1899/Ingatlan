using reat.Persistency.Entities;
using System;
using System.Collections.Generic;

namespace reat.DTO
{
    public class GetNewInactiveAdsQueryResult
    {
        public Guid AdId { get; set; }
        public string? OrigAdId { get; set; }
        public string? AdType { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public Decimal? Area { get; set; }
        public int? PlotSize { get; set; }
        public bool LeaseRights { get; set; }
        public string? Balcony { get; set; }
        public DateTime? Date { get; set; }
        public IReadOnlyList<AdPriceModel> AdPrices { get; set; }
    }
}
