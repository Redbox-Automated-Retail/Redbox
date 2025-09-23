using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using Redbox.Core;
using Redbox.KioskEngine.ComponentModel;

namespace Redbox.KioskEngine.Bootstrap
{
	public static class ScreenshotHelper
	{
		public static void Capture(Form form)
		{
			IMacroService service = ServiceLocator.Instance.GetService<IMacroService>();
			using (Bitmap bitmap = new Bitmap(form.Bounds.Width, form.Bounds.Height))
			{
				using (Graphics graphics = Graphics.FromImage(bitmap))
				{
					graphics.CopyFromScreen(form.Bounds.X, form.Bounds.Y, 0, 0, form.Bounds.Size);
				}
				string text = Path.Combine(service.ExpandProperties("${RunningPath}"), "..\\screenshots");
				if (!Directory.Exists(text))
				{
					Directory.CreateDirectory(text);
				}
				string[] files = Directory.GetFiles(text, "screenshot_*.png");
				int num = 0;
				string[] array = files;
				for (int i = 0; i < array.Length; i++)
				{
					string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(array[i]);
					int num2 = fileNameWithoutExtension.IndexOf("_");
					if (num2 >= 0 && int.TryParse(fileNameWithoutExtension.Substring(num2 + 1), out var result) && result > num)
					{
						num = result;
					}
				}
				string path = $"screenshot_{num + 1}.png";
				bitmap.Save(Path.Combine(text, path), ImageFormat.Png);
			}
		}
	}
}
