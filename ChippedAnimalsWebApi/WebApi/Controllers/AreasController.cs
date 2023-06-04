using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Dtos;
using Services.Management;
using System.ComponentModel.DataAnnotations;
using WebApi.Conventions;
using WebApi.Filters;

namespace WebApi.Controllers
{
    [Route("areas")]
    [ApiController]
    [ApiConventionType(typeof(AreaApiConvention))]
    public class AreasController : ControllerBase
    {
        readonly ILogger<AreasController> _logger;
        readonly IAreaManagementService _areaService;

        public AreasController(
            ILogger<AreasController> logger,
            IAreaManagementService areaService)
        {
            _logger = logger;
            _areaService = areaService;
        }

        [HttpGet("{areaId?}")]
        [Authorize]
        public async Task<ActionResult<AreaDto>> Show(
            [Range(1, long.MaxValue)] long? areaId)
        {
            _logger.LogInformation("areaId: {id}", areaId);
            AreaDto responseDto = await _areaService.GetByIdAsync(areaId);
            _logger.LogInformation("Response DTO: {dto}", responseDto);
            return Ok(responseDto);
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        [RootPathFilter]
        public async Task<ActionResult<AccountDto>> Create(
            [FromBody] AreaCreateDto requestBody)
        {
            _logger.LogInformation("Request body: {body}", requestBody);
            AreaDto responseDto = await _areaService.CreateAsync(requestBody);
            _logger.LogInformation("Response DTO: {dto}", responseDto);
            string? rootPath = (string?)HttpContext.Items["rootPath"];
            return Created($"{rootPath}{HttpContext.Request.Path}/{responseDto.Id}", responseDto);
        }

        [HttpPut("{areaId?}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<AccountDto>> Update(
            [Required, Range(1, long.MaxValue)] long? areaId,
            [FromBody] AreaUpdateDto requestBody)
        {
            _logger.LogInformation("areaId: {id}", areaId);
            _logger.LogInformation("Request body: {body}", requestBody);
            AreaDto responseDto = await _areaService.UpdateAsync(areaId, requestBody);
            _logger.LogInformation("Response DTO: {dto}", responseDto);
            return Ok(responseDto);
        }

        [HttpDelete("{areaId?}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult> Delete(
            [Required, Range(1, long.MaxValue)] long? areaId)
        {
            _logger.LogInformation("areaId: {id}", areaId);
            await _areaService.DeleteAsync(areaId);
            return Ok();
        }
    }
}
