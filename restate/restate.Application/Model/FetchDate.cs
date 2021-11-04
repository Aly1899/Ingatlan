using System;
using System.Collections.Generic;

#nullable disable

namespace Application.Model
{
    public class FetchDate
    {
        public Guid Id { get; set; }
        public DateTime EntryDate { get; set; }
        public string EstateType { get; set; }
    }
}
