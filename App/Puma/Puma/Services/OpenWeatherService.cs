﻿using System;
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
        readonly string _weatherApiUri = "http://localhost:64500/api/Weather";

        private ConcurrentDictionary<(string location, string expirationTime), Forecast> _forecastDictionary = new ConcurrentDictionary<(string, string), Forecast>();

        public EventHandler<string> WeatherForecastAvailable;

        protected virtual void OnNewForecastAvailable(string e) => WeatherForecastAvailable?.Invoke(this, e); // Invoke if not null
        protected virtual void OnErrorMessage(string e) => WeatherForecastAvailable?.Invoke(this, e); // Invoke if not null


        /// <summary>
        /// Gets a forecast for a location with <b>latitude</b> and <b>longitude</b>
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        public async Task<Forecast> GetForecastAsync(string latitude, string longitude)
        {
            string latAndLongKey = $"{latitude}, {longitude}";
            // Cache expiration time, expires after 1 hour
            string expirationTime = GetExpirationTime();

            // TODO: Detta behöver vi inte, men kan vara schysst!
            // If a request is made on the same location within one hour – get cached Forecast
            if (_forecastDictionary.TryGetValue((latAndLongKey, expirationTime), out Forecast f))
            {
                //OnNewForecastAvailable($"Cached forecast for lat: {latitude} long: {longitude} available");
                return f;
            }
            RemoveExpiredCaches();

            var language = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            var response = await _httpClient.GetAsync($"{_weatherApiUri}?lat={latitude}&lon={longitude}&lang={language}");
            
            if (!await response.IsResponseSuccessAsync())
                return null;

            Forecast forecast = await response.Content.ReadFromJsonAsync<Forecast>();

            _forecastDictionary.TryAdd((latAndLongKey, expirationTime), forecast);
            //OnNewForecastAvailable($"New weather forecast for (lat: {latitude} long: {longitude}) available");

            return forecast;
        }


        /// <summary>
        /// Removes expired caches from the dictionary (1 hour after adding)
        /// </summary>
        private void RemoveExpiredCaches()
        {
            if (_forecastDictionary.Count < 1)
                return;

            // List to save outdated request keys
            var outDatedRequestKeys = new List<(string, string)>();

            foreach (var item in _forecastDictionary)
            {
                // These conversions needed to make the if-statement work
                string strNow = DateTime.Now.ToString("yyyy/MM/dd HH:mm");
                DateTime dtNow = DateTime.Parse(strNow);

                DateTime expirationTime = DateTime.Parse(item.Key.expirationTime);

                if (expirationTime < dtNow)
                    outDatedRequestKeys.Add(item.Key);
            }

            if (outDatedRequestKeys.Count > 0)
                outDatedRequestKeys.ForEach(k => _forecastDictionary.TryRemove(k, out _));
        }
        private static string GetExpirationTime() => DateTime.Now.AddMinutes(60).ToString("yyyy/MM/dd HH:mm");
    }
}

