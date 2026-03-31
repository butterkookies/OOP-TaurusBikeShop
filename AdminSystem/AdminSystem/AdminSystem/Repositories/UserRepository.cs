using System.Collections.Generic;
using AdminSystem.Models;
using Dapper;
using System.Data.SqlClient;

namespace AdminSystem.Repositories
{
    public class UserRepository : Repository<User>, IRepository<User>
    {
        public User GetById(int id)
            => QueryFirstOrDefault(
                "SELECT * FROM [User] WHERE UserId = @Id", new { Id = id });

        public IEnumerable<User> GetAll()
            => Query("SELECT * FROM [User] ORDER BY LastName, FirstName");

        public int Insert(User entity)
            => ExecuteScalar(
                @"INSERT INTO [User] (FirstName, LastName, Email, Phone, PasswordHash,
                    IsActive, IsEmailVerified, CreatedAt)
                  VALUES (@FirstName, @LastName, @Email, @Phone, @PasswordHash,
                    @IsActive, @IsEmailVerified, GETUTCDATE());
                  SELECT CAST(SCOPE_IDENTITY() AS INT);", entity);

        public void Update(User entity)
            => Execute(
                @"UPDATE [User] SET FirstName=@FirstName, LastName=@LastName,
                    Email=@Email, Phone=@Phone, IsActive=@IsActive,
                    UpdatedAt=GETUTCDATE()
                  WHERE UserId=@UserId", entity);

        public void Delete(int id)
            => Execute(
                "UPDATE [User] SET IsActive=0 WHERE UserId=@Id", new { Id = id });

        public User FindByEmail(string email)
            => QueryFirstOrDefault(
                "SELECT * FROM [User] WHERE Email=@Email AND IsActive=1",
                new { Email = email });

        public IEnumerable<User> GetByRole(string roleName)
        {
            using (SqlConnection conn = GetConnection())
                return conn.Query<User>(
                    @"SELECT u.* FROM [User] u
                      INNER JOIN UserRole ur ON u.UserId=ur.UserId
                      INNER JOIN Role r ON ur.RoleId=r.RoleId
                      WHERE r.RoleName=@RoleName AND u.IsActive=1
                      ORDER BY u.LastName, u.FirstName",
                    new { RoleName = roleName });
        }

        public string GetUserRole(int userId)
        {
            using (SqlConnection conn = GetConnection())
                return conn.QueryFirstOrDefault<string>(
                    @"SELECT TOP 1 r.RoleName
                      FROM UserRole ur
                      INNER JOIN Role r ON ur.RoleId = r.RoleId
                      WHERE ur.UserId = @UserId",
                    new { UserId = userId });
        }

        public void UpdateLastLogin(int userId)
            => Execute(
                "UPDATE [User] SET LastLoginAt=GETUTCDATE() WHERE UserId=@UserId",
                new { UserId = userId });

        public bool HasRole(int userId, string roleName)
        {
            using (SqlConnection conn = GetConnection())
                return conn.ExecuteScalar<int>(
                    @"SELECT COUNT(1) FROM UserRole ur
                      INNER JOIN Role r ON ur.RoleId = r.RoleId
                      WHERE ur.UserId = @UserId AND r.RoleName = @RoleName",
                    new { UserId = userId, RoleName = roleName }) > 0;
        }
    }
}
