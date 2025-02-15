using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Redbox.HAL.MSHALTester.Properties;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
internal class Resources
{
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
        get
        {
            if (resourceMan == null)
                resourceMan = new ResourceManager("Redbox.HAL.MSHALTester.Properties.Resources",
                    typeof(Resources).Assembly);
            return resourceMan;
        }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
        get => resourceCulture;
        set => resourceCulture = value;
    }

    internal static string DestDeckSlotInvalid =>
        ResourceManager.GetString(nameof(DestDeckSlotInvalid), resourceCulture);

    internal static Bitmap sortAscending2 => (Bitmap)ResourceManager.GetObject(nameof(sortAscending2), resourceCulture);

    internal static Bitmap sortDescending2 =>
        (Bitmap)ResourceManager.GetObject(nameof(sortDescending2), resourceCulture);

    internal static string SourceDeckSlotInvalid =>
        ResourceManager.GetString(nameof(SourceDeckSlotInvalid), resourceCulture);

    internal static Bitmap undo => (Bitmap)ResourceManager.GetObject(nameof(undo), resourceCulture);
}