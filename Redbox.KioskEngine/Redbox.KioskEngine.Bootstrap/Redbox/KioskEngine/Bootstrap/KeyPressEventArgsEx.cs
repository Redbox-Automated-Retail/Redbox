using System.Windows.Forms;

namespace Redbox.KioskEngine.Bootstrap
{
	internal class KeyPressEventArgsEx
	{
		public Keys ModifierKeys { get; private set; }

		public char KeyChar { get; private set; }

		internal KeyPressEventArgsEx(KeyPressEventArgs args, Keys modifierKeys)
		{
			ModifierKeys = modifierKeys;
			KeyChar = args.KeyChar;
		}
	}
}
