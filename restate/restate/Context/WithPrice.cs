using System;
using System.Collections.Generic;

#nullable disable

namespace restate.Context
{
    public partial class WithPrice
    {
        public string AdId { get; set; }
        public string Address { get; set; }
        public decimal? Price { get; set; }
    }
}
