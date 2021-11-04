using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace restate.API.DTO
{
    public class PriceChangeDTO
    {
        public Guid RealEstateId { get; set; }
        public string AdId { get; set; }
        public string AdType { get; set; }
        public long OldPirice { get; set; }
        public long NewPirice { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public decimal? Area { get; set; }
        public int? PlotSize { get; set; }
        public DateTime? Date { get; set; }
    }
}
