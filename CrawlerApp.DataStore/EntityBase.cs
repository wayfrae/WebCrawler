using System;
using System.Collections.Generic;
using System.Text;

namespace CrawlerApp.DataStore
{
    public class EntityBase
    {
        public int ID { get; protected set; }
        public string Address { get; set; }
        public string Response { get; set; }
        public bool IsCrawled { get; set; }
    }
}
