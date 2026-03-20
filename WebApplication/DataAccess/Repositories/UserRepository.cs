using Microsoft.EntityFrameworkCore;
using WebApplication.DataAccess.Context;
using WebApplication.Models.Entities;

namespace WebApplication.DataAccess.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<bool> EmailExistsAsync(string email);
    }

    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context) { }

        public async Task<User?> GetByEmailAsync(string email)
            => await _dbSet.FirstOrDefaultAsync(u => u.Email == email && u.IsActive);

        public async Task<bool> EmailExistsAsync(string email)
            => await _dbSet.AnyAsync(u => u.Email == email);
    }
}
