using Puma.Models;
using Puma.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Puma.ViewModels
{
    internal class WeatherViewModel : BaseViewModel
    {
        readonly IDialogService _dialogService;
        readonly IOpenWeatherService _weatherService;
        public WeatherViewModel(IOpenWeatherService weatherService, IDialogService dialogService)
        {
            _weatherService = weatherService;
            _dialogService = dialogService;
        }

        public double _avgTempToday;
        public string _avgIconUriToday;
        public double _avgTempTomorrow;
        public string _avgIconUriTomorrow;
        public bool WeatherPopUpBool { get; set; }

        public double AvgTempToday
        {
            get => _avgTempToday;
            set
            {
                _avgTempToday = value;
                OnPropertyChanged(nameof(AvgTempToday));
            }
        }
        public string AvgIconUriToday
        {
            get => _avgIconUriToday;
            set
            {
                _avgIconUriToday = value;
                OnPropertyChanged(nameof(AvgIconUriToday));
            }
        }
        public double AvgTempTomorrow
        {
            get => _avgTempTomorrow;
            set
            {
                _avgTempTomorrow = value;
                OnPropertyChanged(nameof(AvgTempTomorrow));
            }
        }
        public string AvgIconUriTomorrow
        {
            get => _avgIconUriTomorrow;
            set
            {
                if (value != null)
                {
                    _avgIconUriTomorrow = value;
                    OnPropertyChanged(nameof(AvgIconUriToday));

                }
            }
        }

        private void WeatherPopup(object obj)
        {
            WeatherPopUpBool = !WeatherPopUpBool;
            OnPropertyChanged(nameof(WeatherPopUpBool));
        }

        // Weather Collection
        public ObservableCollection<IGrouping<DateTime, ForecastItem>> _forecastCollection;

        public ObservableCollection<IGrouping<DateTime, ForecastItem>> ForecastCollection
        {
            get => _forecastCollection;
            set
            {
                _forecastCollection = value;
                OnPropertyChanged(nameof(ForecastCollection));
            }
        }

        public async void SetWeather(double lat, double lon)
        {
            var forecast = await GetWeatherFromDb(lat, lon);

            if (forecast == null)
                return;

            AvgIconUriToday = forecast.AverageIconTodayUrl;
            AvgIconUriTomorrow = forecast.AverageIconTomorrowUrl;
            AvgTempToday = forecast.AverageTemperatureToday;
            AvgTempTomorrow = forecast.AverageTemperatureTomorrow;

            GroupedForecast groupedForecast = new GroupedForecast
            {
                Items = forecast?.Items.GroupBy(f => f.DateTime.Date)
            };
            
            ForecastCollection = new ObservableCollection<IGrouping<DateTime, ForecastItem>>(groupedForecast.Items);

            OnPropertyChanged(nameof(AvgTempToday));
            OnPropertyChanged(nameof(AvgTempTomorrow));
            OnPropertyChanged(nameof(AvgIconUriToday));
            OnPropertyChanged(nameof(AvgIconUriTomorrow));
        }

        public async Task<Forecast> GetWeatherFromDb(double lat, double lon)
        {
            if (lat == 0 || lon == 0)
                return null;

            Forecast forecast = null;
            try
            {
                string roundedLat = Math.Round(lat, 1).ToString();
                string roundedLon = Math.Round(lon, 1).ToString();
                forecast = await _weatherService.GetForecastAsync(roundedLat, roundedLon);
            }
            catch (Exception e)
            {
                await _dialogService.ShowMessageAsync("Error", e.Message);
            }

            if (forecast == null)
                return null;

            return forecast;
        }
    }
}
