using System;
using System.Collections.Generic;

#nullable disable

namespace Application.Model
{
    public class RealEstate
    {
        public Guid RealEstateId { get; set; }
        public string AdId { get; set; }
        public string AdType { get; set; }
        public decimal? Price { get; set; }
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
