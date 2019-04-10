using CrawlerApp.DataStore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrawlerApp.Console
{
    class Scheduler : IScheduler
    {
        private List<Link> _links;
        private object _lock;

        public Scheduler(List<Link> list, object listLock)
        {
            _links = list;
            _lock = listLock;
        }

        public Link GetNext()
        {                        
            lock (_lock)
            {
                var link = _links[0];
                _links.RemoveAt(0);
                return link;
            }
        }

        public bool HasNext()
        {
            if(_links.Count == 0)
            {
                return false;
            }
            return true;
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
