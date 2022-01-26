using Puma.Models;
using System.Threading.Tasks;

namespace Puma.Services
{
    public interface IOpenWeatherService
    {
        Task<Forecast> GetForecastAsync(string city);
        Task<Forecast> GetForecastAsync(double latitude, double longitude);
        Task<Forecast> ReadWebApiAsync(string uri);
    }
}
