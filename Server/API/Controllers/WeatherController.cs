using Logic.Models;
using Logic.Services;
using Logic.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class WeatherController : Controller
    {
        IWeatherService _weatherService;
        public WeatherController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        [HttpGet()]
        public async Task<ActionResult<ForecastDto>> Get([FromQuery]double lat, [FromQuery]double lon, [FromQuery] string lang)
        {
            if (!lat.TryParseToInvariantCulture(out double latDouble) || !lon.TryParseToInvariantCulture(out double lonDouble))
                return BadRequest("The format of lat or lon was wrong.");

            var forecast = await _weatherService.Get(latDouble, lonDouble, lang);

            if (forecast == null)
                return NotFound();

            return Ok(forecast);
        }
    }
}
