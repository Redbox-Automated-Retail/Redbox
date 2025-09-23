using System;

namespace Redbox.KioskEngine.Bootstrap
{
	[AttributeUsage(AttributeTargets.Method)]
	public class EngineCommandAttribute : Attribute
	{
		public string Name { get; set; }
	}
}
