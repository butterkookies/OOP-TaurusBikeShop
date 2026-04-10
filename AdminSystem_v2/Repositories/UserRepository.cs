using AdminSystem_v2.Models;
using Dapper;

namespace AdminSystem_v2.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public async Task<User?> GetByIdAsync(int id)
            => await QueryFirstOrDefaultAsync(
                "SELECT * FROM [User] WHERE UserId = @Id", new { Id = id });

        public async Task<IEnumerable<User>> GetAllAsync()
            => await QueryAsync(
                "SELECT * FROM [User] ORDER BY LastName, FirstName");

        public async Task<int> InsertAsync(User entity)
            => await ExecuteScalarAsync(
                @"INSERT INTO [User] (FirstName, LastName, Email, PhoneNumber, PasswordHash,
                    IsActive, IsWalkIn, CreatedAt)
                  VALUES (@FirstName, @LastName, @Email, @PhoneNumber, @PasswordHash,
                    @IsActive, @IsWalkIn, GETUTCDATE());
                  SELECT CAST(SCOPE_IDENTITY() AS INT);", entity);

        public async Task UpdateAsync(User entity)
            => await ExecuteAsync(
                @"UPDATE [User] SET FirstName=@FirstName, LastName=@LastName,
                    Email=@Email, PhoneNumber=@PhoneNumber, IsActive=@IsActive
                  WHERE UserId=@UserId", entity);

        public async Task DeleteAsync(int id)
            => await ExecuteAsync(
                "UPDATE [User] SET IsActive=0 WHERE UserId=@Id", new { Id = id });

        public async Task<User?> FindByEmailAsync(string email)
            => await QueryFirstOrDefaultAsync(
                "SELECT * FROM [User] WHERE Email=@Email AND IsActive=1",
                new { Email = email });

        public async Task<string> GetUserRoleAsync(int userId)
        {
            await using var conn = GetConnection();
            return await conn.QueryFirstOrDefaultAsync<string>(
                @"SELECT TOP 1 r.RoleName
                  FROM UserRole ur
                  INNER JOIN Role r ON ur.RoleId = r.RoleId
                  WHERE ur.UserId = @UserId",
                new { UserId = userId }) ?? string.Empty;
        }

        public async Task UpdateLastLoginAsync(int userId)
            => await ExecuteAsync(
                "UPDATE [User] SET LastLoginAt=GETUTCDATE() WHERE UserId=@UserId",
                new { UserId = userId });

        public async Task IncrementFailedLoginsAsync(int userId, int maxAttempts, int lockoutMinutes)
            => await ExecuteAsync(
                @"UPDATE [User]
                  SET FailedLoginAttempts = FailedLoginAttempts + 1,
                      LockoutUntil = CASE
                          WHEN FailedLoginAttempts + 1 >= @Max
                          THEN DATEADD(MINUTE, @Mins, GETUTCDATE())
                          ELSE LockoutUntil END
                  WHERE UserId = @UserId",
                new { UserId = userId, Max = maxAttempts, Mins = lockoutMinutes });

        public async Task ResetFailedLoginsAsync(int userId)
            => await ExecuteAsync(
                "UPDATE [User] SET FailedLoginAttempts = 0, LockoutUntil = NULL WHERE UserId = @UserId",
                new { UserId = userId });

        public async Task<bool> HasRoleAsync(int userId, string roleName)
        {
            await using var conn = GetConnection();
            int count = await conn.ExecuteScalarAsync<int>(
                @"SELECT COUNT(1) FROM UserRole ur
                  INNER JOIN Role r ON ur.RoleId = r.RoleId
                  WHERE ur.UserId = @UserId AND r.RoleName = @RoleName",
                new { UserId = userId, RoleName = roleName });
            return count > 0;
        }

        // ── Staff management ──────────────────────────────────────────────────

        public async Task<IEnumerable<User>> GetStaffUsersAsync()
            => await QueryAsync(
                @"SELECT u.*, ISNULL(r.RoleName, '') AS Role
                  FROM [User] u
                  INNER JOIN UserRole ur ON u.UserId  = ur.UserId
                  INNER JOIN Role     r  ON ur.RoleId = r.RoleId
                  WHERE r.RoleName IN ('Admin', 'Manager', 'Staff')
                  ORDER BY r.RoleName, u.LastName, u.FirstName");

        public async Task<IEnumerable<string>> GetAllRoleNamesAsync()
            => await QueryAsync<string>(
                "SELECT RoleName FROM Role ORDER BY RoleName");

        public async Task SetUserRoleAsync(int userId, string roleName)
        {
            await using var conn = GetConnection();
            await using var tx   = await conn.BeginTransactionAsync();
            try
            {
                // Remove any existing staff-role assignments for this user
                await conn.ExecuteAsync(
                    @"DELETE ur FROM UserRole ur
                      INNER JOIN Role r ON ur.RoleId = r.RoleId
                      WHERE ur.UserId = @UserId
                        AND r.RoleName IN ('Admin', 'Manager', 'Staff')",
                    new { UserId = userId }, tx);

                // Assign the new role
                await conn.ExecuteAsync(
                    @"INSERT INTO UserRole (UserId, RoleId, AssignedAt)
                      SELECT @UserId, r.RoleId, GETUTCDATE()
                      FROM Role r
                      WHERE r.RoleName = @RoleName",
                    new { UserId = userId, RoleName = roleName }, tx);

                await tx.CommitAsync();
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        public async Task ToggleActiveAsync(int userId, bool isActive)
            => await ExecuteAsync(
                "UPDATE [User] SET IsActive = @IsActive WHERE UserId = @UserId",
                new { UserId = userId, IsActive = isActive });

        public async Task CreateStaffAsync(User user, string passwordHash, string roleName)
        {
            await using var conn = GetConnection();
            await using var tx   = await conn.BeginTransactionAsync();
            try
            {
                int newId = await conn.ExecuteScalarAsync<int>(
                    @"INSERT INTO [User]
                        (FirstName, LastName, Email, PhoneNumber, PasswordHash,
                         IsActive, IsWalkIn, CreatedAt)
                      VALUES
                        (@FirstName, @LastName, @Email, @PhoneNumber, @PasswordHash,
                         1, 0, GETUTCDATE());
                      SELECT CAST(SCOPE_IDENTITY() AS INT);",
                    new
                    {
                        user.FirstName, user.LastName,
                        user.Email,    user.PhoneNumber,
                        PasswordHash = passwordHash,
                    }, tx);

                await conn.ExecuteAsync(
                    @"INSERT INTO UserRole (UserId, RoleId, AssignedAt)
                      SELECT @UserId, r.RoleId, GETUTCDATE()
                      FROM Role r
                      WHERE r.RoleName = @RoleName",
                    new { UserId = newId, RoleName = roleName }, tx);

                await tx.CommitAsync();
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        public async Task ResetPasswordAsync(int userId, string passwordHash)
            => await ExecuteAsync(
                "UPDATE [User] SET PasswordHash = @Hash WHERE UserId = @UserId",
                new { UserId = userId, Hash = passwordHash });
    }
}
