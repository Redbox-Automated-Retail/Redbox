namespace Redbox.KioskEngine.ComponentModel
{
  public interface ICallbackEntry
  {
    void Invoke();

    string Name { get; }
  }
}
