using System;

namespace Redbox.KioskEngine.ComponentModel
{
  public static class ExceptionExtensions
  {
    public static string GetFullMessage(this Exception e)
    {
      if (e == null)
        return "";
      return e.InnerException != null ? e.Message.Replace("\n", "").Replace("\r", "") + " -> " + e.InnerException.GetFullMessage() : e.Message;
    }
  }
}
