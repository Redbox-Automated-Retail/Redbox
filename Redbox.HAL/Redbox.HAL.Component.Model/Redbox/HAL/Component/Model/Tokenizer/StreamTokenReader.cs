using System;
using System.IO;

namespace Redbox.HAL.Component.Model.Tokenizer
{
    public class StreamTokenReader : ITokenReader, IDisposable
    {
        private readonly int BufferSize;
        private readonly char[] m_buffer;
        private readonly StreamReader m_stream;
        private readonly int MaxLookAhead;
        private int m_bufferSize;
        private int m_index;
        private bool m_resetColumnOnNextMove;
        private bool m_twoCharNewLine;

        public StreamTokenReader(Stream stream, int maxLookAhead)
        {
            MaxLookAhead = maxLookAhead;
            BufferSize = MaxLookAhead + 1;
            m_buffer = new char[BufferSize];
            m_stream = new StreamReader(stream);
            Row = 1;
            Column = 1;
            m_bufferSize = m_stream.ReadBlock(m_buffer, 0, m_buffer.Length);
            m_index = 0;
            m_twoCharNewLine = false;
            m_resetColumnOnNextMove = false;
            AdavnceRowCount();
        }

        public StreamTokenReader(Stream stream)
            : this(stream, 1024)
        {
        }

        public char GetCurrentToken()
        {
            return m_buffer[m_index];
        }

        public char? PeekNextToken()
        {
            return PeekNextToken(1);
        }

        public char? PeekNextToken(int i)
        {
            if (m_index + i < 0)
                return new char?();
            if (i > MaxLookAhead)
                throw new ArgumentException(string.Format("This StreamTokenReader can only look {0} charactes ahead.",
                    MaxLookAhead));
            if (m_index + i < m_bufferSize)
                return m_buffer[m_index + i];
            Array.Copy(m_buffer, m_index, m_buffer, 0, m_bufferSize - m_index);
            m_bufferSize = m_stream.ReadBlock(m_buffer, m_bufferSize - m_index, m_index) + (m_bufferSize - m_index);
            m_index = 0;
            return i >= m_bufferSize ? new char?() : m_buffer[i];
        }

        public bool MoveToNextToken()
        {
            if (m_resetColumnOnNextMove)
            {
                Column = 1;
                m_resetColumnOnNextMove = false;
            }
            else
            {
                ++Column;
            }

            ++m_index;
            if (m_index < m_bufferSize)
            {
                AdavnceRowCount();
                return true;
            }

            m_index = 0;
            AdavnceRowCount();
            m_bufferSize = m_stream.ReadBlock(m_buffer, 0, m_buffer.Length);
            return m_bufferSize > 0;
        }

        public void Reset()
        {
        }

        public bool IsEmpty()
        {
            return m_index >= m_bufferSize && m_stream.EndOfStream;
        }

        public ushort Row { get; private set; }

        public ushort Column { get; private set; }

        public string RemainingTokens => throw new NotImplementedException("Not implementing for streams.");

        public void Dispose()
        {
            m_stream.Dispose();
        }

        private void AdavnceRowCount()
        {
            if (GetCurrentToken() == '\r')
            {
                var nullable = PeekNextToken();
                if (nullable.HasValue)
                {
                    nullable = PeekNextToken();
                    if (nullable.Value == '\n')
                    {
                        m_twoCharNewLine = true;
                        m_resetColumnOnNextMove = false;
                        goto label_5;
                    }
                }

                m_resetColumnOnNextMove = true;
                label_5:
                ++Row;
            }
            else
            {
                if (GetCurrentToken() != '\n')
                    return;
                Column = 1;
                if (m_twoCharNewLine)
                {
                    m_resetColumnOnNextMove = true;
                    m_twoCharNewLine = false;
                }
                else
                {
                    m_resetColumnOnNextMove = true;
                    ++Row;
                }
            }
        }
    }
}