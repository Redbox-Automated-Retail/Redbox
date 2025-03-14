using System;
using System.Runtime.InteropServices;

namespace Redbox.DirectShow.Interop
{
    [ComVisible(false)]
    [StructLayout(LayoutKind.Sequential)]
    internal class AMMediaType : IDisposable
    {
        public Guid MajorType;
        public Guid SubType;
        [MarshalAs(UnmanagedType.Bool)] public bool FixedSizeSamples = true;
        [MarshalAs(UnmanagedType.Bool)] public bool TemporalCompression;
        public int SampleSize = 1;
        public Guid FormatType;
        public IntPtr unkPtr;
        public int FormatSize;
        public IntPtr FormatPtr;

        ~AMMediaType()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (FormatSize != 0 && FormatPtr != IntPtr.Zero)
            {
                Marshal.FreeCoTaskMem(FormatPtr);
                FormatSize = 0;
            }

            if (!(unkPtr != IntPtr.Zero))
                return;
            Marshal.Release(unkPtr);
            unkPtr = IntPtr.Zero;
        }
    }
}