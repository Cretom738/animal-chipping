using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Services.Dtos;
using WebApi.Filters;
using Services.Management;
using WebApi.Conventions;

namespace WebApi.Controllers
{
    [Route("animals")]
    [ApiController]
    [ApiConventionType(typeof(AnimalApiConvention))]
    public class AnimalsController : ControllerBase
    {
        readonly ILogger<AnimalsController> _logger;
        readonly IAnimalManagementService _animalService;

        public AnimalsController(
            ILogger<AnimalsController> logger,
            IAnimalManagementService animalService)
        {
            _logger = logger;
            _animalService = animalService;
        }

        [HttpGet("{animalId?}")]
        [Authorize]
        public async Task<ActionResult<AnimalDto>> Show(
            [Required, Range(1, long.MaxValue)] long? animalId)
        {
            _logger.LogInformation("animalId: {id}", animalId);
            AnimalDto responseDto = await _animalService.GetByIdAsync(animalId);
            _logger.LogInformation("Response DTO: {dto}", responseDto);
            return Ok(responseDto);
        }

        [HttpGet("search")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<AnimalDto>>> List(
            [FromQuery] AnimalListDto parameters)
        {
            _logger.LogInformation("Request parameters: {parameters}", parameters);
            IEnumerable<AnimalDto> responseDtos = await _animalService
                .GetListByFiltersAsync(parameters);
            _logger.LogInformation("Response DTOs: {dtos}", responseDtos);
            return Ok(responseDtos);
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN,CHIPPER")]
        [RootPathFilter]
        public async Task<ActionResult<AnimalDto>> Create(
            [FromBody] AnimalCreateDto requestBody)
        {
            _logger.LogInformation("Request body: {body}", requestBody);
            AnimalDto responseDto = await _animalService.CreateAsync(requestBody);
            _logger.LogInformation("Response DTO: {dto}", responseDto);
            string? rootPath = (string?) HttpContext.Items["rootPath"];
            return Created($"{rootPath}{HttpContext.Request.Path}/{responseDto.Id}", responseDto);
        }

        [HttpPut("{animalId?}")]
        [Authorize(Roles = "ADMIN,CHIPPER")]
        public async Task<ActionResult<AnimalDto>> Update(
            [Required, Range(1, long.MaxValue)] long? animalId,
            [FromBody] AnimalUpdateDto requestBody)
        {
            _logger.LogInformation("animalId: {id}", animalId);
            _logger.LogInformation("Request body: {body}", requestBody);
            AnimalDto responseDto = await _animalService.UpdateAsync(animalId, requestBody);
            _logger.LogInformation("Response DTO: {dto}", responseDto);
            return Ok(responseDto);
        }

        [HttpDelete("{animalId?}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult> Delete(
            [Required, Range(1, long.MaxValue)] long? animalId)
        {
            _logger.LogInformation("animalId: {id}", animalId);
            await _animalService.DeleteAsync(animalId);
            return Ok();
        }

        [HttpPost("{animalId?}/types/{typeId?}")]
        [Authorize(Roles = "ADMIN,CHIPPER")]
        [RootPathFilter]
        public async Task<ActionResult<AnimalDto>> Create(
            [Required, Range(1, long.MaxValue)] long? animalId,
            [Required, Range(1, long.MaxValue)] long? typeId)
        {
            _logger.LogInformation("animalId: {id}", animalId);
            _logger.LogInformation("typeId: {id}", typeId);
            AnimalDto responseDto = await _animalService.AddTypeAsync(animalId, typeId);
            _logger.LogInformation("Response DTO: {dto}", responseDto);
            string? rootPath = (string?)HttpContext.Items["rootPath"];
            return Created($"{rootPath}{HttpContext.Request.Path}", responseDto);
        }

        [HttpPut("{animalId?}/types")]
        [Authorize(Roles = "ADMIN,CHIPPER")]
        public async Task<ActionResult<AnimalDto>> Update(
            [Required, Range(1, long.MaxValue)] long? animalId,
            [FromBody] AnimalAnimalTypeUpdateDto requestBody)
        {
            _logger.LogInformation("animalId: {id}", animalId);
            _logger.LogInformation("Request body: {body}", requestBody);
            AnimalDto responseDto = await _animalService.UpdateTypeAsync(animalId, requestBody);
            _logger.LogInformation("Response DTO: {dto}", responseDto);
            return Ok(responseDto);
        }

        [HttpDelete("{animalId?}/types/{typeId?}")]
        [Authorize(Roles = "ADMIN,CHIPPER")]
        public async Task<ActionResult<AnimalDto>> Delete(
            [Required, Range(1, long.MaxValue)] long? animalId,
            [Required, Range(1, long.MaxValue)] long? typeId)
        {
            _logger.LogInformation("animalId: {id}", animalId);
            _logger.LogInformation("typeId: {id}", typeId);
            AnimalDto responseDto = await _animalService.DeleteTypeAsync(animalId, typeId);
            _logger.LogInformation("Response DTO: {dto}", responseDto);
            return Ok(responseDto);
        }
    }
}
