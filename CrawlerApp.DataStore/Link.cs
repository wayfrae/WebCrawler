﻿using System;
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
        public DateTime Date { get; set; }
        public string FoundOn { get; set; }
        
                
    }
}
