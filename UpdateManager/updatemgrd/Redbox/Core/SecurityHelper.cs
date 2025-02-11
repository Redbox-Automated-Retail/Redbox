using System.Security.Principal;

namespace Redbox.Core
{
    internal static class SecurityHelper
    {
        public static bool IsCurrentUserAnAdministrator()
        {
            WindowsIdentity current = WindowsIdentity.GetCurrent();
            return current != null && new WindowsPrincipal(current).IsInRole("BUILTIN\\Administrators");
        }

        public static bool IsCurrentUserPowerUser()
        {
            WindowsIdentity current = WindowsIdentity.GetCurrent();
            return current != null && new WindowsPrincipal(current).IsInRole("BUILTIN\\Power Users");
        }
    }
}
