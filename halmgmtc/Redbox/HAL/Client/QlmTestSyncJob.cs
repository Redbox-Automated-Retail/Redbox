namespace Redbox.HAL.Client
{
    public sealed class QlmTestSyncJob : JobExecutor
    {
        protected override string JobName => "qlm-test-sync";

        public QlmTestSyncJob(HardwareService service) : base(service)
        {
        }
    }
}