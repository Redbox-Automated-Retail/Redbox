namespace Redbox.Lua
{
    public class LuaDebugBreakpoint
    {
        public LuaDebugBreakpoint(LuaDebugFile file, int line)
        {
            File = file;
            Line = line;
            Enabled = true;
        }

        public int Line { get; set; }

        public bool Enabled { get; set; }

        public LuaDebugFile File { get; private set; }
    }
}