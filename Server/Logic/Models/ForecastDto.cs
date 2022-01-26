using System;
using System.Linq;
using System.Collections.Generic;

namespace Logic.Models
{
    public class ForecastDto
    {
        public string City { get; set; }
        public double AverageTemperatureToday { get; set; }
        public double AverageTemperatureTomorrow { get; set; }
        public string AverageIconToday { get; set; }   
        public string AverageIconTomorrow { get; set; }
        public List<ForecastItemDto> Items { get; set; }
    }


    // TODO: Den här kan vi implementera om vi vill lista alla väderuppgifter framöver (kan bindas till en CollectionView)
    public class GroupedForecast
    {
        public string City { get; set; }
        public IEnumerable<IGrouping<DateTime, ForecastItemDto>> Items { get; set; }
    }
}
