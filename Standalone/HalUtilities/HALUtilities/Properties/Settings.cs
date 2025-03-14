using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace HALUtilities.Properties
{
    [CompilerGenerated]
    [GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "12.0.0.0")]
    internal sealed class Settings : ApplicationSettingsBase
    {
        public static Settings Default { get; } = (Settings)Synchronized(new Settings());

        [ApplicationScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool KillEngine => (bool)this[nameof(KillEngine)];

        [ApplicationScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("10")]
        public int TouchDriverWakeup => (int)this[nameof(TouchDriverWakeup)];

        [ApplicationScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("15000")]
        public int TouchDriverResetTimeout => (int)this[nameof(TouchDriverResetTimeout)];

        [ApplicationScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool DebugHardware => (bool)this[nameof(DebugHardware)];
    }
}