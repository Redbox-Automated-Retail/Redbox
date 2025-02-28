using System;

namespace TaskScheduler
{
    [Flags]
    internal enum MonthsOfTheYear : short
    {
        January = 1,
        February = 2,
        March = 4,
        April = 8,
        May = 16, // 0x0010
        June = 32, // 0x0020
        July = 64, // 0x0040
        August = 128, // 0x0080
        September = 256, // 0x0100
        October = 512, // 0x0200
        November = 1024, // 0x0400
        December = 2048, // 0x0800
    }
}
