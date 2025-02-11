using System.Runtime.InteropServices;

namespace Redbox.Core
{
    internal static class SimpleSound
    {
        public static bool Play(byte[] buffer, SoundFlags flags)
        {
            return SimpleSound.PlaySound(buffer, flags);
        }

        public static bool Play(string fileName, SoundFlags flags)
        {
            return SimpleSound.PlaySound(fileName, flags);
        }

        public static bool StopPlay() => SimpleSound.PlaySound((string)null, SoundFlags.SND_PURGE);

        [DllImport("WinMM.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        internal static extern bool PlaySound(string wfname, SoundFlags fuSound);

        [DllImport("WinMM.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        internal static extern bool PlaySound(byte[] buffer, SoundFlags fuSound);
    }
}
