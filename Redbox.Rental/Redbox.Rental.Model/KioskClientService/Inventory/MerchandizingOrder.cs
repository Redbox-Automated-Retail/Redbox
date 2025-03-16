using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;

namespace Redbox.Rental.Model.KioskClientService.Inventory
{
    public class MerchandizingOrder
    {
        public int KioskId { get; set; }

        [Required]
        [EnumDataType(typeof(MerchandizingOrderType))]
        [JsonConverter(typeof(StringEnumConverter))]
        public MerchandizingOrderType MerchandizingOrderType { get; set; }

        public long TitleId { get; set; }

        public int UnitCount { get; set; }
    }
}