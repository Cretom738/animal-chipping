using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Dtos;
using Services.Processing;
using System.Security.Cryptography;
using System.Text;
using WebApi.Conventions;

namespace WebApi.Controllers
{
    [Route("locations")]
    [ApiController]
    [ApiConventionType(typeof(GeohashApiConvention))]
    public class GeohashController : ControllerBase
    {
        readonly ILogger<GeohashController> _logger;
        readonly IGeohashService _geohashService;

        public GeohashController(
            ILogger<GeohashController> logger,
            IGeohashService geohashService)
        {
            _logger = logger;
            _geohashService = geohashService;
        }

        [HttpGet("geohash")]
        [Authorize]
        public Task<ActionResult<string>> Show(
            [FromQuery] GeohashShowDto parameters)
        {
            _logger.LogInformation("Request parameters: {parameters}", parameters);
            string geohash = _geohashService.Encode(
                parameters.Latitude!.Value, parameters.Longitude!.Value, 12);
            _logger.LogInformation("Geohash: {geohash}", geohash);
            return WrapToTask(Ok(geohash));
        }

        [HttpGet("geohashv2")]
        [Authorize]
        public Task<ActionResult<string>> ShowV2(
            [FromQuery] GeohashShowDto parameters)
        {
            _logger.LogInformation("Request parameters: {parameters}", parameters);
            string geohash = _geohashService.Encode(
                parameters.Latitude!.Value, parameters.Longitude!.Value, 12);
            _logger.LogInformation("Geohash: {geohash}", geohash);
            HttpContext.Response.Headers.TransferEncoding = "BASE64";
            return WrapToTask(Ok(ConvertToBase64(geohash)));
        }

        [HttpGet("geohashv3")]
        [Authorize]
        public Task<ActionResult<string>> ShowV3(
            [FromQuery] GeohashShowDto parameters)
        {
            _logger.LogInformation("Request parameters: {parameters}", parameters);
            string geohash = _geohashService.Encode(
                parameters.Latitude!.Value, parameters.Longitude!.Value, 12);
            _logger.LogInformation("Geohash: {geohash}", geohash);
            HttpContext.Response.Headers.TransferEncoding = "BASE64";
            return WrapToTask(Ok(ConvertToBase64Encrypted(geohash)));
        }

        Task<ActionResult<string>> WrapToTask(ActionResult result)
        {
            return Task.FromResult<ActionResult<string>>(result);
        }

        string ConvertToBase64(string geohash)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(geohash));
        }

        string ConvertToBase64Encrypted(string geohash)
        {
            byte[] reversedGeohashMd5 = MD5
                .HashData(Encoding.UTF8.GetBytes(geohash))
                .Reverse()
                .ToArray();
            return Convert.ToBase64String(reversedGeohashMd5);
        }
    }
}
