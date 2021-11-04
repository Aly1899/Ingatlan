using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WebScrapper.Models
{
    public class FetchDate
    {
        public Guid Id { get; set; }
        public DateTime EntryDate { get; set; }
        public string EstateType { get; set; }
    }
}

