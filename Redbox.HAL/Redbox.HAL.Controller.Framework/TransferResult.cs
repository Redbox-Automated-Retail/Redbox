using Redbox.HAL.Component.Model;

namespace Redbox.HAL.Controller.Framework
{
    internal sealed class TransferResult : ITransferResult
    {
        internal TransferResult()
        {
            TransferError = ErrorCodes.Success;
        }

        public bool ReturnedToSource { get; internal set; }

        public bool Transferred => TransferError == ErrorCodes.Success && Destination != null;

        public ErrorCodes TransferError { get; internal set; }

        public ILocation Source { get; internal set; }

        public ILocation Destination { get; internal set; }
    }
}