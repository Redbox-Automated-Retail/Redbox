using System;

namespace Redbox.HAL.Core;

[AttributeUsage(AttributeTargets.Method)]
public class StateHandlerAttribute : Attribute
{
    public object State { get; set; }
}