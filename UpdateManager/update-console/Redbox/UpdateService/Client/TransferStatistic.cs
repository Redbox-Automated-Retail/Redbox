using System;

namespace Redbox.UpdateService.Client
{
    internal class TransferStatistic
    {
        public long ID { get; set; }

        public Decimal Value { get; set; }

        public string FileName { get; set; }

        public Identifier Store { get; set; }

        public DateTime CreatedOn { get; set; }

        public Identifier Revision { get; set; }

        public Identifier Repository { get; set; }

        public TransferStatisticType Type { get; set; }
    }
}
