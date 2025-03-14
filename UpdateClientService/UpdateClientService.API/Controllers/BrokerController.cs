using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UpdateClientService.API.Controllers.Models;
using UpdateClientService.API.Services.Broker;

namespace UpdateClientService.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BrokerController : ControllerBase
    {
        private readonly IBrokerService _brokerService;
        private readonly IPingStatisticsService _pingStatisticsService;

        public BrokerController(
            IBrokerService brokerService,
            IPingStatisticsService pingStatisticsService)
        {
            _brokerService = brokerService;
            _pingStatisticsService = pingStatisticsService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(OkResult), 200)]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(500)]
        [ProducesResponseType(504)]
        public async Task<IActionResult> Ping(
            [FromHeader(Name = "app-name")] string appName,
            [FromHeader(Name = "x-redbox-activityid")]
            string messageId,
            PingRequest pingRequest)
        {
            return await _brokerService.Ping(appName, messageId, pingRequest);
        }

        [HttpPost]
        [ProducesResponseType(typeof(OkResult), 200)]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(500)]
        [ProducesResponseType(504)]
        public async Task<IActionResult> Register(
            [FromHeader(Name = "app-name")] string appName,
            [FromHeader(Name = "x-redbox-activityid")]
            string messageId,
            RegisterRequest request)
        {
            return await _brokerService.Register(appName, messageId, request);
        }

        [HttpPost]
        [ProducesResponseType(typeof(OkResult), 200)]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(500)]
        [ProducesResponseType(504)]
        public async Task<IActionResult> UnRegister(
            [FromHeader(Name = "app-name")] string appName,
            [FromHeader(Name = "x-redbox-activityid")]
            string messageId,
            UnRegisterRequest request)
        {
            return await _brokerService.Unregister(appName, messageId, request);
        }

        [HttpGet]
        [ProducesResponseType(typeof(PingStatisticsResponse), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PingStatistics()
        {
            return (await _pingStatisticsService.GetPingStatisticsResponse()).ToObjectResult();
        }

        [HttpGet]
        [ProducesResponseType(typeof(LastSuccessfulPingResponse), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> LastSuccessfulPing()
        {
            return (await _pingStatisticsService.GetLastSuccessfulPing()).ToObjectResult();
        }
    }
}