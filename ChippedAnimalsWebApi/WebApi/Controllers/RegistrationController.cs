using Microsoft.AspNetCore.Mvc;
using Services.Dtos;
using WebApi.Filters;
using Services.Management;
using WebApi.Conventions;

namespace WebApi.Controllers
{
    [Route("registration")]
    [ApiController]
    [ApiConventionType(typeof(RegistrationApiConvention))]
    public class RegistrationController : ControllerBase
    {
        readonly ILogger<RegistrationController> _logger;
        readonly IAccountRegistrationService _registrationService;

        public RegistrationController(ILogger<RegistrationController> logger,
            IAccountRegistrationService accountService)
        {
            _logger = logger;
            _registrationService = accountService;
        }

        [HttpPost]
        [ForbidAuthenticatedFilter]
        [RootPathFilter]
        public async Task<ActionResult<AccountDto>> Create(
            [FromBody] AccountRegistrationDto requestBody)
        {
            _logger.LogInformation("Request body: {body}", requestBody);
            AccountDto responseDto = await _registrationService.CreateAsync(requestBody);
            _logger.LogInformation("Response DTO: {dto}", responseDto);
            string? rootPath = (string?) HttpContext.Items["rootPath"];
            return Created($"{rootPath}/accounts/{responseDto.Id}", responseDto);
        }
    }
}
