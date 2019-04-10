using System;
using System.Collections.Generic;
using System.Text;

namespace CrawlerApp.DataStore
{
    public class DataStorage : IDataStorage<Link>
    {
        private List<Link> _links { get; set; }

        public void Create(Link obj)
        {
            _links.Add(obj);
        }

        public void Delete(Link obj)
        {
            _links.Remove(obj);
        }

        public IEnumerable<Link> GetAll()
        {
            return _links;
        }

        public Link GetByID(int id)
        {
            try
            {
               return _links.Find(x => x.ID == id);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool Update(Link obj)
        {
            try
            {
                var temp = _links.Find(x => x.Address == obj.Address && x.FoundOn == obj.FoundOn);
                if(temp == null)
                {
                    return false;
                }
                temp = obj;
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataStorage(List<Link> list)
        {
            _links = list;
        }
    }
}
