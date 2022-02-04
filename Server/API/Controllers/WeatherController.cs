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
        public async Task<ActionResult<ForecastDto>> Get([FromQuery]string lat, [FromQuery]string lon, [FromQuery] string lang)
        {
            if (!lat.TryParseToDouble(out double latDouble) || !lon.TryParseToDouble(out double lonDouble))
                return BadRequest();

            var forecast = await _weatherService.Get(latDouble, lonDouble, lang);

            if (forecast == null)
                return NotFound();

            return Ok(forecast);
        }
    }
}
