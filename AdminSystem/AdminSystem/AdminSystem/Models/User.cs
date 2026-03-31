namespace AdminSystem.Models
{
    public class User
    {
        public int     UserId          { get; set; }
        public string  FirstName       { get; set; }
        public string  LastName        { get; set; }
        public string  Email           { get; set; }
        public string  Phone           { get; set; }
        public string  PasswordHash    { get; set; }
        public int?    DefaultAddressId{ get; set; }
        public bool    IsActive        { get; set; }
        public bool    IsEmailVerified { get; set; }
        public System.DateTime? LastLoginAt { get; set; }
        public System.DateTime CreatedAt   { get; set; }
        public System.DateTime? UpdatedAt  { get; set; }
        public string  Role            { get; set; }
        public string  FullName        => FirstName + " " + LastName;
    }

    public static class RoleNames
    {
        public const string Admin    = "Admin";
        public const string Manager  = "Manager";
        public const string Staff    = "Staff";
        public const string Customer = "Customer";
    }
}
