// WebApplication/DataAccess/Context/AppDbContext.Auth.cs
// Entity configurations: User, Role, UserRole, Address, OTPVerification, GuestSession

using Microsoft.EntityFrameworkCore;
using WebApplication.Models.Entities;

namespace WebApplication.DataAccess.Context;

public sealed partial class AppDbContext
{
    private static void ConfigureUser(ModelBuilder mb)
    {
        mb.Entity<User>(e =>
        {
            e.ToTable("User");
            e.HasKey(u => u.UserId);

            // Filtered unique index — email must be unique when non-NULL
            // (walk-in placeholder users have NULL email)
            e.HasIndex(u => u.Email)
                .IsUnique()
                .HasFilter("[Email] IS NOT NULL")
                .HasDatabaseName("UX_User_Email");

            e.HasIndex(u => u.IsActive)
                .HasDatabaseName("IX_User_IsActive");

            e.HasIndex(u => u.IsWalkIn)
                .HasDatabaseName("IX_User_IsWalkIn");

            e.HasIndex(u => u.PhoneNumber)
                .HasDatabaseName("IX_User_PhoneNumber");

            e.HasIndex(u => u.DefaultAddressId)
                .HasFilter("[DefaultAddressId] IS NOT NULL")
                .HasDatabaseName("IX_User_DefaultAddressId");

            // Circular FK: User.DefaultAddressId → Address.AddressId
            // IsRequired(false) + no cascade delete breaks the dependency cycle.
            // The Address side (Address.UserId → User.UserId) uses CASCADE DELETE.
            e.HasOne(u => u.DefaultAddress)
                .WithMany()
                .HasForeignKey(u => u.DefaultAddressId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);
        });
    }

    private static void ConfigureRole(ModelBuilder mb)
    {
        mb.Entity<Role>(e =>
        {
            e.ToTable("Role");
            e.HasKey(r => r.RoleId);

            e.HasIndex(r => r.RoleName)
                .IsUnique()
                .HasDatabaseName("IX_Role_RoleName");
        });
    }

    private static void ConfigureUserRole(ModelBuilder mb)
    {
        mb.Entity<UserRole>(e =>
        {
            e.ToTable("UserRole");
            e.HasKey(ur => ur.UserRoleId);

            // Unique pair — no duplicate role assignments per user
            e.HasIndex(ur => new { ur.UserId, ur.RoleId })
                .IsUnique()
                .HasDatabaseName("UX_UserRole_Pair");

            e.HasIndex(ur => ur.UserId)
                .HasDatabaseName("IX_UserRole_UserId");

            e.HasIndex(ur => ur.RoleId)
                .HasDatabaseName("IX_UserRole_RoleId");

            e.HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigureAddress(ModelBuilder mb)
    {
        mb.Entity<Address>(e =>
        {
            e.ToTable("Address");
            e.HasKey(a => a.AddressId);

            e.HasIndex(a => new { a.UserId, a.IsDefault })
                .HasFilter("[IsDefault] = 1")
                .HasDatabaseName("IX_Address_IsDefault");

            e.HasIndex(a => a.IsSnapshot)
                .HasDatabaseName("IX_Address_IsSnapshot");

            e.HasIndex(a => a.UserId)
                .HasDatabaseName("IX_Address_UserId");

            // Address.UserId → User.UserId with CASCADE DELETE
            // The reverse circular FK (User.DefaultAddressId) uses NoAction
            e.HasOne(a => a.User)
                .WithMany(u => u.Addresses)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private static void ConfigureOTPVerification(ModelBuilder mb)
    {
        mb.Entity<OTPVerification>(e =>
        {
            e.ToTable("OTPVerification");
            e.HasKey(o => o.OTPId);

            e.HasIndex(o => o.Email)
                .HasDatabaseName("IX_OTP_Email");

            e.HasIndex(o => o.ExpiresAt)
                .HasDatabaseName("IX_OTP_ExpiresAt");

            e.HasIndex(o => o.IsUsed)
                .HasDatabaseName("IX_OTP_IsUsed");

            // No FK to User — OTP is created before the user account exists
        });
    }

    private static void ConfigureGuestSession(ModelBuilder mb)
    {
        mb.Entity<GuestSession>(e =>
        {
            e.ToTable("GuestSession");
            e.HasKey(g => g.GuestSessionId);

            e.HasIndex(g => g.SessionToken)
                .IsUnique()
                .HasDatabaseName("UX_GuestSession_Token");

            e.HasIndex(g => g.ExpiresAt)
                .HasDatabaseName("IX_GuestSession_ExpiresAt");

            e.HasIndex(g => g.SessionToken)
                .HasDatabaseName("IX_GuestSession_Token");

            e.HasOne(g => g.ConvertedToUser)
                .WithMany()
                .HasForeignKey(g => g.ConvertedToUserId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);
        });
    }
}
