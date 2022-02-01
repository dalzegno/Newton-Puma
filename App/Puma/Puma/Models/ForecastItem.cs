using System;
using System.Linq;

namespace Puma.Models
{
    public class ForecastItem
    {
        public DateTime DateTime { get; set; }
        public double Temperature { get; set; }
        public double WindSpeed { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public string IconUrl { get => $"http://openweathermap.org/img/wn/{Icon}@2x.png"; }
        public string DescrAndWind { get => $"{Description}, Wind: {WindSpeed}m/s"; }
    }
}
