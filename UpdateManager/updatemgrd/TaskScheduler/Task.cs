using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security;
using TaskSchedulerInterop;

namespace TaskScheduler
{
    internal class Task : IDisposable
    {
        private ITask iTask;
        private string name;
        private TriggerList triggers;

        internal Task(ITask iTask, string taskName)
        {
            this.iTask = iTask;
            this.name = !taskName.EndsWith(".job") ? taskName : taskName.Substring(0, taskName.Length - 4);
            this.triggers = (TriggerList)null;
            this.Hidden = this.GetHiddenFileAttr();
        }

        public string Name => this.name;

        public TriggerList Triggers
        {
            get
            {
                if (this.triggers == null)
                    this.triggers = new TriggerList(this.iTask);
                return this.triggers;
            }
        }

        public string ApplicationName
        {
            get
            {
                IntPtr ApplicationName;
                this.iTask.GetApplicationName(out ApplicationName);
                return CoTaskMem.LPWStrToString(ApplicationName);
            }
            set => this.iTask.SetApplicationName(value);
        }

        public string AccountName
        {
            get
            {
                IntPtr AccountName = IntPtr.Zero;
                this.iTask.GetAccountInformation(out AccountName);
                return CoTaskMem.LPWStrToString(AccountName);
            }
        }

        public string Comment
        {
            get
            {
                IntPtr Comment;
                this.iTask.GetComment(out Comment);
                return CoTaskMem.LPWStrToString(Comment);
            }
            set => this.iTask.SetComment(value);
        }

        public string Creator
        {
            get
            {
                IntPtr Creator;
                this.iTask.GetCreator(out Creator);
                return CoTaskMem.LPWStrToString(Creator);
            }
            set => this.iTask.SetCreator(value);
        }

        private short ErrorRetryCount
        {
            get
            {
                ushort RetryCount;
                this.iTask.GetErrorRetryCount(out RetryCount);
                return (short)RetryCount;
            }
            set => this.iTask.SetErrorRetryCount((ushort)value);
        }

        private short ErrorRetryInterval
        {
            get
            {
                ushort RetryInterval;
                this.iTask.GetErrorRetryInterval(out RetryInterval);
                return (short)RetryInterval;
            }
            set => this.iTask.SetErrorRetryInterval((ushort)value);
        }

        public int ExitCode
        {
            get
            {
                uint ExitCode = 0;
                this.iTask.GetExitCode(out ExitCode);
                return (int)ExitCode;
            }
        }

        public TaskFlags Flags
        {
            get
            {
                uint Flags;
                this.iTask.GetFlags(out Flags);
                return (TaskFlags)Flags;
            }
            set => this.iTask.SetFlags((uint)value);
        }

        public short IdleWaitMinutes
        {
            get
            {
                ushort IdleMinutes;
                this.iTask.GetIdleWait(out IdleMinutes, out ushort _);
                return (short)IdleMinutes;
            }
            set
            {
                ushort waitDeadlineMinutes = (ushort)this.IdleWaitDeadlineMinutes;
                this.iTask.SetIdleWait((ushort)value, waitDeadlineMinutes);
            }
        }

        public short IdleWaitDeadlineMinutes
        {
            get
            {
                ushort DeadlineMinutes;
                this.iTask.GetIdleWait(out ushort _, out DeadlineMinutes);
                return (short)DeadlineMinutes;
            }
            set => this.iTask.SetIdleWait((ushort)this.IdleWaitMinutes, (ushort)value);
        }

        public TimeSpan MaxRunTime
        {
            get
            {
                uint MaxRunTimeMS;
                this.iTask.GetMaxRunTime(out MaxRunTimeMS);
                return new TimeSpan((long)MaxRunTimeMS * 10000L);
            }
            set
            {
                double totalMilliseconds = value.TotalMilliseconds;
                if (totalMilliseconds >= (double)uint.MaxValue)
                    this.iTask.SetMaxRunTime(uint.MaxValue);
                else
                    this.iTask.SetMaxRunTime((uint)totalMilliseconds);
            }
        }

        public bool MaxRunTimeLimited
        {
            get
            {
                uint MaxRunTimeMS;
                this.iTask.GetMaxRunTime(out MaxRunTimeMS);
                return MaxRunTimeMS == uint.MaxValue;
            }
            set
            {
                if (value)
                {
                    uint MaxRunTimeMS;
                    this.iTask.GetMaxRunTime(out MaxRunTimeMS);
                    if (MaxRunTimeMS != uint.MaxValue)
                        return;
                    this.iTask.SetMaxRunTime(25920000U);
                }
                else
                    this.iTask.SetMaxRunTime(uint.MaxValue);
            }
        }

        public DateTime MostRecentRunTime
        {
            get
            {
                SystemTime LastRun = new SystemTime();
                this.iTask.GetMostRecentRunTime(ref LastRun);
                return LastRun.Year == (ushort)0 ? DateTime.MinValue : new DateTime((int)LastRun.Year, (int)LastRun.Month, (int)LastRun.Day, (int)LastRun.Hour, (int)LastRun.Minute, (int)LastRun.Second, (int)LastRun.Milliseconds);
            }
        }

        public DateTime NextRunTime
        {
            get
            {
                SystemTime NextRun = new SystemTime();
                this.iTask.GetNextRunTime(ref NextRun);
                return NextRun.Year == (ushort)0 ? DateTime.MinValue : new DateTime((int)NextRun.Year, (int)NextRun.Month, (int)NextRun.Day, (int)NextRun.Hour, (int)NextRun.Minute, (int)NextRun.Second, (int)NextRun.Milliseconds);
            }
        }

        public string Parameters
        {
            get
            {
                IntPtr Parameters;
                this.iTask.GetParameters(out Parameters);
                return CoTaskMem.LPWStrToString(Parameters);
            }
            set => this.iTask.SetParameters(value);
        }

        public ProcessPriorityClass Priority
        {
            get
            {
                uint Priority;
                this.iTask.GetPriority(out Priority);
                return (ProcessPriorityClass)Priority;
            }
            set
            {
                if (value == ProcessPriorityClass.AboveNormal || value == ProcessPriorityClass.BelowNormal)
                    throw new ArgumentException("Unsupported Priority Level");
                this.iTask.SetPriority((uint)value);
            }
        }

        public TaskStatus Status
        {
            get
            {
                int Status;
                this.iTask.GetStatus(out Status);
                return (TaskStatus)Status;
            }
        }

        private int FlagsEx
        {
            get
            {
                uint Flags;
                this.iTask.GetTaskFlags(out Flags);
                return (int)Flags;
            }
            set => this.iTask.SetTaskFlags((uint)value);
        }

        public string WorkingDirectory
        {
            get
            {
                IntPtr WorkingDirectory;
                this.iTask.GetWorkingDirectory(out WorkingDirectory);
                return CoTaskMem.LPWStrToString(WorkingDirectory);
            }
            set => this.iTask.SetWorkingDirectory(value);
        }

        public bool Hidden
        {
            get => (this.Flags & TaskFlags.Hidden) != (TaskFlags)0;
            set
            {
                if (value)
                    this.Flags |= TaskFlags.Hidden;
                else
                    this.Flags &= ~TaskFlags.Hidden;
            }
        }

        public object Tag
        {
            get
            {
                ushort DataLen;
                IntPtr Data;
                this.iTask.GetWorkItemData(out DataLen, out Data);
                byte[] numArray = new byte[(int)DataLen];
                Marshal.Copy(Data, numArray, 0, (int)DataLen);
                return new BinaryFormatter().Deserialize((Stream)new MemoryStream(numArray, false));
            }
            set
            {
                if (!value.GetType().IsSerializable)
                    throw new ArgumentException("Objects set as Data for Tasks must be serializable", nameof(value));
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                MemoryStream serializationStream = new MemoryStream();
                binaryFormatter.Serialize((Stream)serializationStream, value);
                this.iTask.SetWorkItemData((ushort)serializationStream.Length, serializationStream.GetBuffer());
            }
        }

        private void SetHiddenFileAttr(bool set)
        {
            string ppszFileName;
            ((IPersistFile)this.iTask).GetCurFile(out ppszFileName);
            FileAttributes attributes = File.GetAttributes(ppszFileName);
            FileAttributes fileAttributes = !set ? attributes & ~FileAttributes.Hidden : attributes | FileAttributes.Hidden;
            File.SetAttributes(ppszFileName, fileAttributes);
        }

        private bool GetHiddenFileAttr()
        {
            string ppszFileName;
            ((IPersistFile)this.iTask).GetCurFile(out ppszFileName);
            try
            {
                return (File.GetAttributes(ppszFileName) & FileAttributes.Hidden) != (FileAttributes)0;
            }
            catch
            {
                return false;
            }
        }

        public DateTime NextRunTimeAfter(DateTime after)
        {
            after = after.AddSeconds(1.0);
            SystemTime Begin = new SystemTime();
            Begin.Year = (ushort)after.Year;
            Begin.Month = (ushort)after.Month;
            Begin.Day = (ushort)after.Day;
            Begin.DayOfWeek = (ushort)after.DayOfWeek;
            Begin.Hour = (ushort)after.Hour;
            Begin.Minute = (ushort)after.Minute;
            Begin.Second = (ushort)after.Second;
            SystemTime systemTime1 = new SystemTime();
            SystemTime End = Begin;
            End.Year = (ushort)DateTime.MaxValue.Year;
            End.Month = 1;
            ushort Count = 1;
            IntPtr TaskTimes;
            this.iTask.GetRunTimes(ref Begin, ref End, ref Count, out TaskTimes);
            if (Count != (ushort)1)
                return DateTime.MinValue;
            SystemTime systemTime2 = new SystemTime();
            SystemTime structure = (SystemTime)Marshal.PtrToStructure(TaskTimes, typeof(SystemTime));
            Marshal.FreeCoTaskMem(TaskTimes);
            return new DateTime((int)structure.Year, (int)structure.Month, (int)structure.Day, (int)structure.Hour, (int)structure.Minute, (int)structure.Second);
        }

        public void Run() => this.iTask.Run();

        public void Save()
        {
            ((IPersistFile)this.iTask).Save((string)null, false);
            this.SetHiddenFileAttr(this.Hidden);
        }

        public void Save(string name)
        {
            IPersistFile iTask = (IPersistFile)this.iTask;
            string ppszFileName;
            iTask.GetCurFile(out ppszFileName);
            string pszFileName = Path.GetDirectoryName(ppszFileName) + (object)Path.DirectorySeparatorChar + name + Path.GetExtension(ppszFileName);
            iTask.Save(pszFileName, true);
            iTask.SaveCompleted(pszFileName);
            this.name = name;
            this.SetHiddenFileAttr(this.Hidden);
        }

        public void Close()
        {
            if (this.triggers != null)
                this.triggers.Dispose();
            Marshal.ReleaseComObject((object)this.iTask);
            this.iTask = (ITask)null;
        }

        public void DisplayForEdit() => this.iTask.EditWorkItem(0U, 0U);

        public bool DisplayPropertySheet()
        {
            return this.DisplayPropertySheet(Task.PropPages.Task | Task.PropPages.Schedule | Task.PropPages.Settings);
        }

        public bool DisplayPropertySheet(Task.PropPages pages)
        {
            PropSheetHeader psh = new PropSheetHeader();
            IProvideTaskPage iTask = (IProvideTaskPage)this.iTask;
            IntPtr[] numArray = new IntPtr[3];
            int num1 = 0;
            IntPtr phPage;
            if ((pages & Task.PropPages.Task) != (Task.PropPages)0)
            {
                iTask.GetPage(0, false, out phPage);
                numArray[num1++] = phPage;
            }
            if ((pages & Task.PropPages.Schedule) != (Task.PropPages)0)
            {
                iTask.GetPage(1, false, out phPage);
                numArray[num1++] = phPage;
            }
            if ((pages & Task.PropPages.Settings) != (Task.PropPages)0)
            {
                iTask.GetPage(2, false, out phPage);
                numArray[num1++] = phPage;
            }
            if (num1 == 0)
                throw new ArgumentException("No Property Pages to display");
            psh.dwSize = (uint)Marshal.SizeOf((object)psh);
            psh.dwFlags = 128U;
            psh.pszCaption = this.Name;
            psh.nPages = (uint)num1;
            GCHandle gcHandle = GCHandle.Alloc((object)numArray, GCHandleType.Pinned);
            psh.phpage = gcHandle.AddrOfPinnedObject();
            int num2 = PropertySheetDisplay.PropertySheet(ref psh);
            gcHandle.Free();
            if (num2 < 0)
                throw new Exception("Property Sheet failed to display");
            return num2 > 0;
        }

        public void SetAccountInformation(string accountName, string password)
        {
            IntPtr coTaskMemUni = Marshal.StringToCoTaskMemUni(password);
            this.iTask.SetAccountInformation(accountName, coTaskMemUni);
            Marshal.FreeCoTaskMem(coTaskMemUni);
        }

        public void SetAccountInformation(string accountName, SecureString password)
        {
            IntPtr coTaskMemUnicode = Marshal.SecureStringToCoTaskMemUnicode(password);
            this.iTask.SetAccountInformation(accountName, coTaskMemUnicode);
            Marshal.ZeroFreeCoTaskMemUnicode(coTaskMemUnicode);
        }

        public void Terminate() => this.iTask.Terminate();

        public override string ToString()
        {
            return string.Format("{0} (\"{1}\" {2})", (object)this.name, (object)this.ApplicationName, (object)this.Parameters);
        }

        public void Dispose() => this.Close();

        [System.Flags]
        public enum PropPages
        {
            Task = 1,
            Schedule = 2,
            Settings = 4,
        }
    }
}
