using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Globalization;
using System.Collections.Generic;

using Logic.Translators;
using Logic.Models;
using PumaDbLibrary;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Logic.Services
{
    public class WeatherService : IWeatherService
    {
        readonly HttpClient _httpClient;
        readonly PumaDbContext _context;

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
            return forecast;
        }


        /// <summary>
        /// Sends a GET request to the provided uri string and returns a Task<ForecastDto>
        /// </summary>
        /// <param name="uri"></param>
        /// <returns>ForecastDto object</returns>
        private async Task<ForecastDto> ReadWebApiAsync(string uri)
        {
            using (HttpClient _httpClient = new())
            {
                var response = await _httpClient.GetAsync(uri);
                response.EnsureSuccessStatusCode();
                var weatherData = await response.Content.ReadFromJsonAsync<WeatherApiData>();

                return ForecastTranslator.ToModel(weatherData); // Converts WeatherApiData to ForecastDto
            }
        }
    }
}
