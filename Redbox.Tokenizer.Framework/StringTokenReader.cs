using System;

namespace Redbox.Tokenizer.Framework
{
    public class StringTokenReader : ITokenReader, IDisposable
    {
        private readonly string m_statement;

        public StringTokenReader(int row, string statement)
        {
            m_statement = statement;
            Row = (ushort)row;
            Column = 0;
        }

        public char GetCurrentToken()
        {
            return m_statement[Column];
        }

        public bool MoveToNextToken()
        {
            ++Column;
            return Column < m_statement.Length;
        }

        public void Reset()
        {
            Column = 0;
        }

        public char? PeekNextToken(int i)
        {
            return Column + i >= m_statement.Length ? new char?() : m_statement[Column + i];
        }

        public char? PeekNextToken()
        {
            return PeekNextToken(1);
        }

        public bool IsEmpty()
        {
            return string.IsNullOrEmpty(m_statement);
        }

        public void Dispose()
        {
        }

        public int Column { get; private set; }

        public int Row { get; private set; }

        public string RemainingTokens => m_statement.Substring(Column);

        public void IncrementRowCount()
        {
            ++Row;
        }
    }
}