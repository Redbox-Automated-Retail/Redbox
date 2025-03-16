using Redbox.Rental.Model.Personalization;
using System;
using System.Security;

namespace Redbox.Rental.Model.KioskClientService.CustomerProfile
{
    public interface IKioskClientServiceCustomerProfile
    {
        void LoginWithEmail(
            string emailAddress,
            SecureString password,
            Action<ILoginResult> loginResultCallback);

        void LoginWithPhoneNumber(
            string phoneNumber,
            SecureString pin,
            Action<ILoginResult> loginResultCallback);

        void LoginWithCustomerProfileNumber(
            string customerProfileNumber,
            Action<ILoginResult> loginResultCallback);
    }
}