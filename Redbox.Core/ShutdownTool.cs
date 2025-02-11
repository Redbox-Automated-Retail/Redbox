using System;
using System.Runtime.InteropServices;

namespace Redbox.Core
{
    public static class ShutdownTool
    {
        private const int SE_PRIVILEGE_ENABLED = 2;
        private const int TOKEN_QUERY = 8;
        private const int TOKEN_ADJUST_PRIVILEGES = 32;
        private const string SE_SHUTDOWN_NAME = "SeShutdownPrivilege";

        public static void Shutdown(ShutdownFlags flags, ShutdownReason reason)
        {
            var currentProcess = GetCurrentProcess();
            var zero = IntPtr.Zero;
            ref var local = ref zero;
            OpenProcessToken(currentProcess, 40, ref local);
            TokPriv1Luid newst;
            newst.Count = 1;
            newst.Luid = 0L;
            newst.Attr = 2;
            LookupPrivilegeValue(null, "SeShutdownPrivilege", ref newst.Luid);
            AdjustTokenPrivileges(zero, false, ref newst, 0, IntPtr.Zero, IntPtr.Zero);
            ExitWindowsEx(flags, reason);
        }

        [DllImport("user32.dll")]
        private static extern bool ExitWindowsEx(ShutdownFlags uFlags, ShutdownReason dwReason);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool LookupPrivilegeValue(string host, string name, ref long pluid);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool AdjustTokenPrivileges(
            IntPtr htok,
            bool disall,
            ref TokPriv1Luid newst,
            int len,
            IntPtr prev,
            IntPtr relen);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetCurrentProcess();

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool OpenProcessToken(IntPtr h, int acc, ref IntPtr phtok);

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct TokPriv1Luid
        {
            public int Count;
            public long Luid;
            public int Attr;
        }
    }
}