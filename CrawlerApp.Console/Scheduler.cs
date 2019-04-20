using CrawlerApp.DataStore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrawlerApp.Console
{
    class Scheduler : IScheduler
    {
        private readonly List<Link> _links;
        private readonly object _lock;

        public Scheduler(List<Link> list, object listLock)
        {
            _links = list;
            _lock = listLock;
        }

        public Link GetNext()
        {                        
            lock (_lock)
            {
                if (_links.Count == 0) return null;
                Link link = _links[0];
                _links.RemoveAt(0);
                return link;
            }
        }

        public bool HasNext()
        {
            lock (_lock)
            {
                return _links.Count != 0;
            }
        }
        
        public void Push(Link obj)
        {
            lock (_lock)
            {
                _links.Add(obj);
            }
        }                
    }
}
