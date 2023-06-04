using Microsoft.AspNetCore.Mvc;
using Services.Dtos;

#nullable disable
namespace WebApi.Conventions
{
    public static class RegistrationApiConvention
    {
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public static void Create(AccountRegistrationDto requestBody)
        {
        }
    }
}
#nullable restore
