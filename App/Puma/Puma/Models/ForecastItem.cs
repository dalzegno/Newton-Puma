using System;
using Puma.Helpers;

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
        public string DescrAndWind { get => $"{Description.FirstCharToUpper()}, Wind: {WindSpeed}m/s"; }


        public override string ToString() => $"{Description.FirstCharToUpper()}, Temperature: {Temperature}°C, Wind: {WindSpeed} m/s";
    }
}
