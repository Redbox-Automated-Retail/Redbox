using System.Collections;
using System.Collections.Generic;

namespace Redbox.Lua
{
    public class LuaDebugBreakpointContainer : IEnumerable<LuaDebugBreakpoint>, IEnumerable
    {
        private readonly List<LuaDebugBreakpoint> m_items = new List<LuaDebugBreakpoint>();

        internal LuaDebugBreakpointContainer()
        {
        }

        public int Count => m_items.Count;

        public LuaDebugBreakpoint this[int index] => m_items[index];

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_items.GetEnumerator();
        }

        public IEnumerator<LuaDebugBreakpoint> GetEnumerator()
        {
            return m_items.GetEnumerator();
        }

        public void Clear()
        {
            m_items.Clear();
        }

        public void Remove(LuaDebugBreakpoint item)
        {
            m_items.Remove(item);
        }

        internal void Add(LuaDebugBreakpoint item)
        {
            m_items.Add(item);
        }

        internal void RemoveAt(int index)
        {
            m_items.RemoveAt(index);
        }
    }
}