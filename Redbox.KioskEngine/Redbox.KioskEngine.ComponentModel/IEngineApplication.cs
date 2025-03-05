using System.Windows.Forms;

namespace Redbox.KioskEngine.ComponentModel
{
  public interface IEngineApplication
  {
    void ThreadSafeHostUpdate(MethodInvoker invoker);

    bool CanRestart();

    bool CanShutDown();

    void FlagForSafeShutdown();

    void ShutDown();

    bool AppStarting { get; set; }

    string DataPath { get; }

    string RunningPath { get; }
  }
}
