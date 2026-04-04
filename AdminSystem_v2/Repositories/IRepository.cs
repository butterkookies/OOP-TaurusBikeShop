namespace AdminSystem_v2.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<T?>             GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<int>            InsertAsync(T entity);
        Task                 UpdateAsync(T entity);
        Task                 DeleteAsync(int id);
    }
}
