using System.ComponentModel;
using Redbox.HAL.Component.Model;
using Redbox.HAL.Component.Model.Attributes;
using Redbox.HAL.IPC.Framework;
using Redbox.IPC.Framework;

namespace Redbox.HAL.Service.Framework
{
    [Command("stack")]
    public sealed class StackCommand
    {
        [CommandForm(Name = "show")]
        [Usage("STACK show job: job-id")]
        [Description("Dumps a diagnostic list of the stack contents.")]
        public void Show(CommandContext context, [CommandKeyValue(KeyName = "job", IsRequired = true)] string jobId)
        {
            var job = CommandHelper.GetJob(jobId, context.Errors);
            if (job == null)
                return;
            var position = job.StackDepth + job.ResultCount;
            job.ForeachStackItem(o => context.Messages.Add(string.Format("{0}: {1}", position--, o)));
            job.ForeachResult(r => context.Messages.Add(string.Format("{0}: {1}", position--, r)));
        }

        [CommandForm(Name = "clear")]
        [Usage("STACK clear job: job-id")]
        [Description("Clears the execution engine's data stack.")]
        public void Clear(CommandContext context, [CommandKeyValue(KeyName = "job", IsRequired = true)] string jobId)
        {
            CommandHelper.GetJob(jobId, context.Errors)?.ClearStack();
        }

        [CommandForm(Name = "remove-results")]
        [Usage("STACK remove-results job: job-id")]
        [Description("Removes only entries on the stack that were added using the RESULT instruction.")]
        public void RemoveResults(CommandContext context,
            [CommandKeyValue(KeyName = "job", IsRequired = true)] string jobId)
        {
            CommandHelper.GetJob(jobId, context.Errors)?.ClearResults();
        }

        [CommandForm(Name = "pop")]
        [Usage("STACK pop job: job-id value: value [end: bottom|top]")]
        [Description(
            "Pops any value at the specified end of the data stack for the designated job.  Values are popped from the top of the stack by default.")]
        public void Pop(CommandContext context, [CommandKeyValue(KeyName = "job", IsRequired = true)] string jobId,
            StackEnd? end)
        {
            var job = CommandHelper.GetJob(jobId, context.Errors);
            if (job == null)
                return;
            context.Messages.Add(job.Pop((StackEnd)((int?)end ?? 1)).ToString());
        }

        [CommandForm(Name = "push")]
        [Usage("STACK push job: job-id [end: bottom|top]")]
        [Description("Pushes value onto specified end of stack for the designated job.")]
        public void Push(CommandContext context, [CommandKeyValue(KeyName = "job", IsRequired = true)] string jobId,
            [CommandKeyValue(IsRequired = true)] string value, StackEnd? end)
        {
            var job = CommandHelper.GetJob(jobId, context.Errors);
            if (job == null)
                return;
            ServiceLocator.Instance.GetService<IExecutionService>()
                .PushValue(job, value, (StackEnd)((int?)end ?? 1), context.Errors);
        }
    }
}