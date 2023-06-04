using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Services.Dtos;
using WebApi.Filters;
using Services.Management;
using WebApi.Conventions;

namespace WebApi.Controllers
{
    [Route("animals/{animalId?}/locations")]
    [ApiController]
    [ApiConventionType(typeof(AnimalVisitedLocationApiConvention))]
    public class AnimalVisitedLocationsController : ControllerBase
    {
        readonly ILogger<AnimalVisitedLocationsController> _logger;
        readonly IAnimalVisitedLocationManagementService _animalVisitedLocationManagementService;

        public AnimalVisitedLocationsController(
            ILogger<AnimalVisitedLocationsController> logger,
            IAnimalVisitedLocationManagementService animalVisitedLocationManagementService)
        {
            _logger = logger;
            _animalVisitedLocationManagementService = animalVisitedLocationManagementService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<AnimalVisitedLocationDto>>> List(
            [Required, Range(1, long.MaxValue)] long? animalId, 
            [FromQuery] AnimalVisitedLocationListDto parameters)
        {
            _logger.LogInformation("animalId: {id}", animalId);
            _logger.LogInformation("Request parameters: {parameters}", parameters);
            IEnumerable<AnimalVisitedLocationDto> responseDtos =
                await _animalVisitedLocationManagementService
                    .GetListByFiltersAsync(animalId, parameters);
            _logger.LogInformation("Response DTOs: {dtos}", responseDtos);
            return Ok(responseDtos);
        }

        [HttpPost("{pointId?}")]
        [Authorize(Roles = "ADMIN,CHIPPER")]
        [RootPathFilter]
        public async Task<ActionResult<AnimalVisitedLocationDto>> Create(
            [Required, Range(1, long.MaxValue)] long? animalId,
            [Required, Range(1, long.MaxValue)] long? pointId)
        {
            _logger.LogInformation("animalId: {id}", animalId);
            _logger.LogInformation("locationId: {id}", pointId);
            AnimalVisitedLocationDto responseDto = await _animalVisitedLocationManagementService
                .CreateAsync(animalId, pointId);
            _logger.LogInformation("Response DTO: {dto}", responseDto);
            string? rootPath = (string?) HttpContext.Items["rootPath"];
            return Created($"{rootPath}{HttpContext.Request.Path}", responseDto);
        }

        [HttpPut]
        [Authorize(Roles = "ADMIN,CHIPPER")]
        public async Task<ActionResult<AnimalVisitedLocationDto>> Update(
            [Required, Range(1, long.MaxValue)] long? animalId,
            [FromBody] AnimalVisitedLocationUpdateDto requestBody)
        {
            _logger.LogInformation("animalId: {id}", animalId);
            _logger.LogInformation("Request body: {body}", requestBody);
            AnimalVisitedLocationDto responseDto = await _animalVisitedLocationManagementService
                .UpdateAsync(animalId, requestBody);
            _logger.LogInformation("Response DTO: {dto}", responseDto);
            return Ok(responseDto);
        }

        [HttpDelete("{visitedPointId?}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult> Delete(
            [Required, Range(1, long.MaxValue)] long? animalId,
            [Required, Range(1, long.MaxValue)] long? visitedPointId)
        {
            _logger.LogInformation("animalId: {id}", animalId);
            _logger.LogInformation("visitedPointId: {id}", visitedPointId);
            await _animalVisitedLocationManagementService.DeleteAsync(animalId, visitedPointId);
            return Ok();
        }
    }
}
