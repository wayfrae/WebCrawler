using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CrawlerApp.DataStore
{
    public interface ICrawler
    {
        Task Start(Uri urlToCrawl);
        Task<EntityBase> Crawl(Uri urlToCrawl, EntityBase crawlerObjective);
    }
}
