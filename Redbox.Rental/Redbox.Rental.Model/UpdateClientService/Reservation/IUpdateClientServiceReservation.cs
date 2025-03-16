using System.Threading.Tasks;

namespace Redbox.Rental.Model.UpdateClientService.Reservation
{
    public interface IUpdateClientServiceReservation
    {
        Task<IRegisterWithBrokerServiceResponse> RegisterWithBrokerService(
            IRegisterWithBrokerServiceRequest registerWithBrokerServiceRequest);

        Task<IUnregisterFromBrokerServiceResponse> UnregisterFromBrokerService(
            IUnregisterFromBrokerServiceRequest unregisterWithBrokerServiceRequest);
    }
}