using System;
using System.Reflection;

namespace Redbox.Core
{
    internal static class AssemblyInfoHelper
    {
        public static string GetProductName(Assembly assembly)
        {
            return ((AssemblyProductAttribute)Attribute.GetCustomAttribute(assembly, typeof(AssemblyProductAttribute)))?.Product;
        }

        public static string GetCopyright(Assembly assembly)
        {
            return ((AssemblyCopyrightAttribute)Attribute.GetCustomAttribute(assembly, typeof(AssemblyCopyrightAttribute)))?.Copyright;
        }

        public static string GetVersion(Assembly assembly) => assembly.GetName().Version.ToString();
    }
}
