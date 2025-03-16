using System;
using Redbox.Core;

namespace Redbox.KioskEngine.ComponentModel.KioskServices
{
    public sealed class RemoteServiceCallbackEntry : ICallbackEntry
    {
        public RemoteServiceCallback Callback { get; set; }

        public IRemoteServiceResult Result { get; set; }

        public string Message { get; set; }

        public string Name => nameof(RemoteServiceCallbackEntry);

        public void Invoke()
        {
            if (Callback == null)
                return;
            try
            {
                if (!string.IsNullOrEmpty(Message))
                    LogHelper.Instance.Log(Message);
                Callback(Result);
            }
            catch (Exception ex)
            {
                LogHelper.Instance.Log("An unhandled exception was raised in SimpleCallback handler.", ex);
            }
        }
    }
}