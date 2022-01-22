using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Globalization;
using System.Collections.Generic;

using Puma.Translators;
using Puma.Models;
using Puma.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(OpenWeatherService))]]
namespace Puma.Services
{
    public class OpenWeatherService
    {
        readonly HttpClient _httpClient = new HttpClient();
        readonly string _apiKey = "8cc9320af9a3d3526020f84de4718e80"; // TODO: Ni borde ha en egen API-nyckel på er login (https://openweathermap.org/)

        private ConcurrentDictionary<(string location, string expirationTime), Forecast> _forecastDictionary = new ConcurrentDictionary<(string, string), Forecast>();

        public EventHandler<string> WeatherForecastAvailable;

        protected virtual void OnNewForecastAvailable(string e) => WeatherForecastAvailable?.Invoke(this, e); // Invoke if not null

        /// <summary>
        /// Gets a forecast for a location with <b>city</b>
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        public async Task<Forecast> GetForecastAsync(string city)
        {
            //https://openweathermap.org/current
            var language = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            var uri = $"https://api.openweathermap.org/data/2.5/forecast?q={city}&units=metric&lang={language}&appid={_apiKey}";
            
            // Cache expiration time, expires after 1 hour
            string expirationTime = GetExpirationTime();

            // If a request is made on the same location within one minute – get cached Forecast
            if (_forecastDictionary.TryGetValue((city, expirationTime), out Forecast f))
            {
                OnNewForecastAvailable($"Cached forecast for {city} available");
                return f;
            }
            // Removes all expired caches
            RemoveExpiredCaches();

            Forecast forecast = await ReadWebApiAsync(uri);
            _forecastDictionary.TryAdd((city, expirationTime), forecast);
            OnNewForecastAvailable($"New weather forecast for {city} available");

            return forecast;
        }

        /// <summary>
        /// Gets a forecast for a location with <b>latitude</b> and <b>longitude</b>
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        public async Task<Forecast> GetForecastAsync(double latitude, double longitude)
        {
            //https://openweathermap.org/current
            var language = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            var uri = $"https://api.openweathermap.org/data/2.5/forecast?lat={latitude}&lon={longitude}&units=metric&lang={language}&appid={_apiKey}";
            string latAndLongKey = $"{latitude}, {longitude}";

            // Cache expiration time, expires after 1 minute
            string expirationTime = GetExpirationTime();

            // TODO: Detta behöver vi inte, men kan vara schysst!
            // If a request is made on the same location within one minute – get cached Forecast
            if (_forecastDictionary.TryGetValue((latAndLongKey, expirationTime), out Forecast f))
            {
                OnNewForecastAvailable($"Cached forecast for lat: {latitude} long: {longitude} available");
                return f;
            }

            RemoveExpiredCaches();

            Forecast forecast = await ReadWebApiAsync(uri);
            
            _forecastDictionary.TryAdd((latAndLongKey, expirationTime), forecast);
            OnNewForecastAvailable($"New weather forecast for (lat: {latitude} long: {longitude}) available");

            return forecast;
        }        

        /// <summary>
        /// Sends a GET request to the provided uri string and returns a Task<Forecast>
        /// </summary>
        /// <param name="uri"></param>
        /// <returns>Forecast object</returns>
        private async Task<Forecast> ReadWebApiAsync(string uri)
        {
            var response = await _httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var weatherData = await response.Content.ReadFromJsonAsync<WeatherApiData>();

            return ForecastTranslator.ToModel(weatherData); // Converts WeatherApiData to Forecast
        }

        /// <summary>
        /// Removes expired caches from the dictionary (1 minute after adding)
        /// </summary>
        private void RemoveExpiredCaches()
        {
            if (_forecastDictionary.Count > 0)
            {
                // List to save outdated request keys
                var outDatedRequestKeys = new List<(string, string)>();

                foreach (var item in _forecastDictionary)
                {
                    // These conversions needed to make the if-statement work
                    string strNow = DateTime.Now.ToString("yyyy/MM/dd HH");
                    DateTime dtNow = DateTime.Parse(strNow);

                    DateTime expirationTime = DateTime.Parse(item.Key.expirationTime);

                    if (expirationTime < dtNow)
                        outDatedRequestKeys.Add(item.Key);
                }

                if (outDatedRequestKeys.Count > 0)
                    outDatedRequestKeys.ForEach(k => _forecastDictionary.TryRemove(k, out _));
            }
        }
        private static string GetExpirationTime() => DateTime.Now.AddHours(1).ToString("yyyy/MM/dd HH");
    }
}
