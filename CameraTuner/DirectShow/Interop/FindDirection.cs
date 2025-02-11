using System;
using System.Runtime.InteropServices;

namespace Redbox.DirectShow.Interop
{
    [ComVisible(false)]
    internal static class FindDirection
    {
        public static readonly Guid UpstreamOnly = new Guid(2893646816U, 39139, 4561, 179, 241, 0, 170, 0, 55, 97, 197);

        public static readonly Guid DownstreamOnly =
            new Guid(2893646817U, 39139, 4561, 179, 241, 0, 170, 0, 55, 97, 197);
    }
}