using System;
using System.Collections.Generic;

#nullable disable

namespace Application.Model
{
    public class AdPrice
    {
        public Guid AdPriceId { get; set; }
        public Guid RealEstateId { get; set; }
        public string AdId { get; set; }
        public decimal? OldPrice { get; set; }
        public decimal? NewPrice { get; set; }
        public DateTime? EntryDate { get; set; }
        public Guid FetchId { get; set; }
    }
}
