using Logic.Models;
using Logic.Services;
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
            var forecast = await _weatherService.Get(lat, lon, lang);

            if (forecast == null)
                return NotFound();

            return Ok(forecast);
        }
    }
}
