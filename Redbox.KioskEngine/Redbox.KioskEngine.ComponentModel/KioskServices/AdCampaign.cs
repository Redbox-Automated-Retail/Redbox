using System;
using System.Collections.Generic;

namespace Redbox.KioskEngine.ComponentModel.KioskServices
{
  public class AdCampaign
  {
    public Guid CampaignId { get; set; }

    public string Name { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public TimeSpan? StartTime { get; set; }

    public TimeSpan? EndTime { get; set; }

    public int? DaysOfWeek { get; set; }

    public List<AdImpression> Impressions { get; set; }
  }
}
