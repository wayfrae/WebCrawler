using System;
using System.Collections.Generic;

namespace CrawlerApp.DataStore
{
    public interface IRepository<T> where T : EntityBase
    {
        void Create(T obj);
        T GetByID(int id);
        IEnumerable<T> GetAll();
        void Update(T obj);
        void Delete(T obj);
    }
}
