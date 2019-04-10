using System;
using System.Collections.Generic;

namespace CrawlerApp.DataStore
{
    public interface IDataStorage<T> where T : EntityBase
    {
        void Create(T obj);
        T GetByID(int id);
        IEnumerable<T> GetAll();
        bool Update(T obj);
        void Delete(T obj);
    }
}
