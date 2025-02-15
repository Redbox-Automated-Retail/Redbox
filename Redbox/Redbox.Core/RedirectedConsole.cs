using System;
using System.IO;

namespace Redbox.Core
{
    public class RedirectedConsole : IConsole, IDisposable
    {
        private readonly StreamWriter m_writer;

        public RedirectedConsole(string fileName)
        {
            var directoryName = Path.GetDirectoryName(fileName);
            if (!Directory.Exists(directoryName))
                Directory.CreateDirectory(directoryName);
            m_writer = File.CreateText(fileName);
            m_writer.AutoFlush = true;
        }

        public void Dispose()
        {
            if (m_writer == null)
                return;
            m_writer.Close();
            m_writer.Dispose();
        }

        public bool IsAttachedToConsole => true;

        public void Write(string value)
        {
            m_writer.Write(value);
        }

        public void Write(string format, params object[] parms)
        {
            m_writer.Write(format, parms);
        }

        public void WriteLine()
        {
            m_writer.WriteLine();
        }

        public void WriteLine(object value)
        {
            m_writer.WriteLine(value);
        }

        public void WriteLine(string value)
        {
            m_writer.WriteLine(value);
        }

        public void WriteLine(string format, params object[] parms)
        {
            m_writer.WriteLine(format, parms);
        }
    }
}