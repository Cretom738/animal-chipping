using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Services.Dtos;
using WebApi.Filters;
using Services.Management;
using WebApi.Conventions;

namespace WebApi.Controllers
{
    [Route("locations")]
    [ApiController]
    [ApiConventionType(typeof(LocationApiConvention))]
    public class LocationsController : ControllerBase
    {
        readonly ILogger<LocationsController> _logger;
        readonly ILocationManagementService _locationManagementService;

        public LocationsController(
            ILogger<LocationsController> logger,
            ILocationManagementService locationManagementService)
        {
            _logger = logger;
            _locationManagementService = locationManagementService;
        }

        [HttpGet("{pointId?}")]
        [Authorize]
        public async Task<ActionResult<LocationDto>> Show(
            [Required, Range(1, long.MaxValue)] long? pointId)
        {
            _logger.LogInformation("pointId: {id}", pointId);
            LocationDto responseDto = await _locationManagementService.GetByIdAsync(pointId);
            _logger.LogInformation("Response DTO: {dto}", responseDto);
            return Ok(responseDto);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<long>> Show(
            [FromQuery] LocationShowDto parameters)
        {
            _logger.LogInformation("Request parameters: {parameters}", parameters);
            long pointId = await _locationManagementService.GetIdByCoordinatesAsync(parameters);
            _logger.LogInformation("Fetched pointId: {id}", pointId);
            return Ok(pointId);
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN,CHIPPER")]
        [RootPathFilter]
        public async Task<ActionResult<LocationDto>> Create(
            [FromBody] LocationCreateDto requestBody)
        {
            _logger.LogInformation("Request body: {body}", requestBody);
            LocationDto responseDto = await _locationManagementService.CreateAsync(requestBody);
            _logger.LogInformation("Response DTO: {dto}", responseDto);
            string? rootPath = (string?) HttpContext.Items["rootPath"];
            return Created($"{rootPath}{HttpContext.Request.Path}/{responseDto.Id}", responseDto);
        }

        [HttpPut("{pointId?}")]
        [Authorize(Roles = "ADMIN,CHIPPER")]
        public async Task<ActionResult<LocationDto>> Update(
            [Required, Range(1, long.MaxValue)] long? pointId, 
            [FromBody] LocationUpdateDto requestBody)
        {
            _logger.LogInformation("pointId: {id}", pointId);
            _logger.LogInformation("Request body: {body}", requestBody);
            LocationDto responseDto = await _locationManagementService
                .UpdateAsync(pointId, requestBody);
            _logger.LogInformation("Response DTO: {dto}", responseDto);
            return Ok(responseDto);
        }

        [HttpDelete("{pointId?}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult> Delete(
            [Required, Range(1, long.MaxValue)] long? pointId)
        {
            _logger.LogInformation("pointId: {id}", pointId);
            await _locationManagementService.DeleteAsync(pointId);
            return Ok();
        }
    }
}
