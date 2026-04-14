using AdminSystem_v2.Models;

namespace AdminSystem_v2.Helpers
{
    /// <summary>
    /// Centralised role-hierarchy checks.
    ///   Admin  ▸ full access (staff management, products, vouchers, etc.)
    ///   Manager▸ products, vouchers, orders, POS, reports — NOT staff management
    ///   Staff  ▸ POS, view orders/products/reports — NO create/edit/delete on products or vouchers
    /// </summary>
    public static class RoleGuard
    {
        public static bool IsAdmin(string role)
            => role == RoleNames.Admin;

        public static bool IsAdminOrManager(string role)
            => role == RoleNames.Admin || role == RoleNames.Manager;

        public static void RequireAdmin(string callerRole)
        {
            if (!IsAdmin(callerRole))
                throw new UnauthorizedAccessException("Only admins can perform this operation.");
        }

        public static void RequireAdminOrManager(string callerRole)
        {
            if (!IsAdminOrManager(callerRole))
                throw new UnauthorizedAccessException("Only admins and managers can perform this operation.");
        }
    }
}
