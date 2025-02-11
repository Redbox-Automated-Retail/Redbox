using Redbox.HAL.Component.Model;

namespace Redbox.HAL.Script.Framework
{
    internal sealed class VMZThinHelper : ThinHelper
    {
        internal VMZThinHelper(ExecutionResult result, ExecutionContext context)
            : base(result, context)
        {
        }

        protected override ILocation FindEmptyTarget(MerchFlags flags, out MerchandizeResult result)
        {
            LogHelper.Instance.Log(LogEntryType.Debug, "[FindEmptyTarget] Context = {0}", flags.ToString());
            if (ServiceLocator.Instance.GetService<IInventoryService>().MachineIsFull())
            {
                result = MerchandizeResult.MachineFull;
                return null;
            }

            result = MerchandizeResult.Success;
            using (var manager = new MerchTransferManager(Context, Debug))
            {
                var emptyTarget1 = SegmentManager.Instance.SelectLocation(flags, manager, out result);
                if (emptyTarget1 != null)
                {
                    if (Debug)
                        LogHelper.Instance.Log("[FindEmptyTarget] Select location found target at = {0}",
                            emptyTarget1.ToString());
                    return emptyTarget1;
                }

                if (Debug)
                    LogHelper.Instance.Log("[FindEmptyTarget] Select location didn't find a target; return code = {0}",
                        result.ToString());
                if (result != MerchandizeResult.VMZFull && result != MerchandizeResult.Success)
                    return null;
                var service = ServiceLocator.Instance.GetService<IDumpbinService>();
                if (service.IsFull())
                {
                    LogHelper.Instance.Log("[FindEmptyTarget] MoveLowPriorityToBin: the dump bin is full.");
                    result = MerchandizeResult.VMZFull;
                    return null;
                }

                var emptyTarget2 = (ILocation)null;
                foreach (var flags1 in SegmentManager.Instance.IncreasingPriorityOrder)
                    if (flags1 != flags)
                    {
                        var merchSegment = MerchSegmentFactory.Get(flags1);
                        if (merchSegment.CanDump)
                        {
                            var low = merchSegment.FindLow();
                            if (low != null)
                            {
                                var id = low.ID;
                                var flags2 = low.Flags;
                                if (FetchThin(low, id) != MerchandizeResult.Success)
                                {
                                    LogHelper.Instance.Log("Thin: cannot move low at {0}; bail.", low.ToString());
                                }
                                else
                                {
                                    var target = emptyTarget2 == null ? service.PutLocation : emptyTarget2;
                                    var location = ThinDiskToLocation(low, target, id, flags2);
                                    if (Debug)
                                        LogHelper.Instance.Log(
                                            "MoveLowPriorityToBin: transfer of {0} [Flags = {1}] to {2} returned {3}",
                                            low.ToString(), flags1.ToString(), target.ToString(), location.ToString());
                                    if (location != MerchandizeResult.Success)
                                    {
                                        result = location;
                                        return null;
                                    }

                                    emptyTarget2 = low;
                                }
                            }
                        }
                    }
                    else
                    {
                        break;
                    }

                if (emptyTarget2 != null)
                {
                    result = MerchandizeResult.Success;
                    return emptyTarget2;
                }

                result = MerchandizeResult.VMZFull;
                return null;
            }
        }

        private ILocation TryDumpBin(MerchFlags flags, out MerchandizeResult result)
        {
            return TryDumpBin(MerchSegmentFactory.Get(flags), out result);
        }

        private ILocation TryDumpBin(MerchSegment segment, out MerchandizeResult result)
        {
            if (!segment.CanDump)
            {
                LogHelper.Instance.Log("There are insufficient slots in the machine.");
                result = MerchandizeResult.MachineFull;
                return null;
            }

            var service = ServiceLocator.Instance.GetService<IDumpbinService>();
            if (service.IsFull())
            {
                Context.CreateInfoResult("DumpBinIsFull", "The dump bin is filled to capacity.");
                result = MerchandizeResult.MachineFull;
                return null;
            }

            result = MerchandizeResult.Success;
            return service.PutLocation;
        }
    }
}