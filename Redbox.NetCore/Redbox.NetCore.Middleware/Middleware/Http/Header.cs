namespace Redbox.NetCore.Middleware.Http
{
    public class Header
    {
        public Header(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; set; }

        public string Value { get; set; }
    }
}