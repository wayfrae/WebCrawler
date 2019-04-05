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
        public bool IsCrawled { get; set; }
        public List<AssociatedLink> AssociatedLinks { get; set; }
        
        public Link(List<AssociatedLink> list)
        {
            AssociatedLinks = list;
        }

        public Link(List<AssociatedLink> list, string address)
        {
            AssociatedLinks = list;
            Address = address;
        }
                
    }
}
