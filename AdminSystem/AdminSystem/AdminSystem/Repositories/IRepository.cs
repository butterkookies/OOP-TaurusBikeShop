using System.Collections.Generic;

namespace AdminSystem.Repositories
{
    public interface IRepository<T> where T : class
    {
        T GetById(int id);
        IEnumerable<T> GetAll();
        int Insert(T entity);
        void Update(T entity);
        void Delete(int id);
    }
}
