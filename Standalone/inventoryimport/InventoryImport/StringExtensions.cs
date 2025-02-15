namespace InventoryImport
{
    public static class StringExtensions
    {
        public static string ToHalBarcode(this string s, int disktype)
        {
            if (s.Trim().Length == 0 && disktype == 2)
                return "EMPTY";
            if (s.Trim().Length == 0 && disktype == 3)
                return "UNKNOWN";
            return s.Trim().Length > 0 ? s.Trim() : string.Empty;
        }
    }
}