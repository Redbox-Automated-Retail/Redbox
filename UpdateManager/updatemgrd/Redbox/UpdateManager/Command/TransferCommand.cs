using Redbox.Core;
using Redbox.IPC.Framework;
using Redbox.UpdateManager.ComponentModel;
using System;
using System.Collections.Generic;

namespace Redbox.UpdateManager.Command
{
    [Redbox.IPC.Framework.Command("transfer")]
    internal class TransferCommand
    {
        [CommandForm(Name = "cancel-all")]
        [Usage("transfer cancel-all")]
        public void CancelAll(CommandContext context)
        {
            Redbox.IPC.Framework.ErrorList collection = TransferCommand.ConvertErrors((List<Redbox.UpdateManager.ComponentModel.Error>)ServiceLocator.Instance.GetService<ITransferService>().CancelAll());
            context.Errors.AddRange((IEnumerable<Redbox.IPC.Framework.Error>)collection);
        }

        [CommandForm(Name = "repositories-in-transit")]
        [Usage("transfer repositories-in-transit")]
        public void InTransit(CommandContext context)
        {
            HashSet<string> inTransit;
            Redbox.IPC.Framework.ErrorList collection = TransferCommand.ConvertErrors((List<Redbox.UpdateManager.ComponentModel.Error>)ServiceLocator.Instance.GetService<ITransferService>().GetRepositoriesInTransit(out inTransit));
            context.Errors.AddRange((IEnumerable<Redbox.IPC.Framework.Error>)collection);
            context.Messages.Add(new List<string>((IEnumerable<string>)inTransit).ToJson());
        }

        [CommandForm(Name = "list")]
        [Usage("transfer list [allusers:]")]
        public void InTransit(CommandContext context, bool? allusers)
        {
            ITransferService service = ServiceLocator.Instance.GetService<ITransferService>();
            List<ITransferJob> jobs;
            context.Errors.AddRange((IEnumerable<Redbox.IPC.Framework.Error>)TransferCommand.ConvertErrors((List<Redbox.UpdateManager.ComponentModel.Error>)service.GetJobs(out jobs, allusers.HasValue && allusers.Value)));
            if (context.Errors.ContainsError())
                return;
            context.Messages.Add(jobs.ToJson());
        }

        [CommandForm(Name = "create-download")]
        [Usage("transfer create-download name:")]
        public void CreateDownload(CommandContext context, [CommandKeyValue(IsRequired = true)] string name)
        {
            ITransferService service = ServiceLocator.Instance.GetService<ITransferService>();
            ITransferJob job;
            context.Errors.AddRange((IEnumerable<Redbox.IPC.Framework.Error>)TransferCommand.ConvertErrors((List<Redbox.UpdateManager.ComponentModel.Error>)service.CreateDownloadJob(name, out job)));
            if (context.Errors.ContainsError())
                return;
            context.Messages.Add(job.ToJson());
        }

        [CommandForm(Name = "create-upload")]
        [Usage("transfer create-upload name:")]
        public void CreateUpload(CommandContext context, [CommandKeyValue(IsRequired = true)] string name)
        {
            ITransferService service = ServiceLocator.Instance.GetService<ITransferService>();
            ITransferJob job;
            context.Errors.AddRange((IEnumerable<Redbox.IPC.Framework.Error>)TransferCommand.ConvertErrors((List<Redbox.UpdateManager.ComponentModel.Error>)service.CreateUploadJob(name, out job)));
            if (context.Errors.ContainsError())
                return;
            context.Messages.Add(job.ToJson());
        }

        [CommandForm(Name = "get-job")]
        [Usage("transfer get-job id:")]
        public void GetJob(CommandContext context, [CommandKeyValue(IsRequired = true)] string id)
        {
            ITransferService service = ServiceLocator.Instance.GetService<ITransferService>();
            ITransferJob job;
            context.Errors.AddRange((IEnumerable<Redbox.IPC.Framework.Error>)TransferCommand.ConvertErrors((List<Redbox.UpdateManager.ComponentModel.Error>)service.GetJob(new Guid(id), out job)));
            if (context.Errors.ContainsError())
                return;
            context.Messages.Add(job.ToJson());
        }

        [CommandForm(Name = "are-jobs-running")]
        [Usage("transfer are-jobs-running:")]
        public void AreJobsRunning(CommandContext context)
        {
            ITransferService service = ServiceLocator.Instance.GetService<ITransferService>();
            bool isRunning;
            context.Errors.AddRange((IEnumerable<Redbox.IPC.Framework.Error>)TransferCommand.ConvertErrors((List<Redbox.UpdateManager.ComponentModel.Error>)service.AreJobsRunning(out isRunning)));
            if (context.Errors.ContainsError())
                return;
            context.Messages.Add(isRunning.ToJson());
        }

        [CommandForm(Name = "add-item")]
        [Usage("add-item id: url: file:")]
        public void AddItem(CommandContext context, [CommandKeyValue(IsRequired = true)] string id, [CommandKeyValue(IsRequired = true)] string url, [CommandKeyValue(IsRequired = true)] string file)
        {
            ITransferService service = ServiceLocator.Instance.GetService<ITransferService>();
            ITransferJob job;
            context.Errors.AddRange((IEnumerable<Redbox.IPC.Framework.Error>)TransferCommand.ConvertErrors((List<Redbox.UpdateManager.ComponentModel.Error>)service.GetJob(new Guid(id), out job)));
            if (context.Errors.ContainsError())
                return;
            context.Errors.AddRange((IEnumerable<Redbox.IPC.Framework.Error>)TransferCommand.ConvertErrors((List<Redbox.UpdateManager.ComponentModel.Error>)job.AddItem(url, file)));
        }

        [CommandForm(Name = "complete")]
        [Usage("complete id:")]
        public void Complete(CommandContext context, [CommandKeyValue(IsRequired = true)] string id)
        {
            ITransferService service = ServiceLocator.Instance.GetService<ITransferService>();
            ITransferJob job;
            context.Errors.AddRange((IEnumerable<Redbox.IPC.Framework.Error>)TransferCommand.ConvertErrors((List<Redbox.UpdateManager.ComponentModel.Error>)service.GetJob(new Guid(id), out job)));
            if (context.Errors.ContainsError())
                return;
            context.Errors.AddRange((IEnumerable<Redbox.IPC.Framework.Error>)TransferCommand.ConvertErrors((List<Redbox.UpdateManager.ComponentModel.Error>)job.Complete()));
        }

        [CommandForm(Name = "resume")]
        [Usage("resume id:")]
        public void Resume(CommandContext context, [CommandKeyValue(IsRequired = true)] string id)
        {
            ITransferService service = ServiceLocator.Instance.GetService<ITransferService>();
            ITransferJob job;
            context.Errors.AddRange((IEnumerable<Redbox.IPC.Framework.Error>)TransferCommand.ConvertErrors((List<Redbox.UpdateManager.ComponentModel.Error>)service.GetJob(new Guid(id), out job)));
            if (context.Errors.ContainsError())
                return;
            context.Errors.AddRange((IEnumerable<Redbox.IPC.Framework.Error>)TransferCommand.ConvertErrors((List<Redbox.UpdateManager.ComponentModel.Error>)job.Resume()));
        }

        [CommandForm(Name = "cancel")]
        [Usage("cancel id:")]
        public void Cancel(CommandContext context, [CommandKeyValue(IsRequired = true)] string id)
        {
            ITransferService service = ServiceLocator.Instance.GetService<ITransferService>();
            ITransferJob job;
            context.Errors.AddRange((IEnumerable<Redbox.IPC.Framework.Error>)TransferCommand.ConvertErrors((List<Redbox.UpdateManager.ComponentModel.Error>)service.GetJob(new Guid(id), out job)));
            if (context.Errors.ContainsError())
                return;
            context.Errors.AddRange((IEnumerable<Redbox.IPC.Framework.Error>)TransferCommand.ConvertErrors((List<Redbox.UpdateManager.ComponentModel.Error>)job.Cancel()));
        }

        [CommandForm(Name = "suspend")]
        [Usage("suspend id:")]
        public void Suspend(CommandContext context, [CommandKeyValue(IsRequired = true)] string id)
        {
            ITransferService service = ServiceLocator.Instance.GetService<ITransferService>();
            ITransferJob job;
            context.Errors.AddRange((IEnumerable<Redbox.IPC.Framework.Error>)TransferCommand.ConvertErrors((List<Redbox.UpdateManager.ComponentModel.Error>)service.GetJob(new Guid(id), out job)));
            if (context.Errors.ContainsError())
                return;
            context.Errors.AddRange((IEnumerable<Redbox.IPC.Framework.Error>)TransferCommand.ConvertErrors((List<Redbox.UpdateManager.ComponentModel.Error>)job.Suspend()));
        }

        [CommandForm(Name = "set-priority")]
        [Usage("set-priority id:")]
        public void SetPriority(CommandContext context, [CommandKeyValue(IsRequired = true)] string id, [CommandKeyValue(IsRequired = true)] TransferJobPriority priority)
        {
            ITransferService service = ServiceLocator.Instance.GetService<ITransferService>();
            ITransferJob job;
            context.Errors.AddRange((IEnumerable<Redbox.IPC.Framework.Error>)TransferCommand.ConvertErrors((List<Redbox.UpdateManager.ComponentModel.Error>)service.GetJob(new Guid(id), out job)));
            if (context.Errors.ContainsError())
                return;
            context.Errors.AddRange((IEnumerable<Redbox.IPC.Framework.Error>)TransferCommand.ConvertErrors((List<Redbox.UpdateManager.ComponentModel.Error>)job.SetPriority(priority)));
        }

        [CommandForm(Name = "take-ownership")]
        [Usage("take-ownership id:")]
        public void TakeOwnership(CommandContext context, [CommandKeyValue(IsRequired = true)] string id)
        {
            ITransferService service = ServiceLocator.Instance.GetService<ITransferService>();
            ITransferJob job;
            context.Errors.AddRange((IEnumerable<Redbox.IPC.Framework.Error>)TransferCommand.ConvertErrors((List<Redbox.UpdateManager.ComponentModel.Error>)service.GetJob(new Guid(id), out job)));
            if (context.Errors.ContainsError())
                return;
            context.Errors.AddRange((IEnumerable<Redbox.IPC.Framework.Error>)TransferCommand.ConvertErrors((List<Redbox.UpdateManager.ComponentModel.Error>)job.TakeOwnership()));
        }

        [CommandForm(Name = "set-no-progress-timeout")]
        [Usage("set-no-progress-timeout id: timeout:")]
        public void SetNoProgressTimeout(CommandContext context, [CommandKeyValue(IsRequired = true)] string id, [CommandKeyValue(IsRequired = true)] uint? timeout)
        {
            ITransferService service = ServiceLocator.Instance.GetService<ITransferService>();
            ITransferJob job;
            context.Errors.AddRange((IEnumerable<Redbox.IPC.Framework.Error>)TransferCommand.ConvertErrors((List<Redbox.UpdateManager.ComponentModel.Error>)service.GetJob(new Guid(id), out job)));
            if (context.Errors.ContainsError())
                return;
            context.Errors.AddRange((IEnumerable<Redbox.IPC.Framework.Error>)TransferCommand.ConvertErrors((List<Redbox.UpdateManager.ComponentModel.Error>)job.SetNoProgressTimeout(timeout.Value)));
        }

        [CommandForm(Name = "set-minimum-retry-delay")]
        [Usage("set-minimum-retry-delay id: seconds:")]
        public void SetMinimumRetryDelay(CommandContext context, [CommandKeyValue(IsRequired = true)] string id, [CommandKeyValue(IsRequired = true)] uint? seconds)
        {
            ITransferService service = ServiceLocator.Instance.GetService<ITransferService>();
            ITransferJob job;
            context.Errors.AddRange((IEnumerable<Redbox.IPC.Framework.Error>)TransferCommand.ConvertErrors((List<Redbox.UpdateManager.ComponentModel.Error>)service.GetJob(new Guid(id), out job)));
            if (context.Errors.ContainsError())
                return;
            context.Errors.AddRange((IEnumerable<Redbox.IPC.Framework.Error>)TransferCommand.ConvertErrors((List<Redbox.UpdateManager.ComponentModel.Error>)job.SetMinimumRetryDelay(seconds.Value)));
        }

        [CommandForm(Name = "get-errors")]
        [Usage("get-errors id:")]
        public void GetErrors(CommandContext context, [CommandKeyValue(IsRequired = true)] string id)
        {
            ITransferService service = ServiceLocator.Instance.GetService<ITransferService>();
            ITransferJob job;
            context.Errors.AddRange((IEnumerable<Redbox.IPC.Framework.Error>)TransferCommand.ConvertErrors((List<Redbox.UpdateManager.ComponentModel.Error>)service.GetJob(new Guid(id), out job)));
            if (context.Errors.ContainsError())
                return;
            Redbox.UpdateManager.ComponentModel.ErrorList errors;
            job.GetErrors(out errors);
            context.Errors.AddRange((IEnumerable<Redbox.IPC.Framework.Error>)TransferCommand.ConvertErrors((List<Redbox.UpdateManager.ComponentModel.Error>)errors));
        }

        [CommandForm(Name = "get-items")]
        [Usage("get-items id:")]
        public void GetItems(CommandContext context, [CommandKeyValue(IsRequired = true)] string id)
        {
            ITransferService service = ServiceLocator.Instance.GetService<ITransferService>();
            ITransferJob job;
            context.Errors.AddRange((IEnumerable<Redbox.IPC.Framework.Error>)TransferCommand.ConvertErrors((List<Redbox.UpdateManager.ComponentModel.Error>)service.GetJob(new Guid(id), out job)));
            if (context.Errors.ContainsError())
                return;
            List<ITransferItem> items1;
            Redbox.UpdateManager.ComponentModel.ErrorList items2 = job.GetItems(out items1);
            context.Errors.AddRange((IEnumerable<Redbox.IPC.Framework.Error>)TransferCommand.ConvertErrors((List<Redbox.UpdateManager.ComponentModel.Error>)items2));
            if (context.Errors.ContainsError())
                return;
            context.Messages.Add(items1.ToJson());
        }

        private static Redbox.IPC.Framework.ErrorList ConvertErrors(List<Redbox.UpdateManager.ComponentModel.Error> errors)
        {
            Redbox.IPC.Framework.ErrorList ipc = new Redbox.IPC.Framework.ErrorList();
            errors.ForEach((Action<Redbox.UpdateManager.ComponentModel.Error>)(e => ipc.Add(e.IsWarning ? Redbox.IPC.Framework.Error.NewWarning(e.Code, e.Description, e.Details) : Redbox.IPC.Framework.Error.NewError(e.Code, e.Description, e.Details))));
            return ipc;
        }
    }
}
