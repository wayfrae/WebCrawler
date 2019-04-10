using CrawlerApp.DataStore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CrawlerApp.Console
{
    public interface ICrawler
    {
        Task<bool> Start(Uri urlToCrawl);
        void Stop();
    }
}
