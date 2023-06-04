using Microsoft.AspNetCore.Mvc;
using Services.Dtos;

#nullable disable
namespace WebApi.Conventions
{
    public static class AreaAnalyticsApiConvention
    {
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static void Show(long? areaId, AreaAnalyticsShowDto requestBody)
        {
        }
    }
}
#nullable restore
