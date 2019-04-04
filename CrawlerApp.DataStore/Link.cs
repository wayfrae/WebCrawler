using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrawlerApp.DataStore;

namespace CrawlerApp.DataStore
{
    public class Link : EntityBase
    {
        public List<string> AssociatedLinks { get; set; }
        
        public Link(List<string> list)
        {
            AssociatedLinks = list;
        }

        public Link(List<string> list, string address)
        {
            AssociatedLinks = list;
            Address = address;
        }
    }
}
