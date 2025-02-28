using System;

namespace Redbox.HAL.Component.Model
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PollerAttribute : Attribute
    {
        public string Name { get; set; }

        public bool ConfigNotifications { get; set; }
    }
}