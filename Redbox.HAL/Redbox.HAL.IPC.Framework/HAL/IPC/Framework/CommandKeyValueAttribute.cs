using System;
using Redbox.HAL.Component.Model;

namespace Redbox.HAL.IPC.Framework
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class CommandKeyValueAttribute : Attribute
    {
        public string KeyName { get; set; }

        public bool IsRequired { get; set; }

        public string DefaultValue { get; set; }

        public BinaryEncoding BinaryEncoding { get; set; }

        public CompressionType CompressionType { get; set; }
    }
}