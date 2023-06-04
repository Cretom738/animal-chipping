using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Services.Dtos;
using WebApi.Filters;
using Services.Management;
using WebApi.Conventions;

namespace WebApi.Controllers
{
    [Route("animals/types")]
    [ApiController]
    [ApiConventionType(typeof(AnimalTypeApiConvention))]
    public class AnimalTypesController : ControllerBase
    {
        readonly ILogger<AnimalTypesController> _logger;
        readonly IAnimalTypeManagementService _animalTypeManagmentService;

        public AnimalTypesController(
            ILogger<AnimalTypesController> logger,
            IAnimalTypeManagementService animalTypeManagmentService)
        {
            _logger = logger;
            _animalTypeManagmentService = animalTypeManagmentService;
        }

        [HttpGet("{typeId?}")]
        [Authorize]
        public async Task<ActionResult<AnimalTypeDto>> Show(
            [Required, Range(1, long.MaxValue)] long? typeId)
        {
            _logger.LogInformation("typeId: {id}", typeId);
            AnimalTypeDto responseDto = await _animalTypeManagmentService.GetByIdAsync(typeId);
            _logger.LogInformation("Response DTO: {dto}", responseDto);
            return Ok(responseDto);
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN,CHIPPER")]
        [RootPathFilter]
        public async Task<ActionResult<AnimalTypeDto>> Create(
            [FromBody] AnimalTypeCreateDto requestBody)
        {
            _logger.LogInformation("Request body: {body}", requestBody);
            AnimalTypeDto responseDto = await _animalTypeManagmentService.CreateAsync(requestBody);
            _logger.LogInformation("Response DTO: {dto}", responseDto);
            string? rootPath = (string?) HttpContext.Items["rootPath"];
            return Created($"{rootPath}{HttpContext.Request.Path}/{responseDto.Id}", responseDto);
        }

        [HttpPut("{typeId?}")]
        [Authorize(Roles = "ADMIN,CHIPPER")]
        public async Task<ActionResult<AnimalTypeDto>> Update(
            [Required, Range(1, long.MaxValue)] long? typeId, 
            [FromBody] AnimalTypeUpdateDto requestBody)
        {
            _logger.LogInformation("typeId: {id}", typeId);
            _logger.LogInformation("Request body: {body}", requestBody);
            AnimalTypeDto responseDto = await _animalTypeManagmentService
                .UpdateAsync(typeId, requestBody);
            _logger.LogInformation("Response DTO: {dto}", responseDto);
            return Ok(responseDto);
        }

        [HttpDelete("{typeId?}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult> Delete(
            [Required, Range(1, long.MaxValue)] long? typeId)
        {
            _logger.LogInformation("typeId: {id}", typeId);
            await _animalTypeManagmentService.DeleteAsync(typeId);
            return Ok();
        }
    }
}
