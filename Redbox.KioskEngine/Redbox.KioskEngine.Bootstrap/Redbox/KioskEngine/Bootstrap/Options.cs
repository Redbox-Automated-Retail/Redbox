using System.Collections.Generic;
using System.ComponentModel;
using Redbox.Core;
using Redbox.GetOpts;

namespace Redbox.KioskEngine.Bootstrap
{
	[Usage("kioskengine [-D:property=value ...] [--bundle[:|=]file]")]
	public class Options
	{
		[Option(LongName = "bundle")]
		[Description("The name of the resource bundle to load.")]
		public string BundleFile;

		[Option(ShortName = "D")]
		[Description("Define properties to inject into runtime environment.")]
		public IDictionary<string, string> Properties = new Dictionary<string, string>();

		public static Options Instance => Singleton<Options>.Instance;

		private Options()
		{
		}
	}
}
