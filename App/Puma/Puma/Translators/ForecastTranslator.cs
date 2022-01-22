using Puma.Helpers;
using Puma.Models;
using System.Linq;

namespace Puma.Translators
{
    public static class ForecastTranslator
    {
        /// <summary>
        /// Translates WeatherApiData to a Forecast object
        /// </summary>
        /// <param name="weatherData"></param>
        /// <returns>A translated Forecast object</returns>
        public static Forecast ToModel(WeatherApiData weatherData)
        {
            if (weatherData == null)
                return null;

            var forecast = new Forecast()
            {
                City = weatherData.city?.name ?? "unknown", // if name is null, set forecast.City to "unkown"
                Items = weatherData.list.Select(x => ToModel(x)).ToList(),
                
            };
            return forecast;
        }

        /// <summary>
        /// Translates a List object <b(not a List<>)</b> to a ForecastItem
        /// </summary>
        /// <param name="item"></param>
        /// <returns>A ForecastItem object</returns>
        public static ForecastItem ToModel(List item)
        {
            if (item == null)
                return null;

            return new ForecastItem
            {
                Temperature = item.main?.temp ?? 0, // if main is null, set temperature to 0
                DateTime = Utils.UnixTimeStampToDateTime(item.dt),
                WindSpeed = item.wind?.speed ?? 0, // if wind is null, set WindSpeed to 0
                Description = item.weather?.FirstOrDefault()?.description ?? "", // if weather is null, set Description to ""
                Icon = item.weather?.FirstOrDefault()?.icon ?? "" // if icon is null, set icon to ""
            };
        }
    }
}
