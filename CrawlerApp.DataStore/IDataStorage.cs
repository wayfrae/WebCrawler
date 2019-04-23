using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrawlerApp.DataStore
{
    public interface IDataStorage<T> where T : EntityBase
    {
        void Create(T obj);
        T GetByID(int id);
        Task<IEnumerable<Link>> GetAll();
        void Update(T obj);
        void Delete(T obj);
    }
}
