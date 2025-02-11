namespace Redbox.UpdateService.Model
{
    internal class PollReply
    {
        public PollReplyType PollReplyType { get; set; }

        public string CacheKey { get; set; }

        public int SyncId { get; set; }

        public string Data { get; set; }
    }
}
