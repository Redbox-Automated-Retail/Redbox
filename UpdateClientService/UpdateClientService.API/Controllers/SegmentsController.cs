using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UpdateClientService.API.Services.Segment;

namespace UpdateClientService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SegmentsController : ControllerBase
    {
        private readonly ISegmentService _segmentService;

        public SegmentsController(ISegmentService segmentService)
        {
            _segmentService = segmentService;
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(503)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateKioskSegmentsFromUpdateService()
        {
            return new StatusCodeResult((int)(await _segmentService.UpdateKioskSegmentsFromUpdateService()).StatusCode);
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetKioskSegments()
        {
            return (await _segmentService.GetKioskSegments()).ToObjectResult();
        }
    }
}