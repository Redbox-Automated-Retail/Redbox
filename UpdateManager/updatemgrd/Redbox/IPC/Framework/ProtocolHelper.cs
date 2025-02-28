using Redbox.Compression;
using Redbox.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace Redbox.IPC.Framework
{
    internal static class ProtocolHelper
    {
        public const string NotApplicable = "N/A";

        public static byte? ParseByte(string data)
        {
            if (data == "N/A")
                return new byte?();
            byte result;
            return byte.TryParse(data, out result) ? new byte?(result) : new byte?();
        }

        public static int? ParseInteger(string data)
        {
            if (data == "N/A")
                return new int?();
            int result;
            return int.TryParse(data, out result) ? new int?(result) : new int?();
        }

        public static long? ParseLong(string data)
        {
            if (data == "N/A")
                return new long?();
            long result;
            return long.TryParse(data, out result) ? new long?(result) : new long?();
        }

        public static DateTime? ParseDate(string data)
        {
            if (data == "N/A")
                return new DateTime?();
            DateTime result;
            return DateTime.TryParse(data, out result) ? new DateTime?(DateTime.SpecifyKind(result, DateTimeKind.Utc)) : new DateTime?();
        }

        public static TimeSpan? ParseTimeSpan(string data)
        {
            if (data == "N/A")
                return new TimeSpan?();
            TimeSpan result;
            return TimeSpan.TryParse(data, out result) ? new TimeSpan?(result) : new TimeSpan?();
        }

        public static ReadOnlyCollection<string> ParseProperties(string data)
        {
            List<string> stringList = new List<string>();
            StringBuilder stringBuilder = new StringBuilder();
            int num = 0;
            Stack<char> charStack = new Stack<char>();
            foreach (char ch in data)
            {
                if (ch == '|' && num == 0)
                {
                    stringList.Add(stringBuilder.ToString());
                    stringBuilder.Length = 0;
                }
                else
                {
                    if (ch == '<')
                    {
                        charStack.Push('>');
                        ++num;
                        if (num == 1)
                            continue;
                    }
                    if (ch == '[' && (charStack.Count == 0 || charStack.Peek() != '>'))
                    {
                        charStack.Push(']');
                        ++num;
                        if (num == 1)
                            continue;
                    }
                    if (charStack.Count > 0 && (int)ch == (int)charStack.Peek())
                    {
                        --num;
                        if (num == 0)
                            continue;
                    }
                    stringBuilder.Append(ch);
                }
            }
            stringList.Add(stringBuilder.ToString());
            return stringList.AsReadOnly();
        }

        public static void FormatErrors(ErrorList errors, IList<string> messages)
        {
            foreach (Error error in (List<Error>)errors)
                messages.Add(string.Format("|*{0}|{1}*|", (object)error, (object)error.Details));
        }

        public static void FormatErrors(ErrorList errors, CommandContext context)
        {
            ProtocolHelper.FormatErrors(errors, (IList<string>)context.Messages);
        }

        public static void FormatEventMessages(IEnumerable<string> messages, CommandContext context)
        {
            foreach (string message in messages)
                context.Messages.Add(message);
        }

        public static void FormatFile(string path, CommandContext context)
        {
            context.Messages.Add(File.ReadAllBytes(path).ToBase64());
        }

        public static void FormatCompressedFile(string path, CommandContext context)
        {
            ProtocolHelper.FormatGzipCompressedMessage(CompressionAlgorithm.GetAlgorithm(CompressionType.GZip).Compress(Encoding.ASCII.GetBytes(File.ReadAllBytes(path).ToBase64())), context);
        }

        public static void FormatCompressedString(string value, CommandContext context)
        {
            ProtocolHelper.FormatLzmaCompressedMessage(CompressionAlgorithm.GetAlgorithm(CompressionType.LZMA).Compress(Encoding.ASCII.GetBytes(value)), context);
        }

        public static void FormatCompressedBytes(byte[] buffer, CommandContext context)
        {
            ProtocolHelper.FormatLzmaCompressedMessage(CompressionAlgorithm.GetAlgorithm(CompressionType.LZMA).Compress(buffer), context);
        }

        public static byte[] DecompressBase64String(string value)
        {
            return CompressionAlgorithm.GetAlgorithm(CompressionType.LZMA).Decompress(value.Base64ToBytes());
        }

        private static void FormatLzmaCompressedMessage(byte[] buffer, CommandContext context)
        {
            context.Messages.Add(string.Format("LZMA|{0}", (object)buffer.ToBase64()));
        }

        private static void FormatGzipCompressedMessage(byte[] buffer, CommandContext context)
        {
            context.Messages.Add(string.Format("GZIP|{0}", (object)buffer.ToBase64()));
        }
    }
}
