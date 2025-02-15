using System;

namespace Redbox.Lua
{
    public class LuaError
    {
        private LuaError()
        {
        }

        public int? LineNumber { get; internal set; }

        public string Message { get; internal set; }

        public string ChunkName { get; internal set; }

        public string RawMessage { get; internal set; }

        public static LuaError FromString(string rawMessage)
        {
            var strArray = rawMessage.Split(new char[1]
            {
                ':'
            }, StringSplitOptions.RemoveEmptyEntries);
            var nullable = new int?();
            var str1 = (string)null;
            var str2 = (string)null;
            if (strArray.Length == 3)
            {
                var num1 = strArray[0].IndexOf('"');
                if (num1 != -1)
                {
                    var num2 = strArray[0].LastIndexOf('"');
                    if (num2 != -1)
                        str2 = strArray[0].Substring(num1 + 1, num2 - num1 - 1);
                }

                int result;
                if (int.TryParse(strArray[1], out result))
                    nullable = result;
                str1 = strArray[2];
            }

            return new LuaError
            {
                Message = str1,
                ChunkName = str2,
                RawMessage = rawMessage,
                LineNumber = nullable
            };
        }
    }
}