using Redbox.HAL.Core;

namespace Redbox.HAL.Client
{
    public sealed class SyncRange
    {
        public SyncRange()
        {
        }

        public SyncRange(int startDeck, int endDeck, Range slots)
        {
            StartDeck = startDeck;
            EndDeck = endDeck;
            Slots = slots;
        }

        public int EndDeck { get; set; }

        public Range Slots { get; set; }

        public int StartDeck { get; set; }
    }
}