using System;
using Redbox.HAL.Component.Model;

namespace Redbox.HAL.Core.Descriptors;

internal sealed class MicrotouchDescriptor : AbstractTouchscreenDescriptor
{
    internal MicrotouchDescriptor(IUsbDeviceService s, DeviceClass clazz, IDriverDescriptor desc)
        : base(s, clazz, desc)
    {
        Vendor = "0596";
        Product = "0001";
        Friendlyname = "3M";
    }

    protected override bool OnSoftReset()
    {
        return new _3mSoftResetCommand().Reset();
    }

    protected override bool OnHardReset()
    {
        return OnResetDriver(2000);
    }

    protected override string OnReadFirmware()
    {
        return new _3mFirmwareCommand().GetFirmwareRevision();
    }

    protected override DeviceStatus OnGetStatus()
    {
        throw new NotImplementedException();
    }

    protected override bool OnMatchDriver()
    {
        return OnMatchDriver(this, DriverDesc);
    }
}