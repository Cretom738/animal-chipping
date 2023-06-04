using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Services.Dtos;
using WebApi.Filters;
using Services.Management;
using WebApi.Conventions;

namespace WebApi.Controllers
{
    [Route("accounts")]
    [ApiController]
    [ApiConventionType(typeof(AccountApiConvention))]
    public class AccountsController : ControllerBase
    {
        readonly ILogger<AccountsController> _logger;
        readonly IAccountManagementService _accountService;

        public AccountsController(
            ILogger<AccountsController> logger,
            IAccountManagementService accountService)
        {
            _logger = logger;
            _accountService = accountService;
        }

        [HttpGet("{accountId?}")]
        [Authorize]
        public async Task<ActionResult<AccountDto>> Show(
            [Required, Range(1, int.MaxValue)] int? accountId)
        {
            _logger.LogInformation("accountId: {id}", accountId);
            AccountDto responseDto = await _accountService.GetByIdAsync(accountId, User.Claims);
            _logger.LogInformation("Response DTO: {dto}", responseDto);
            return Ok(responseDto);
        }

        [HttpGet("search")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<IEnumerable<AccountDto>>> List(
            [FromQuery] AccountListDto parameters)
        {
            _logger.LogInformation("Request parameters: {parameters}", parameters);
            IEnumerable<AccountDto> responseDtos = await _accountService
                .GetListByFiltersAsync(parameters);
            _logger.LogInformation("Response DTOs: {dtos}", responseDtos);
            return Ok(responseDtos);
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        [RootPathFilter]
        public async Task<ActionResult<AccountDto>> Create(
            [FromBody] AccountCreateDto requestBody)
        {
            _logger.LogInformation("Request body: {body}", requestBody);
            AccountDto responseDto = await _accountService.CreateAsync(requestBody);
            _logger.LogInformation("Response DTO: {dto}", responseDto);
            string? rootPath = (string?)HttpContext.Items["rootPath"];
            return Created($"{rootPath}{HttpContext.Request.Path}/{responseDto.Id}", responseDto);
        }

        [HttpPut("{accountId?}")]
        [Authorize]
        public async Task<ActionResult<AccountDto>> Update(
            [Required, Range(1, int.MaxValue)] int? accountId, 
            [FromBody] AccountUpdateDto requestBody)
        {
            _logger.LogInformation("accountId: {id}", accountId);
            _logger.LogInformation("Request body: {body}", requestBody);
            AccountDto responseDto = await _accountService
                .UpdateAsync(accountId, requestBody, User.Claims);
            _logger.LogInformation("Response DTO: {dto}", responseDto);
            return Ok(responseDto);
        }

        [HttpDelete("{accountId?}")]
        [Authorize]
        public async Task<ActionResult> Delete(
            [Required, Range(1, int.MaxValue)] int? accountId)
        {
            _logger.LogInformation("accountId: {id}", accountId);
            await _accountService.DeleteAsync(accountId, User.Claims);
            return Ok();
        }
    }
}
