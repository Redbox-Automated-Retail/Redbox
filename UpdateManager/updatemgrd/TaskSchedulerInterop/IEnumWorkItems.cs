using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace TaskSchedulerInterop
{
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("148BD528-A2AB-11CE-B11F-00AA00530503")]
    internal interface IEnumWorkItems
    {
        [MethodImpl(MethodImplOptions.PreserveSig)]
        int Next([In] uint RequestCount, out IntPtr Names, out uint Fetched);

        void Skip([In] uint Count);

        void Reset();

        void Clone([MarshalAs(UnmanagedType.Interface)] out IEnumWorkItems EnumWorkItems);
    }
}
