using System;
using System.Runtime.InteropServices;
using TaskSchedulerInterop;

namespace TaskScheduler
{
    internal abstract class Trigger : ICloneable
    {
        private ITaskTrigger iTaskTrigger;
        internal TaskTrigger taskTrigger;

        internal Trigger()
        {
            this.iTaskTrigger = (ITaskTrigger)null;
            this.taskTrigger = new TaskTrigger();
            this.taskTrigger.TriggerSize = (ushort)Marshal.SizeOf((object)this.taskTrigger);
            this.taskTrigger.BeginYear = (ushort)DateTime.Today.Year;
            this.taskTrigger.BeginMonth = (ushort)DateTime.Today.Month;
            this.taskTrigger.BeginDay = (ushort)DateTime.Today.Day;
        }

        internal Trigger(ITaskTrigger iTrigger)
        {
            if (iTrigger == null)
                throw new ArgumentNullException(nameof(iTrigger), "ITaskTrigger instance cannot be null");
            this.taskTrigger = new TaskTrigger();
            this.taskTrigger.TriggerSize = (ushort)Marshal.SizeOf((object)this.taskTrigger);
            iTrigger.GetTrigger(ref this.taskTrigger);
            this.iTaskTrigger = iTrigger;
        }

        public object Clone()
        {
            Trigger trigger = (Trigger)this.MemberwiseClone();
            trigger.iTaskTrigger = (ITaskTrigger)null;
            return (object)trigger;
        }

        internal bool Bound => this.iTaskTrigger != null;

        public DateTime BeginDate
        {
            get
            {
                return new DateTime((int)this.taskTrigger.BeginYear, (int)this.taskTrigger.BeginMonth, (int)this.taskTrigger.BeginDay);
            }
            set
            {
                this.taskTrigger.BeginYear = (ushort)value.Year;
                this.taskTrigger.BeginMonth = (ushort)value.Month;
                this.taskTrigger.BeginDay = (ushort)value.Day;
                this.SyncTrigger();
            }
        }

        public bool HasEndDate
        {
            get => ((int)this.taskTrigger.Flags & 1) == 1;
            set
            {
                if (value)
                    throw new ArgumentException("HasEndDate can only be set false");
                this.taskTrigger.Flags &= 4294967294U;
                this.SyncTrigger();
            }
        }

        public DateTime EndDate
        {
            get
            {
                return this.taskTrigger.EndYear == (ushort)0 ? DateTime.MinValue : new DateTime((int)this.taskTrigger.EndYear, (int)this.taskTrigger.EndMonth, (int)this.taskTrigger.EndDay);
            }
            set
            {
                this.taskTrigger.Flags |= 1U;
                this.taskTrigger.EndYear = (ushort)value.Year;
                this.taskTrigger.EndMonth = (ushort)value.Month;
                this.taskTrigger.EndDay = (ushort)value.Day;
                this.SyncTrigger();
            }
        }

        public int DurationMinutes
        {
            get => (int)this.taskTrigger.MinutesDuration;
            set
            {
                if ((long)value < (long)this.taskTrigger.MinutesInterval)
                    throw new ArgumentOutOfRangeException(nameof(DurationMinutes), (object)value, "DurationMinutes must be greater than or equal the IntervalMinutes value");
                this.taskTrigger.MinutesDuration = (uint)value;
                this.SyncTrigger();
            }
        }

        public int IntervalMinutes
        {
            get => (int)this.taskTrigger.MinutesInterval;
            set
            {
                if ((long)value > (long)this.taskTrigger.MinutesDuration)
                    throw new ArgumentOutOfRangeException(nameof(IntervalMinutes), (object)value, "IntervalMinutes must be less than or equal the DurationMinutes value");
                this.taskTrigger.MinutesInterval = (uint)value;
                this.SyncTrigger();
            }
        }

        public bool KillAtDurationEnd
        {
            get => ((int)this.taskTrigger.Flags & 2) == 2;
            set
            {
                if (value)
                    this.taskTrigger.Flags |= 2U;
                else
                    this.taskTrigger.Flags &= 4294967293U;
                this.SyncTrigger();
            }
        }

        public bool Disabled
        {
            get => ((int)this.taskTrigger.Flags & 4) == 4;
            set
            {
                if (value)
                    this.taskTrigger.Flags |= 4U;
                else
                    this.taskTrigger.Flags &= 4294967291U;
                this.SyncTrigger();
            }
        }

        internal static Trigger CreateTrigger(ITaskTrigger iTaskTrigger)
        {
            if (iTaskTrigger == null)
                throw new ArgumentNullException(nameof(iTaskTrigger), "Instance of ITaskTrigger cannot be null");
            TaskTrigger Trigger = new TaskTrigger();
            Trigger.TriggerSize = (ushort)Marshal.SizeOf((object)Trigger);
            iTaskTrigger.GetTrigger(ref Trigger);
            switch (Trigger.Type)
            {
                case TaskTriggerType.TIME_TRIGGER_ONCE:
                    return (Trigger)new RunOnceTrigger(iTaskTrigger);
                case TaskTriggerType.TIME_TRIGGER_DAILY:
                    return (Trigger)new DailyTrigger(iTaskTrigger);
                case TaskTriggerType.TIME_TRIGGER_WEEKLY:
                    return (Trigger)new WeeklyTrigger(iTaskTrigger);
                case TaskTriggerType.TIME_TRIGGER_MONTHLYDATE:
                    return (Trigger)new MonthlyTrigger(iTaskTrigger);
                case TaskTriggerType.TIME_TRIGGER_MONTHLYDOW:
                    return (Trigger)new MonthlyDOWTrigger(iTaskTrigger);
                case TaskTriggerType.EVENT_TRIGGER_ON_IDLE:
                    return (Trigger)new OnIdleTrigger(iTaskTrigger);
                case TaskTriggerType.EVENT_TRIGGER_AT_SYSTEMSTART:
                    return (Trigger)new OnSystemStartTrigger(iTaskTrigger);
                case TaskTriggerType.EVENT_TRIGGER_AT_LOGON:
                    return (Trigger)new OnLogonTrigger(iTaskTrigger);
                default:
                    throw new ArgumentException("Unable to recognize type of trigger referenced in iTaskTrigger", nameof(iTaskTrigger));
            }
        }

        protected void SyncTrigger()
        {
            if (this.iTaskTrigger == null)
                return;
            this.iTaskTrigger.SetTrigger(ref this.taskTrigger);
        }

        internal void Bind(ITaskTrigger iTaskTrigger)
        {
            this.iTaskTrigger = this.iTaskTrigger == null ? iTaskTrigger : throw new ArgumentException("Attempt to bind an already bound trigger");
            iTaskTrigger.SetTrigger(ref this.taskTrigger);
        }

        internal void Bind(Trigger trigger) => this.Bind(trigger.iTaskTrigger);

        internal void Unbind()
        {
            if (this.iTaskTrigger == null)
                return;
            Marshal.ReleaseComObject((object)this.iTaskTrigger);
            this.iTaskTrigger = (ITaskTrigger)null;
        }

        public override string ToString()
        {
            if (this.iTaskTrigger == null)
                return "Unbound " + this.GetType().ToString();
            IntPtr TriggerString;
            this.iTaskTrigger.GetTriggerString(out TriggerString);
            return CoTaskMem.LPWStrToString(TriggerString);
        }

        public override bool Equals(object obj)
        {
            return this.taskTrigger.Equals((object)((Trigger)obj).taskTrigger);
        }

        public override int GetHashCode() => this.taskTrigger.GetHashCode();

        [Flags]
        private enum TaskTriggerFlags
        {
            HasEndDate = 1,
            KillAtDurationEnd = 2,
            Disabled = 4,
        }
    }
}
