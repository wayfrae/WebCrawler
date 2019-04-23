using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CrawlerApp.DataStore
{
    public class DataStorageLocal : IDataStorage<Link>
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

        public async Task<IEnumerable<Link>> GetAll()
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

        public void Update(Link obj)
        {
            try
            {
                var temp = _links.Find(x => x.Address == obj.Address && x.FoundOn == obj.FoundOn);
                
            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataStorageLocal(List<Link> list)
        {
            _links = list;
        }
    }
}
