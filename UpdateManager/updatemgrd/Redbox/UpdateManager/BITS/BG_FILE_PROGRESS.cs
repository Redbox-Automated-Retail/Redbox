using System.Runtime.InteropServices;

namespace Redbox.UpdateManager.BITS
{
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    internal struct BG_FILE_PROGRESS
    {
        internal ulong BytesTotal;
        internal ulong BytesTransferred;
        internal int Completed;
    }
}
