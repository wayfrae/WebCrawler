using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrawlerApp.DataStore;

namespace CrawlerApp.DataStore
{
    public class Link : EntityBase
    {
        public string Address { get; set; }
        public string Response { get; set; }
        public List<string> FoundLinks { get; set; } 
        public bool IsCrawled { get; set; }
    }
}
