using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace CrawlerApp.Client.Models
{
    public class Link : EntityBase
    {
        public string Address { get; set; }
        public string Response { get; set; }
        public bool IsCrawled { get; set; }
        public DateTime Date { get; set; }
        public string FoundOn { get; set; }

    }
}
