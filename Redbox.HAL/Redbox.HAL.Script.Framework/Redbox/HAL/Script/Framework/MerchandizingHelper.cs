using Redbox.HAL.Component.Model;
using Redbox.HAL.Controller.Framework;
using Redbox.HAL.Core;

namespace Redbox.HAL.Script.Framework
{
    internal class MerchandizingHelper
    {
        private MerchandizingHelper()
        {
        }

        internal static MerchandizingHelper Instance => Singleton<MerchandizingHelper>.Instance;

        internal bool Setup(ExecutionResult result, bool checkSlots, ExecutionContext context)
        {
            if (ControllerConfiguration.Instance.IsVMZMachine)
                return false;
            if (checkSlots)
            {
                var numberOfEmpty = 5;
                if (!ServiceLocator.Instance.GetService<IInventoryService>().EmptyCountExceeds(numberOfEmpty))
                {
                    context.CreateInfoResult("QlmUnloadMachineFull", "The machine is full, can't continue.");
                    return false;
                }
            }

            var service1 = ServiceLocator.Instance.GetService<IDecksService>();
            var service2 = ServiceLocator.Instance.GetService<IControlSystem>();
            var service3 = ServiceLocator.Instance.GetService<IMotionControlService>();
            var qlmDeck = service1.QlmDeck;
            if (qlmDeck == null)
            {
                LogHelper.Instance.WithContext("There is no QLM deck on this machine.");
                context.CreateInfoResult("QlmNoDeck", "There is no QLM deck defined for this machine.");
                return false;
            }

            if (qlmDeck.NumberOfSlots == -1)
            {
                LogHelper.Instance.WithContext("There is no QLM max slots defined for this machine.");
                context.CreateInfoResult("QlmNoMaxSlots", "There is no QLM max slots defined for this machine.");
                return false;
            }

            var qlmStatus = service2.GetQlmStatus();
            if (QlmStatus.AuxNotResponsive == qlmStatus)
            {
                context.CreateInfoResult("MachineError", "The QLM status query did not get an AUX response.");
                AddError(result, "The AUX board is not responsive.");
                return false;
            }

            if (QlmStatus.Empty == qlmStatus)
            {
                context.AppLog.Write("There is no QLM in the bay.");
                context.CreateInfoResult("QlmDeckEmpty",
                    "The QLM unload request failed because there is no QLM case in the QLM bay.");
                return false;
            }

            if (service3.InitAxes() != ErrorCodes.Success)
            {
                context.CreateInfoResult("MachineError", "There was an error homing the motors.");
                AddError(result, "The homing operation failed.");
                return false;
            }

            if (QlmStatus.Engaged == qlmStatus)
            {
                context.AppLog.Write("The QLM is already engaged.");
                context.CreateInfoResult("QlmDeckEngaged", "The QLM deck is already engaged.");
            }
            else
            {
                context.AppLog.Write("Engage the QLM.");
                if (service2.EngageQlm(false, context.AppLog) != ErrorCodes.Success)
                {
                    context.CreateInfoResult("QlmDeckEngageFailure", "The QLM engage failed.");
                    if (service2.DisengageQlm(false, context.AppLog) != ErrorCodes.Success)
                        context.CreateInfoResult("QlmDeckDisengageFailure", "The QLM disengage failed.");
                    AddError(result, "Engaging the QLM case timed out.");
                    return false;
                }

                ClearQlmInventory();
                context.CreateInfoResult("QlmDeckEngageSuccess", "The QLM engaged successfully.");
                service3.TestAndReset();
            }

            context.PushTop(qlmDeck.NumberOfSlots);
            return true;
        }

        internal bool CleanupJob(ExecutionResult result, ExecutionContext context, ErrorCodes failCode)
        {
            if (failCode != ErrorCodes.Success)
                context.CreateInfoResult("QlmMoveError",
                    string.Format("The MOVE instruction failed with code {0}.", failCode.ToString().ToUpper()));
            var service1 = ServiceLocator.Instance.GetService<IMotionControlService>();
            if (service1.InitAxes() != ErrorCodes.Success)
            {
                context.CreateInfoResult("MachineError", "There was an error initializing the axes.");
                context.CreateInfoResult("QlmDeckStillEngaged", "The QLM deck is still engaged.");
                result.Errors.Add(Error.NewError("E999", "Execution context error.", "Init axes failed."));
                return false;
            }

            var service2 = ServiceLocator.Instance.GetService<IControlSystem>();
            if (!ControllerConfiguration.Instance.IsVMZMachine)
            {
                if (service2.DisengageQlm(false, context.AppLog) != ErrorCodes.Success)
                {
                    context.CreateInfoResult("QlmDeckDisengageFailure", "The QLM disengage failed.");
                    result.Errors.Add(Error.NewError("E999", "Execution context error.",
                        "The QLM Case didn't disengage."));
                    return false;
                }

                ClearQlmInventory();
                context.CreateInfoResult("QlmDeckDisengageSuccess", "The QLM disengaged successfully.");
            }

            var flag = true;
            if (!service1.TestAndReset(false))
            {
                context.AppLog.WriteFormatted(
                    "The job completed; however, communication with the motion controller isn't working.",
                    LogEntryType.Error);
                result.Errors.Add(Error.NewError("E231", "Motion control error.",
                    "The motion controller is not responding."));
                flag = false;
            }
            else if (service1.MoveVend(MoveMode.Get, context.AppLog) != ErrorCodes.Success)
            {
                context.CreateInfoResult("MachineError", "There was an error executing the MOVEVEND instruction.");
                result.Errors.Add(
                    Error.NewError("E999", "Execution context error.", "The MOVEVEND instruction failed."));
                flag = false;
            }

            return flag;
        }

        internal void AddItemStuckError(ExecutionResult result)
        {
            result.Errors.Add(Error.NewError("E999", "Item stuck in gripper.", "Unable to file disc."));
        }

        internal void ExitOnMoveError(ExecutionResult result, ExecutionContext context)
        {
            context.CreateMoveErrorResult();
            result.Errors.Add(Error.NewError("E999", "Move failure", "The MOVE instruction failed."));
        }

        internal void AddError(ExecutionResult result, string desc)
        {
            result.Errors.Add(Error.NewError("E999", desc,
                "There was an unrecoverable error during a merchandizing operation."));
        }

        private void ClearQlmInventory()
        {
            var service = ServiceLocator.Instance.GetService<IDecksService>();
            ServiceLocator.Instance.GetService<IInventoryService>().MarkDeckInventory(service.QlmDeck, "UNKNOWN");
        }
    }
}