using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml;
using Redbox.Core;
using Redbox.KioskEngine.ComponentModel;
using Redbox.KioskEngine.ComponentModel.TrackData;
using Redbox.KioskEngine.Environment;
using Redbox.KioskEngine.IDE;

namespace Redbox.KioskEngine.Bootstrap
{
	public class HostForm : Form
	{
		private struct COPYDATASTRUCT
		{
			public IntPtr dwData;

			public int cbData;

			[MarshalAs(UnmanagedType.LPStr)]
			public string lpData;
		}

		private const int WM_USER = 1024;

		private const int WM_COPYDATA = 74;

		private bool m_isCursorHidden;

		private DateTime _keyPressStartAfter = DateTime.MinValue;

		private IClassicTrackDataService _classicTrackDataService;

		private bool windowSizeInitialized;

		private IContainer components;

		private IClassicTrackDataService ClassicTrackDataService
		{
			get
			{
				if (_classicTrackDataService == null)
				{
					_classicTrackDataService = ServiceLocator.Instance.GetService<IClassicTrackDataService>();
				}
				return _classicTrackDataService;
			}
		}

		public HostForm()
		{
			InitializeComponent();
            this.Bounds = Screen.PrimaryScreen.Bounds;
            ServiceLocator.Instance.GetService<IViewService>().UITaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
			IMachineSettingsStore service = ServiceLocator.Instance.GetService<IMachineSettingsStore>();
			if (!Debugger.IsAttached && !service.GetValue("Core", "EnableMouseCursor", defaultValue: true))
			{
				Cursor.Hide();
				m_isCursorHidden = true;
			}
			base.ShowInTaskbar = service.GetValue("Ide", "ShowKioskEngineInTaskBar", defaultValue: false);
			service.GetValue<string>("Store", "Environment", null);
			service.GetValue("Ide", "ShowChrome", defaultValue: false);
			if (Debugger.IsAttached)
			{
				base.FormBorderStyle = FormBorderStyle.Sizable;
				base.ControlBox = true;
				MaximumSize = System.Drawing.Size.Empty;
			}
			EngineApplication.DisableProcessWindowsGhosting();
		}

		protected override void WndProc(ref Message m)
		{
			EnvironmentNotificationType environmentNotificationType = EnvironmentNotificationType.None;
			IEnvironmentNotificationService service = ServiceLocator.Instance.GetService<IEnvironmentNotificationService>();
			if (service != null)
			{
				environmentNotificationType = service.ProcessClassicCardReaderNotification(ref m);
			}
			if (environmentNotificationType == EnvironmentNotificationType.CreditCardReaderInvalid && service != null)
			{
				service.RaiseInvalidClassicCardReader();
			}
			else if (environmentNotificationType == EnvironmentNotificationType.CreditCardReaderValid && service != null)
			{
				service.RaiseValidClassicCardReader();
			}
			else
			{
				switch (m.Msg)
				{
				case 74:
					CommandHelper.ProcessCommandMessage(((COPYDATASTRUCT)m.GetLParam(typeof(COPYDATASTRUCT))).lpData);
					break;
				}
			}
			base.WndProc(ref m);
		}

		private void OnPaint(object sender, PaintEventArgs e)
		{
			IRenderingService service = ServiceLocator.Instance.GetService<IRenderingService>();
			if (service.ActiveScene != null)
			{
				service.ActiveScene.MakeDirty(null);
			}
		}

		private string EncodePassword(string originalPassword)
		{
			MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
			byte[] bytes = Encoding.Default.GetBytes(originalPassword);
			return BitConverter.ToString(mD5CryptoServiceProvider.ComputeHash(bytes));
		}

		public void OnKeyDown(object sender, KeyEventArgs e)
		{
			IResourceBundleService service = ServiceLocator.Instance.GetService<IResourceBundleService>();
			switch (e.KeyCode)
			{
			case Keys.Home:
			{
				ITextToSpeechService service4 = ServiceLocator.Instance.GetService<ITextToSpeechService>();
				if (service4 != null)
				{
					service4.AudioDeviceConnected = true;
					service4.RunSpeechWorkflow($"tts_{ServiceLocator.Instance.GetService<IViewService>()?.PeekViewFrame()?.ViewFrame?.ViewName}");
				}
				break;
			}
			case Keys.End:
			{
				ITextToSpeechService service3 = ServiceLocator.Instance.GetService<ITextToSpeechService>();
				if (service3 != null)
				{
					service3.AudioDeviceConnected = false;
				}
				break;
			}
			case Keys.F1:
				ScreenshotHelper.Capture(this);
				break;
			case Keys.F2:
				if (service != null && service.Filter["environment"] != "Production")
				{
					new ManageViewsForm().ShowDialog();
				}
				break;
			case Keys.F4:
			{
				PreferencesManager preferencesManager = new PreferencesManager();
				if (service == null)
				{
					break;
				}
				XmlNode xml = service.GetXml("config");
				if (xml == null)
				{
					break;
				}
				XmlNodeList xmlNodeList = xml.SelectNodes("row");
				if (xmlNodeList == null)
				{
					break;
				}
				string text = null;
				foreach (XmlNode item in xmlNodeList)
				{
					XmlNodeList xmlNodeList2 = item.SelectNodes("column");
					if (xmlNodeList2 == null)
					{
						continue;
					}
					foreach (XmlNode item2 in xmlNodeList2)
					{
						if (item2.GetAttributeValue<string>("name") == "F4PasswordHash")
						{
							text = item2.GetNodeValue(string.Empty);
							break;
						}
					}
				}
				if (text == null)
				{
					break;
				}
				UnlockForm unlockForm = new UnlockForm();
				if (unlockForm.ShowDialog() == DialogResult.OK)
				{
					SHA256Managed sHA256Managed = new SHA256Managed();
					string obj = Convert.ToBase64String(sHA256Managed.ComputeHash(sHA256Managed.ComputeHash(Encoding.UTF8.GetBytes(unlockForm.Password))));
					sHA256Managed.Clear();
					if (obj == text)
					{
						preferencesManager.ShowDialog();
					}
					else
					{
						System.Windows.Forms.MessageBox.Show("The password provided is invalid.", "Redbox Engine for Distributed Stores", MessageBoxButtons.OK);
					}
				}
				break;
			}
			case Keys.F5:
				if (service != null && service.Filter["environment"] != "Production")
				{
					ServiceLocator.Instance.GetService<IIdeService>()?.Show();
				}
				break;
			case Keys.F6:
				if (service != null && service.Filter["environment"] != "Production")
				{
					new ManageTimersForm().ShowDialog();
				}
				break;
			case Keys.F8:
				ServiceLocator.Instance.GetService<IEnvironmentNotificationService>()?.ToggleTouchScreen();
				break;
			case Keys.F10:
			{
				IRenderingService service5 = ServiceLocator.Instance.GetService<IRenderingService>();
				string name = "ADALine";
				if (service5.ActiveScene.SceneRenderType == RenderType.WPF)
				{
					System.Windows.Controls.UserControl userControl = (service5.ActiveScene.Actors.First?.Value)?.WPFFrameworkElement as System.Windows.Controls.UserControl;
					Grid grid = userControl?.Content as Grid;
					IEnumerator enumerator = grid.Children.GetEnumerator();
					bool flag = false;
					while (enumerator.MoveNext())
					{
						if (enumerator.Current is Line line)
						{
							line.Visibility = ((line.Visibility == Visibility.Visible) ? Visibility.Hidden : Visibility.Visible);
							userControl.InvalidateVisual();
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						Line element = new Line
						{
							X1 = 0.0,
							Y1 = 286.0,
							X2 = 1024.0,
							Y2 = 286.0,
							Stroke = new SolidColorBrush
							{
								Color = Colors.GreenYellow
							},
							StrokeThickness = 3.0,
							Stretch = Stretch.None,
							Name = name,
							Visibility = Visibility.Visible
						};
						grid.Children.Add(element);
					}
				}
				else
				{
					service5.ActiveScene.SuspendDrawing();
					IActor actor = service5.ActiveScene.GetActor(name);
					if (actor != null)
					{
						service5.ActiveScene.Remove(actor);
					}
					else
					{
						IActor actor2 = service5.ActiveScene.CreateActor();
						actor2.Name = name;
						actor2.Y = 286;
						actor2.X = 0;
						actor2.Width = 1024;
						actor2.Height = 3;
						actor2.BackgroundColor = System.Drawing.Color.GreenYellow;
						actor2.Visible = true;
						actor2.HitFlags = HitTestFlags.None;
						actor2.ForegroundColor = System.Drawing.Color.GreenYellow;
						actor2.OptionFlags = RenderOptionFlags.Background;
						service5.ActiveScene.AddLast(actor2);
					}
					service5.ActiveScene.ResumeDrawing();
				}
				break;
			}
			case Keys.F11:
				if (m_isCursorHidden)
				{
					Cursor.Show();
				}
				else
				{
					Cursor.Hide();
				}
				m_isCursorHidden = !m_isCursorHidden;
				break;
			case Keys.F12:
			{
				IViewService service2 = ServiceLocator.Instance.GetService<IViewService>();
				if (service2.Peek() == "product_detail_view")
				{
					service2.Push("title_details_dev_view", false);
					service2.Show();
				}
				break;
			}
			}
		}

		public void OnKeyPress(object sender, KeyPressEventArgs e)
		{
			IResourceBundleService service = ServiceLocator.Instance.GetService<IResourceBundleService>();
			if (e.KeyChar == '\u001b')
			{
				Close();
				e.Handled = true;
				return;
			}
			IClassicTrackDataService classicTrackDataService = ClassicTrackDataService;
			if (classicTrackDataService != null && classicTrackDataService.AllowTrackDataParsing)
			{
				e.Handled = ClassicTrackDataService.ProcessKey(e.KeyChar, delegate
				{
					_keyPressStartAfter = DateTime.Now.AddSeconds(3.0);
				});
			}
			if (e.Handled)
			{
				return;
			}
			if (e.KeyChar == '`')
			{
				ResourceBundleService.Instance.GetManifest(out var manifestInfo);
				string text = ResourceBundleService.Instance.Filter["view_name"];
				if (!(manifestInfo.ProductName != "Rental Application"))
				{
					if (!(manifestInfo.ProductName == "Rental Application"))
					{
						goto IL_00ea;
					}
					switch (text)
					{
					case "start_view":
					case "welcome_view":
					case "maintenance_view":
						break;
					default:
						goto IL_00ea;
					}
				}
				EngineApplication.Instance.SelectBundle();
				goto IL_00ea;
			}
			IClassicTrackDataService classicTrackDataService2 = ClassicTrackDataService;
			if (classicTrackDataService2 != null && classicTrackDataService2.AllowTrackDataParsing && service != null && service.Filter["environment"] != "Production")
			{
				switch (e.KeyChar)
				{
				case '~':
					SendKeys.Send("{%}B4111111111111111{^}JOE/COOL                {^}1608041103263?;4111111111111111=1608041103263?");
					break;
				case '!':
					SendKeys.Send("{%}B24931070601{^}JOE/COOL                {^}1608041103263?;249931070601=1608041103263?");
					break;
				case '@':
					SendKeys.Send("{%}B6010562000017685{^}REDBOX                {^}00010004000060013779?;6010562000017685=00010004000060013779?");
					break;
				case '#':
					SendKeys.Send("{%}B4111111111111111{^}JOE/COOL                {^}1608041103263?;4111111111=1608041103263?");
					break;
				}
			}
			else if (DeviceServiceClientHelperSimulator.Instance.UseSimulatedCardReader)
			{
				e.Handled = DeviceServiceClientHelperSimulator.Instance.ProcessKey(e.KeyChar);
			}
			goto IL_019d;
			IL_00ea:
			e.Handled = true;
			goto IL_019d;
			IL_019d:
			if (DateTime.Now > _keyPressStartAfter)
			{
				EngineApplication.Instance.ActivateKeyPressHandlers(new KeyPressEventArgsEx(e, System.Windows.Forms.Control.ModifierKeys));
			}
		}

		private void OnActivated(object sender, EventArgs e)
		{
			IRenderingService service = ServiceLocator.Instance.GetService<IRenderingService>();
			if (service.ActiveScene != null)
			{
				service.ActiveScene.MakeDirty(null);
				EngineApplication.Instance.ActivateActivatedHandlers();
				Focus();
				BringToFront();
			}
		}

		private void OnMouseClick(object sender, MouseEventArgs e)
		{
			IRenderingService service = ServiceLocator.Instance.GetService<IRenderingService>();
			if (service.ActiveScene != null)
			{
				service.ActiveScene.HitTest(e.X, e.Y);
				EngineApplication.Instance.ActivateMouseClickHandlers(e);
			}
		}

		private void OnMouseDoubleClick(object sender, MouseEventArgs e)
		{
			EngineApplication.Instance.ActivateMouseDoubleClickHandlers(e);
		}

		private void HostForm_Shown(object sender, EventArgs e)
		{
			if (!Debugger.IsAttached || windowSizeInitialized)
			{
				return;
			}
			windowSizeInitialized = true;
			string value = ServiceLocator.Instance.GetService<IMachineSettingsStore>().GetValue<string>("Core", "DebugWindowSize");
			if (value != null)
			{
				string[] array = value.Split(',');
				if (array != null && array.Length == 2 && int.TryParse(array[0], out var result) && int.TryParse(array[1], out var result2) && result != 0 && result2 != 0)
				{
					base.Width = result;
					base.Height = result2;
				}
			}
		}

		private void HostForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (Debugger.IsAttached)
			{
				IMachineSettingsStore service = ServiceLocator.Instance.GetService<IMachineSettingsStore>();
				string value = $"{base.Width},{base.Height}";
				service.SetValue("Core", "DebugWindowSize", value);
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Redbox.KioskEngine.Bootstrap.HostForm));
			base.SuspendLayout();
			base.AutoScaleDimensions = new System.Drawing.SizeF(9f, 17f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(153, 153, 152);
			//base.ClientSize = new System.Drawing.Size(1024, 768);
			base.ControlBox = false;
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
			base.MaximizeBox = false;
			//this.MaximumSize = new System.Drawing.Size(1024, 768);
			base.MinimizeBox = false;
			base.Name = "HostForm";
			base.ShowInTaskbar = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Redbox Kiosk Engine";
			base.Activated += new System.EventHandler(OnActivated);
			base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(HostForm_FormClosing);
			base.Shown += new System.EventHandler(HostForm_Shown);
			base.Paint += new System.Windows.Forms.PaintEventHandler(OnPaint);
			base.KeyDown += new System.Windows.Forms.KeyEventHandler(OnKeyDown);
			base.KeyPress += new System.Windows.Forms.KeyPressEventHandler(OnKeyPress);
			base.MouseClick += new System.Windows.Forms.MouseEventHandler(OnMouseClick);
			base.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(OnMouseDoubleClick);
			base.ResumeLayout(false);
		}
	}
}
