using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Text.RegularExpressions;

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

        public static void Serialize(Forecast forecast, string fileName)
        {
            var _locker = new object();
            lock (_locker)
            {
                var xs = new XmlSerializer(typeof(Forecast));
                using (Stream s = File.Create(fileName))
                    xs.Serialize(s, forecast);
            }
        }
        public static Forecast Deserialize(string fileName)
        {
            var _locker = new object();
            lock (_locker)
            {
                Forecast forecast;
                var xs = new XmlSerializer(typeof(Forecast));

                using (Stream s = File.OpenRead(fileName))
                    forecast = (Forecast)xs.Deserialize(s);

                return forecast;
            }
        }

        public static void RemoveExpiredCaches()
        {
            string documentPath = GetDocumentPath();

            if (!Directory.Exists(documentPath))
                return;

            string[] fileNames = Directory.GetFiles(documentPath); // Gets all filenames, including full path

            if (fileNames.Length < 1)
                return;

            var expiredCaches = new List<string>();
            foreach (string fileName in fileNames)
            {
                if (IsCacheExpired(fileName))
                    expiredCaches.Add(fileName);
            }

            if (expiredCaches.Any())
                DeleteExpiredCachesFromList(expiredCaches);
        }

        private static string GetDocumentPath()
        {
            var documentPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            documentPath = Path.Combine(documentPath, "PuMap", "WeatherCache");
            return documentPath;
        }

        private static void DeleteExpiredCachesFromList(List<string> outDatedCaches)
        {
            outDatedCaches.ForEach(cache => File.Delete(cache));
        }

        private static bool IsCacheExpired(string fileName)
        {
            // A cachekey is "Latitude, Longitude, TimeWindow,.xml"
            var cacheKeys = fileName.Split(',');

            string strNow = DateTime.Now.ToString("yyyy/MM/dd HH:mm");
            DateTime dtNow = DateTime.Parse(strNow);

            var timeWindow = cacheKeys[2];
            string strTimeWindow = Regex.Replace(timeWindow, @"\.", ":");

            DateTime expirationTime = DateTime.Parse(strTimeWindow);

            if (expirationTime < dtNow)
                return true;

            return false;
        }
    }

    public class GroupedForecast
    {
        public string City { get; set; }
        public IEnumerable<IGrouping<DateTime, ForecastItem>> Items { get; set; }
    }

    public class ForecastCacheKey
    {
        readonly string _latAndLongKey;
        readonly string _timeWindow;

        public string LatAndLongKey => _latAndLongKey;
        public string FileName => Fname($"{Key},.xml");
        public string Key => $"{_latAndLongKey}, {_timeWindow}";
        public bool CacheExists => File.Exists(FileName);

        public ForecastCacheKey(string latAndLongKey)
        {
            _latAndLongKey = latAndLongKey;
            var dateTimeRounded = RoundToHour(DateTime.Now.AddHours(1));
            _timeWindow = dateTimeRounded.ToString("yyyy-MM-dd HH.mm");
        }

        static string Fname(string name)
        {
            var documentPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            documentPath = Path.Combine(documentPath, "PuMap", "WeatherCache");
            
            if (!Directory.Exists(documentPath)) 
                Directory.CreateDirectory(documentPath);
            
            return Path.Combine(documentPath, name);
        }

        static DateTime RoundToHour(DateTime dt)
        {
            long ticks = dt.Ticks + 18000000000;
            return new DateTime(ticks - ticks % 36000000000, dt.Kind);
        }
    }
}
