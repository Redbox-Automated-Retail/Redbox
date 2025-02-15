using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Redbox.Core
{
    public static class FileHelper
    {
        public static void CreateBackup(string path, string extension)
        {
            if (!File.Exists(path))
                return;
            if (extension != null && !extension.StartsWith("."))
                extension = "." + extension;
            var destFileName = string.Format("{0}.{1}{2}", path, DateTime.Now.ToString("yyyyMMddHHmmss"),
                extension ?? ".bak");
            File.Copy(path, destFileName, true);
        }

        public static string GetShortName(string path)
        {
            var lpszShortPath = new StringBuilder(256);
            var shortPathName = (int)GetShortPathName(path, lpszShortPath, lpszShortPath.Capacity);
            return lpszShortPath.ToString();
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern uint GetShortPathName(
            [MarshalAs(UnmanagedType.LPTStr)] string lpszLongPath,
            [MarshalAs(UnmanagedType.LPTStr)] StringBuilder lpszShortPath,
            int cchBuffer);
    }
}