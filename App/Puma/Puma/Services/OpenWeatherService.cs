using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Globalization;
using System.Collections.Generic;

using Puma.Models;
using Puma.Services;
using Xamarin.Forms;
using Puma.Extensions;



[assembly: Dependency(typeof(OpenWeatherService))]
namespace Puma.Services
{
    public class OpenWeatherService : IOpenWeatherService
    {
        readonly IDialogService _dialogService = DependencyService.Get<IDialogService>();
        readonly HttpClient _httpClient = new HttpClient();
        readonly string _weatherApiUri = "https://localhost:44314/api/Weather";

        /// <summary>
        /// Gets a forecast for a location with <b>latitude</b> and <b>longitude</b>
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        public async Task<Forecast> GetForecastAsync(string latitude, string longitude)
        {
            if (latitude.Contains(",") || longitude.Contains(","))
            {
                latitude.Replace(",", ".");
                longitude.Replace(",", ".");
            }

            string latAndLongKey = $"{latitude}, {longitude}";
            var cacheKey = new ForecastCacheKey(latAndLongKey);

            if (cacheKey.CacheExists)
                return Forecast.Deserialize(cacheKey.FileName);

            Forecast.RemoveExpiredCaches();

            var language = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            var response = await _httpClient.GetAsync($"{_weatherApiUri}?lat={latitude}&lon={longitude}&lang={language}");

            if (!await response.IsResponseSuccessAsync(_dialogService))
                return null;

            Forecast forecast = await response.Content.ReadFromJsonAsync<Forecast>();
            Forecast.Serialize(forecast, cacheKey.FileName);
            return forecast;
        }
    }
}

