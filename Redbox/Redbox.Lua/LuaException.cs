using System;
using System.Runtime.Serialization;

namespace Redbox.Lua
{
    [Serializable]
    public class LuaException : Exception
    {
        public LuaException()
        {
        }

        public LuaException(string message)
            : base(message)
        {
        }

        public LuaException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected LuaException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}