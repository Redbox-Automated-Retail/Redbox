using System;
using System.Threading;

namespace Redbox.HAL.Component.Model.Threading;

public struct WithUpgradeableReadLock : IDisposable
{
    private readonly ReaderWriterLockSlim TheLock;
    private bool Disposed;

    public void Dispose()
    {
        if (Disposed)
            return;
        Disposed = true;
        TheLock.ExitUpgradeableReadLock();
    }

    public WithUpgradeableReadLock(ReaderWriterLockSlim _lock)
        : this()
    {
        Disposed = false;
        TheLock = _lock;
        TheLock.EnterUpgradeableReadLock();
    }
}