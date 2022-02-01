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
        public string IconUrl { get; set; }
        public string DescrAndWind { get; set; }
    }
}