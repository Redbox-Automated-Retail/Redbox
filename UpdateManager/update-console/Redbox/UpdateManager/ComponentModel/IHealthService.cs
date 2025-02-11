using System;

namespace Redbox.UpdateManager.ComponentModel
{
    internal interface IHealthService
    {
        ErrorList Initialize();

        ErrorList Start();

        ErrorList Stop();

        ErrorList Add(string key, TimeSpan expireTime, Action rescue);

        ErrorList Remove(string key);

        ErrorList Update(string key);
    }
}
