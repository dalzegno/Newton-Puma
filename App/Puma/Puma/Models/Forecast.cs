using System;
using System.Linq;
using System.Collections.Generic;

namespace Puma.Models
{
    public class Forecast
    {
        public string City { get; set; }
        public double AverageTemperatureToday { get; set; }
        public double AverageTemperatureTomorrow { get; set; }  
        public string AverageIconToday { get; set; }
        public string AverageIconTomorrow { get; set; }
        
        // Sätt dessa till <Image Source="{Binding AverageIconTodayUrl}">
        public string AverageIconTodayUrl { get => $"http://openweathermap.org/img/wn/{AverageIconToday}@2x.png"; }
        public string AverageIconTomorrowUrl { get => $"http://openweathermap.org/img/wn/{AverageIconTomorrow}@2x.png"; }
        public List<ForecastItem> Items { get; set; }

    }

    // TODO: Den här kan vi implementera om vi vill lista alla väderuppgifter framöver (kan bindas till en CollectionView)
    public class GroupedForecast
    {
        public string City { get; set; }
        public IEnumerable<IGrouping<DateTime, ForecastItem>> Items { get; set; }
    }
}
