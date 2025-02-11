using System;
using System.Collections.Generic;

namespace Redbox.HAL.Script.Framework
{
    internal sealed class Deque<T>
    {
        private readonly Wintellect.PowerCollections.Deque<T> m_impl = new Wintellect.PowerCollections.Deque<T>();

        public int Count => m_impl.Count;

        public T PopTop()
        {
            return m_impl.Count != 0 ? m_impl.RemoveFromFront() : default;
        }

        public T PopBottom()
        {
            return m_impl.Count != 0 ? m_impl.RemoveFromBack() : default;
        }

        public void PushTop(T instance)
        {
            m_impl.AddToFront(instance);
        }

        public void PushBottom(T instance)
        {
            m_impl.AddToBack(instance);
        }

        public void AddManyToFront(IEnumerable<T> toAdd)
        {
            m_impl.AddManyToFront(toAdd);
        }

        public void AddToBack(T item)
        {
            m_impl.AddToBack(item);
        }

        public void Clear()
        {
            m_impl.Clear();
        }

        public ICollection<T> RemoveAll(Predicate<T> predicate)
        {
            return m_impl.RemoveAll(predicate);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return m_impl.GetEnumerator();
        }
    }
}