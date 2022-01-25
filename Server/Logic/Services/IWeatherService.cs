
using Logic.Models;
using System.Threading.Tasks;

namespace Logic.Services
{
    public interface IWeatherService
    {
        Task<ForecastDto> Get(double latitude, double longitude, string language);
    }
}
