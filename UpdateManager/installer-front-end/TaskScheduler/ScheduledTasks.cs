using System;
using System.Runtime.InteropServices;
using TaskSchedulerInterop;

namespace TaskScheduler
{
    internal class ScheduledTasks : IDisposable
    {
        private ITaskScheduler its;
        internal static Guid ITaskGuid = Marshal.GenerateGuidForType(typeof(ITask));
        internal static Guid CTaskGuid = Marshal.GenerateGuidForType(typeof(CTask));

        public ScheduledTasks(string computer)
          : this()
        {
            this.its.SetTargetComputer(computer);
        }

        public ScheduledTasks() => this.its = (ITaskScheduler)new CTaskScheduler();

        private string[] GrowStringArray(string[] s, uint n)
        {
            string[] strArray = new string[(long)s.Length + (long)n];
            for (int index = 0; index < s.Length; ++index)
                strArray[index] = s[index];
            return strArray;
        }

        public string[] GetTaskNames()
        {
            string[] s = new string[0];
            int num = 0;
            IEnumWorkItems EnumWorkItems;
            this.its.Enum(out EnumWorkItems);
            uint Fetched;
            IntPtr Names;
            while (EnumWorkItems.Next(10U, out Names, out Fetched) >= 0 && Fetched > 0U)
            {
                s = this.GrowStringArray(s, Fetched);
                while (Fetched > 0U)
                {
                    IntPtr ptr = Marshal.ReadIntPtr(Names, (int)--Fetched * IntPtr.Size);
                    s[num++] = Marshal.PtrToStringUni(ptr);
                    Marshal.FreeCoTaskMem(ptr);
                }
                Marshal.FreeCoTaskMem(Names);
            }
            return s;
        }

        public Task CreateTask(string name)
        {
            Task task = this.OpenTask(name);
            if (task != null)
            {
                task.Close();
                throw new ArgumentException("The task \"" + name + "\" already exists.");
            }
            try
            {
                object obj;
                this.its.NewWorkItem(name, ref ScheduledTasks.CTaskGuid, ref ScheduledTasks.ITaskGuid, out obj);
                return new Task((ITask)obj, name);
            }
            catch
            {
                return (Task)null;
            }
        }

        public bool DeleteTask(string name)
        {
            try
            {
                this.its.Delete(name);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Task OpenTask(string name)
        {
            try
            {
                object obj;
                this.its.Activate(name, ref ScheduledTasks.ITaskGuid, out obj);
                return new Task((ITask)obj, name);
            }
            catch
            {
                return (Task)null;
            }
        }

        public void Dispose()
        {
            Marshal.ReleaseComObject((object)this.its);
            this.its = (ITaskScheduler)null;
        }
    }
}
