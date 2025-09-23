using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Redbox.KioskEngine.Bootstrap.Properties
{
	[CompilerGenerated]
	[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.0.0.0")]
	internal sealed class Settings : ApplicationSettingsBase
	{
		private static Settings defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());

		public static Settings Default => defaultInstance;

		[ApplicationScopedSetting]
		[DebuggerNonUserCode]
		[DefaultSettingValue("C:\\Program Files\\Redbox\\reds\\Kiosk Engine\\data")]
		public string DevDataPath => (string)this["DevDataPath"];

		[ApplicationScopedSetting]
		[DebuggerNonUserCode]
		[DefaultSettingValue("C:\\work\\tfs\\redbox_rental\\Rental Bundles\\Main")]
		public string DevBundlePath => (string)this["DevBundlePath"];
	}
}
