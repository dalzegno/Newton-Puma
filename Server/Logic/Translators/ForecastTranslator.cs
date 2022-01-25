using Logic.Helpers;
using Logic.Models;
using System.Linq;

namespace Logic.Translators
{
    public static class ForecastTranslator
    {
        /// <summary>
        /// Translates WeatherApiData to a ForecastDto object
        /// </summary>
        /// <param name="weatherData"></param>
        /// <returns>A translated ForecastDto object</returns>
        public static ForecastDto ToModel(WeatherApiData weatherData)
        {
            if (weatherData == null)
                return null;

            var forecast = new ForecastDto()
            {
                City = weatherData.city?.name ?? "unknown", // if name is null, set forecast.City to "unkown"
                Items = weatherData.list.Select(x => ToModel(x)).ToList(),
                
            };
            return forecast;
        }

        /// <summary>
        /// Translates a List object <b(not a List<>)</b> to a ForecastItemDto
        /// </summary>
        /// <param name="item"></param>
        /// <returns>A ForecastItemDto object</returns>
        public static ForecastItemDto ToModel(List item)
        {
            if (item == null)
                return null;

            return new ForecastItemDto
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
