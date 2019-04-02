using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrawlerApp.DataStore;

namespace CrawlerApp.Client.Models
{
    public class Link : EntityBase
    {
        public string Address { get; set; }
        public string Response { get; set; }
        public bool IsCrawled { get; set; }
    }
}
