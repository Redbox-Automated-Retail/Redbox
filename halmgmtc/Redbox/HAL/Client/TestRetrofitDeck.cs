namespace Redbox.HAL.Client
{
    public sealed class TestRetrofitDeck : JobExecutor
    {
        public TestRetrofitDeck(HardwareService service) : base(service)
        {
        }

        protected override string JobName => "test-retrofit-deck";
    }
}