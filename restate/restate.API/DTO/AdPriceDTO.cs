using System;
using System.Collections.Generic;
using System.Text;

namespace restate.API.DTO
{
    public class AdPriceDTO
    {
        public Guid AdPriceId { get; set; }
        public Guid RealEstateId { get; set; }
        public string AdId { get; set; }
        public decimal? OldPrice { get; set; }
        public decimal? NewPrice { get; set; }
        public DateTime? EntryDate { get; set; }
        public Guid FetchDateId { get; set; }
    }
}
