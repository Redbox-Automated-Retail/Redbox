using System;
using System.Collections.Generic;
using Redbox.HAL.Component.Model;

namespace Redbox.IPC.Framework
{
    public class ClientCommandResult
    {
        public readonly List<string> CommandMessages = new List<string>();
        public readonly ErrorList Errors = new ErrorList();

        public bool Success { get; set; }

        public string CommandText { get; internal set; }

        public string StatusMessage { get; internal set; }

        public TimeSpan ExecutionTime { get; internal set; }
    }
}