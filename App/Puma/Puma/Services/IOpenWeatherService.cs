using Puma.Models;
using System.Threading.Tasks;

namespace Puma.Services
{
    public interface IOpenWeatherService
    {
        Task<Forecast> GetForecastAsync(string latitude, string longitude);
    }
}
