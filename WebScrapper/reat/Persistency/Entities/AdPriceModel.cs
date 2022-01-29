using System;

namespace reat.Persistency.Entities
{
    public class AdPriceModel
    {
        public Guid AdPriceId { get; set; }
        public Guid AdId { get; set; }
        public string? OrigAdId { get; set; }
        public Decimal? Price { get; set; }
        public DateTime? EntryDate { get; set; }
    }
}

