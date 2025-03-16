using Redbox.KioskEngine.ComponentModel;
using Redbox.KioskEngine.ComponentModel.KioskServices;
using System.Collections.Generic;

namespace Redbox.Rental.Model.KioskClientService.Marketing
{
    public interface IKioskClientServiceMarketing
    {
        IRemoteServiceResult EmailConfirmation(long kioskId, string emailAddress, string accountNumber);

        ErrorList KioskOptIn(long kioskId, IEnumerable<IOptInItem> optInItems);
    }
}