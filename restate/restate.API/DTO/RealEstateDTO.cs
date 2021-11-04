using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace restate.API.DTO
{
    public class RealEstateDTO
    {
        public Guid RealEstateId { get; set; }
        public string AdId { get; set; }
        public string AdType { get; set; }
        public List<AdPriceDTO> AdPrices { get; set; }
        public decimal? PricePerSqm { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public decimal? Area { get; set; }
        public int? PlotSize { get; set; }
        public bool LeaseRights { get; set; }
        public string Balcony { get; set; }
        public DateTime? Date { get; set; }
    }
}
