using System;
using System.Linq;
using System.Collections.Generic;

namespace Logic.Models
{
    public class ForecastDto
    {
        public string City { get; set; }
        public double AverageTemperatureToday
        {
            get
            {
                var average = Items?.Count > 0 ? Items.Where(f => f.DateTime.Day == DateTime.Now.Day)
                                                      .Average(f => f.Temperature)
                              : 0.0;
                return Math.Round(average, 1);
            }
        }
        public double AverageTemperatureTomorrow
        {
            get
            {
                var average = Items?.Count > 0 ? Items.Where(f => f.DateTime.Day == DateTime.Now.Day + 1)
                                                      .Average(f => f.Temperature)
                   : 0.0;
                return Math.Round(average, 1);
            }
        }
        public string AverageIconToday
        {
            get => Items?.Count > 0 ?
                    Items.Where(foreCastItem => !string.IsNullOrEmpty(foreCastItem.Icon) && foreCastItem.DateTime.Day == DateTime.Now.Day)
                         .GroupBy(f => f.Icon)
                         .OrderByDescending(groupedForecasts => groupedForecasts.Key)
                         .FirstOrDefault().Key
                    : "";
        }
        public string AverageIconTomorrow
        {
            get => Items?.Count > 0 ?
                    Items.Where(foreCastItem => !string.IsNullOrEmpty(foreCastItem.Icon) && foreCastItem.DateTime.Day == DateTime.Now.Day + 1)
                         .GroupBy(f => f.Icon)
                         .OrderByDescending(groupedForecasts => groupedForecasts.Key)
                         .FirstOrDefault().Key
                    : "";
        }
        // Sätt dessa till <Image Source="{Binding AverageIconTodayUrl}">
        public string AverageIconTodayUrl { get => $"http://openweathermap.org/img/wn/{AverageIconToday}@2x.png"; }
        public string AverageIconTomorrowUrl { get => $"http://openweathermap.org/img/wn/{AverageIconTomorrow}@2x.png"; }
        public List<ForecastItemDto> Items { get; set; }
    }


    // TODO: Den här kan vi implementera om vi vill lista alla väderuppgifter framöver (kan bindas till en CollectionView)
    public class GroupedForecast
    {
        public string City { get; set; }
        public IEnumerable<IGrouping<DateTime, ForecastItemDto>> Items { get; set; }
    }
}
