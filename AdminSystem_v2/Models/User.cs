namespace AdminSystem_v2.Models
{
    public class User
    {
        public int       UserId           { get; set; }
        public string    FirstName        { get; set; } = string.Empty;
        public string    LastName         { get; set; } = string.Empty;
        public string    Email            { get; set; } = string.Empty;
        public string    PhoneNumber      { get; set; } = string.Empty;
        public string    PasswordHash     { get; set; } = string.Empty;
        public int?      DefaultAddressId { get; set; }
        public bool      IsActive         { get; set; }
        public bool      IsWalkIn         { get; set; }
        public DateTime? LastLoginAt      { get; set; }
        public DateTime  CreatedAt        { get; set; }

        // Brute-force protection (VULN-003)
        public int       FailedLoginAttempts { get; set; }
        public DateTime? LockoutUntil        { get; set; }

        // Populated separately via GetUserRoleAsync — not a DB column
        public string Role { get; set; } = string.Empty;

        public string FullName  => $"{FirstName} {LastName}".Trim();
        public string Initials  => FirstName.Length > 0
            ? FirstName[0].ToString().ToUpper()
            : "A";
    }

    public class LoginResult
    {
        public bool   Success      { get; init; }
        public User?  User         { get; init; }
        public string ErrorMessage { get; init; } = string.Empty;

        public static LoginResult Ok(User user) => new() { Success = true, User = user };
        public static LoginResult Fail(string message) => new() { Success = false, ErrorMessage = message };
    }

    public static class RoleNames
    {
        public const string Admin    = "Admin";
        public const string Manager  = "Manager";
        public const string Staff    = "Staff";
        public const string Customer = "Customer";
    }
}
