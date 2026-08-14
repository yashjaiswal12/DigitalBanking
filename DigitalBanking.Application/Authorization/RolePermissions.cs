using DigitalBanking.Domain.Enums.Security;

namespace DigitalBanking.Application.Authorization
{
    public static class RolePermissions
    {
        public static IReadOnlyCollection<string> GetPermissions(Roles role)
        {
            return role switch
            {
                Roles.Admin => [
                    Permissions.ViewCustomers,
                    Permissions.ManageCustomers,
                    Permissions.ViewAccounts,
                    Permissions.ManageAccounts,
                    Permissions.FreezeAccounts,
                    Permissions.ViewAuditLogs
                ],

                Roles.SupportAgent => [
                    Permissions.ViewCustomers,
                    Permissions.ViewAccounts
                ],

                Roles.Auditor => [
                    Permissions.ViewCustomers,
                    Permissions.ViewAccounts,
                    Permissions.ViewAuditLogs
                ],

                Roles.Customer => [
                    Permissions.ViewAccounts
                ],

                _ => Array.Empty<string>()
            };
        }
    }
}
