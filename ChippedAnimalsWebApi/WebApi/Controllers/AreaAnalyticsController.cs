using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Analytics;
using Services.Dtos;
using System.ComponentModel.DataAnnotations;
using WebApi.Conventions;

namespace WebApi.Controllers
{
    [Route("areas")]
    [ApiController]
    [ApiConventionType(typeof(AreaAnalyticsApiConvention))]
    public class AreaAnalyticsController : ControllerBase
    {
        readonly ILogger<AreaAnalyticsController> _logger;
        readonly IAreaAnalyticsService _areaAnalyticsService;

        public AreaAnalyticsController(
            ILogger<AreaAnalyticsController> logger,
            IAreaAnalyticsService areaAnalyticsService)
        {
            _logger = logger;
            _areaAnalyticsService = areaAnalyticsService;
        }

        [HttpGet("{areaId?}/analytics")]
        [Authorize]
        public async Task<ActionResult<AreaAnalyticsDto>> Show(
            [Required, Range(1, long.MaxValue)] long? areaId,
            [FromQuery] AreaAnalyticsShowDto requestBody)
        {
            _logger.LogInformation("areaId: {id}", areaId);
            _logger.LogInformation("Request body: {body}", requestBody);
            AreaAnalyticsDto responseDto = await _areaAnalyticsService.GetAreaAnalyticsAsync(areaId, requestBody);
            _logger.LogInformation("Response DTO: {dto}", responseDto);
            return Ok(responseDto);
        }
    }
}
