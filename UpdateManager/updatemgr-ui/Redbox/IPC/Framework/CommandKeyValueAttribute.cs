using Redbox.Compression;
using System;

namespace Redbox.IPC.Framework
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    internal class CommandKeyValueAttribute : Attribute
    {
        public string KeyName { get; set; }

        public bool IsRequired { get; set; }

        public string DefaultValue { get; set; }

        public BinaryEncoding BinaryEncoding { get; set; }

        public CompressionType CompressionType { get; set; }
    }
}
