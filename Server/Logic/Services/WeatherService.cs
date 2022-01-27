using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

using Logic.Translators;
using Logic.Models;
using PumaDbLibrary;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Logic.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly PumaDbContext _context;

        public WeatherService(PumaDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets a forecast for a location with <b>latitude and longitude</b>
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        public async Task<ForecastDto> Get(double latitude, double longitude, string language)
        {
            string apiKey = await _context.Apis.Where(api => api.Name == "OpenWeatherMap")
                                               .Select(api => api.Key)
                                               .FirstOrDefaultAsync();
            if (apiKey == null)
                throw new Exception("Could not find the api key.");

            var uri = $"https://api.openweathermap.org/data/2.5/forecast?lat={latitude}&lon={longitude}&units=metric&lang={language}&appid={apiKey}";

            ForecastDto forecast = await ReadWebApiAsync(uri);
            SetAveragesForForecast(forecast);

            return forecast;
        }


        /// <summary>
        /// Sends a GET request to the provided uri string and returns a Task<ForecastDto>
        /// </summary>
        /// <param name="uri"></param>
        /// <returns>ForecastDto object</returns>
        private async static Task<ForecastDto> ReadWebApiAsync(string uri)
        {
            using (HttpClient _httpClient = new())
            {
                var response = await _httpClient.GetAsync(uri);
                response.EnsureSuccessStatusCode();
                var weatherData = await response.Content.ReadFromJsonAsync<WeatherApiData>();

                return ForecastTranslator.ToModel(weatherData); // Converts WeatherApiData to ForecastDto
            }
        }

        private static void SetAveragesForForecast(ForecastDto forecast)
        {
            bool today = true;
            SetAverageTemperature(forecast, today);
            SetAverageTemperature(forecast, !today);

            SetAverageIcon(forecast, today);
            SetAverageIcon(forecast, !today);
        }

        private static void SetAverageIcon(ForecastDto forecast, bool today)
        {
            var average = forecast.Items?.Count > 0 ?
                                        forecast.Items.Where(foreCastItem => !string.IsNullOrEmpty(foreCastItem.Icon) &&
                                                            foreCastItem.DateTime.Day == (today ? DateTime.Now.Day : DateTime.Now.Day + 1))
                                                      .GroupBy(f => f.Icon)
                                                      .OrderByDescending(groupedForecasts => groupedForecasts.Key)
                                                      .FirstOrDefault().Key
                                                      : "";

            if (today)
            {
                forecast.AverageIconToday = average;
                return;
            }

            forecast.AverageIconTomorrow = average;
        }

        private static void SetAverageTemperature(ForecastDto forecast, bool today)
        {

            var average = forecast.Items?.Count > 0 ?
                                        forecast.Items.Where(f => f.DateTime.Day == (today ? DateTime.Now.Day : DateTime.Now.Day + 1))
                                                      .Average(f => f.Temperature)
                                        : 0.0;

            if (today)
            {
                forecast.AverageTemperatureToday = Math.Round(average, 1);
                return;
            }

            forecast.AverageTemperatureTomorrow = Math.Round(average, 1);
        }
    }
}
