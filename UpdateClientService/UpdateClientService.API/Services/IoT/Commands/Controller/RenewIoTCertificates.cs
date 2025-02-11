using System.Threading.Tasks;
using UpdateClientService.API.Services.IoT.Certificate;

namespace UpdateClientService.API.Services.IoT.Commands.Controller
{
    public class RenewIoTCertificates : ICommandIoTController
    {
        private readonly ICertificateService _certificateInstaller;

        public RenewIoTCertificates(ICertificateService certificateInstaller)
        {
            _certificateInstaller = certificateInstaller;
        }

        public CommandEnum CommandEnum => CommandEnum.RenewIoTCertificates;

        public int Version => 1;

        public async Task Execute(IoTCommandModel ioTCommand)
        {
            var certificateAsync = await _certificateInstaller.GetCertificateAsync(true);
        }
    }
}