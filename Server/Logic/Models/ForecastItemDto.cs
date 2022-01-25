using System;
using Logic.Helpers;

namespace Logic.Models
{
    public class ForecastItemDto
    {
        public DateTime DateTime { get; set; }
        public double Temperature { get; set; }
        public double WindSpeed { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public string IconUrl { get => $"http://openweathermap.org/img/wn/{Icon}@2x.png"; }
        public string DescrAndWind { get => $"{Description.FirstCharToUpper()}, Wind: {Math.Round(WindSpeed, 1)}m/s"; }
        public override string ToString() => $"{Description.FirstCharToUpper()}, Temperature: {Math.Round(Temperature, 1)}°C, Wind: {Math.Round(WindSpeed, 1)}m/s";
    }
}