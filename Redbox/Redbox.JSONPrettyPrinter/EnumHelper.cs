using System;

namespace Redbox.JSONPrettyPrinter
{
    internal static class EnumHelper
    {
        public static T? TryParse<T>(string value, bool ignoreCase) where T : struct
        {
            if (string.IsNullOrEmpty(value))
                return new T?();
            try
            {
                return (T)Enum.Parse(typeof(T), value, ignoreCase);
            }
            catch (ArgumentException ex)
            {
                return new T?();
            }
        }
    }
}