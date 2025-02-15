namespace Redbox.HAL.Client.Executors
{
    public sealed class HardwareStatusExecutor : JobExecutor
    {
        public HardwareStatusExecutor(HardwareService service) : base(service)
        {
        }

        public bool InventoryError { get; private set; }

        public bool VendDoorError { get; private set; }

        public bool HardwareOk { get; private set; }

        public bool MotionControlError { get; private set; }

        public bool PickerObstructed { get; private set; }

        protected override string JobName => "hardware-status";

        protected override void OnJobCompleted()
        {
            foreach (var result in Results)
                if (result.Code == "MotionControllerCommunicationError")
                    MotionControlError = true;
                else if (result.Code == "GripperObstructed")
                    PickerObstructed = true;
                else if (result.Code == "VendDoorOpen")
                    VendDoorError = true;
                else if (result.Code == "InventoryStoreError")
                    InventoryError = true;
                else if (result.Code == "HardwareStatusInError")
                    HardwareOk = false;
                else if (result.Code == "HardwareStatusOk")
                    HardwareOk = true;
        }
    }
}