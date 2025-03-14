namespace Redbox.HAL.Client.Executors
{
    public sealed class InitJob : JobExecutor
    {
        public InitJob(HardwareService service) : base(service)
        {
        }

        protected override string JobName => "init";

        protected override string Label => "MS Tester Init";
    }
}