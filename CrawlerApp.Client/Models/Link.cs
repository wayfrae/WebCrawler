using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace CrawlerApp.Client.Models
{
    public class Link : EntityBase
    {
        private List<AssociatedLink> _associatedLinks;

        public string Address { get; set; }
        public string Response { get; set; }
        public bool IsCrawled { get; set; }
        public List<AssociatedLink> AssociatedLinks { get => LazyLoader.Load(this, ref _associatedLinks); set => _associatedLinks = value; }

        private ILazyLoader LazyLoader { get; set; }

        private Link(ILazyLoader lazyLoader)
        {
            LazyLoader = lazyLoader;
        }
    }
}
