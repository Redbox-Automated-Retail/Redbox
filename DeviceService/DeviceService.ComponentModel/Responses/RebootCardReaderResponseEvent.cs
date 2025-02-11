using DeviceService.ComponentModel.Commands;

namespace DeviceService.ComponentModel.Responses
{
    public class RebootCardReaderResponseEvent : BaseResponseEvent
    {
        public RebootCardReaderResponseEvent(BaseCommandRequest baseCommandRequest)
            : base(baseCommandRequest)
        {
            EventName = nameof(RebootCardReaderResponseEvent);
        }
    }
}