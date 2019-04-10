using CrawlerApp.DataStore;
using System.Collections.Generic;

namespace CrawlerApp.Console
{
    public interface IScheduler
    {
        Link GetNext();
        void Push(Link obj);
        bool HasNext();
    }
}