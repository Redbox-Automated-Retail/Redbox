namespace TaskSchedulerInterop
{
    internal enum TaskTriggerType
    {
        TIME_TRIGGER_ONCE,
        TIME_TRIGGER_DAILY,
        TIME_TRIGGER_WEEKLY,
        TIME_TRIGGER_MONTHLYDATE,
        TIME_TRIGGER_MONTHLYDOW,
        EVENT_TRIGGER_ON_IDLE,
        EVENT_TRIGGER_AT_SYSTEMSTART,
        EVENT_TRIGGER_AT_LOGON,
    }
}
