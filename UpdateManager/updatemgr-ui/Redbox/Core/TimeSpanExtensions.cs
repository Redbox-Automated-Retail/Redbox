using System;

namespace Redbox.Core
{
    internal static class TimeSpanExtensions
    {
        public static void SpinSleep(this TimeSpan timeSpan)
        {
            using (ExecutionTimer executionTimer = new ExecutionTimer())
            {
                do
                    ;
                while ((double)executionTimer.ElapsedMilliseconds < timeSpan.TotalMilliseconds);
            }
        }
    }
}
