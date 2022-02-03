using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Puma.Models;
using Puma.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms.Xaml;
using System.Linq;
using Puma.Views;
using System.Threading.Tasks;

namespace Puma.ViewModels
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    class PoiViewModel : BaseViewModel
    {
        readonly IPoiService _poiService;
        readonly IDialogService _dialogService;
        readonly IOpenWeatherService _weatherService;
        public PoiViewModel(IPoiService poiService, IDialogService dialogService, IOpenWeatherService weatherService)
        {
            _poiService = poiService;
            _dialogService = dialogService;
            _weatherService = weatherService;
            _tags = new List<Tag>();
            _tagButtons = new ObservableCollection<Tag>();
            GetTagsFromDb();
            //RemoveTagCommand = new Command<Tag>(RemoveTag);
        }

        Command _createPoiCommand;
        Command _removeTagCommand;
        Command _poiCollectionPopupCommand;
        Command _poiSingleViewCommand;
        Command _poiCreationPopupCommand;
        Command _selectedPoiCommand;

        public Command SelectedPoiCommand => _selectedPoiCommand ?? (_selectedPoiCommand = new Command(SelectPoi));

        Command _weatherPopupCommand;


        public Command CreatePoiCommand => _createPoiCommand ?? (_createPoiCommand = new Command(CreatePoi, CanCreate));
        public Command RemoveTagCommand => _removeTagCommand ?? (_removeTagCommand = new Command(RemoveTag));

        public Command PoiCollectionPopupCommand => _poiCollectionPopupCommand ?? (_poiCollectionPopupCommand = new Command(PoiCollectionPopup));
        public Command PoiCreationPopupCommand => _poiCreationPopupCommand ?? (_poiCreationPopupCommand = new Command(PoiCreationPopup));
        public Command WeatherPopupCommand => _weatherPopupCommand ?? (_weatherPopupCommand = new Command(WeatherPopup));
        public Command PoiSingleViewCommand => _poiSingleViewCommand ?? (_poiSingleViewCommand = new Command(PoiSinglePopup));


        public string _name;
        public string _description;
        public string _country { get; set; } = "Click or search the map";
        public string _area { get; set; } = "to start exploring!";
        public string _streetName;
        public string _longitude;
        public string _latitude;
        public double _avgTempToday;
        public string _avgIconUriToday;
        public double _avgTempTomorrow;
        public string _avgIconUriTomorrow;
        public PointOfInterest _selectedSinglePoi;

        public bool openPoiCreationBool { get; set; } = false;
        public bool openPoiCollectionBool { get; set; } = false;
        public bool poiCollectionVisibleBool { get; set; } = true;
        public bool poiSingleVisibleBool { get; set; } = false;
        public bool openWeatherPopupBool { get; set; } = false;


        #region Fields
        // Poi Collection
        public ObservableCollection<PointOfInterest> _poiCollection;
        public ObservableCollection<PointOfInterest> PoiCollection
        {
            get => _poiCollection;
            set
            {
                _poiCollection = value;
                OnPropertyChanged(nameof(PoiCollection));
            }
        }

        public PointOfInterest SelectedSinglePoi
        {
            get => _selectedSinglePoi;
            set
            {
                _selectedSinglePoi = value;
                OnPropertyChanged(nameof(SelectedSinglePoi));
            }
        }
            

        public ObservableCollection<IGrouping<DateTime, ForecastItem>> _forecastCollection;
        // Weather Collection
        public ObservableCollection<IGrouping<DateTime, ForecastItem>> ForecastCollection
        {
            get => _forecastCollection;
            set
            {
                _forecastCollection = value;
                OnPropertyChanged(nameof(ForecastCollection));
            }
        }

        // TAGS
        public List<Tag> _tags;
        public List<Tag> Tags
        {
            get => _tags;
            set
            {
                _tags = value;
                OnPropertyChanged(nameof(Tags));
            }
        }
        public ObservableCollection<Tag> _tagButtons;
        public ObservableCollection<Tag> TagButtons
        {
            get => _tagButtons;
            set
            {
                _tagButtons = value;
                OnPropertyChanged(nameof(TagButtons));
            }
        }
        public Tag _tag;
        public Tag SelectedTagInfo
        {
            get => _tag;
            set
            {
                _tag = value;
                TagButtons.Add(value);

                OnPropertyChanged();
                OnPropertyChanged(nameof(TagButtons));

            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(Name);
                CreatePoiCommand.ChangeCanExecute();
            }
        }
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged(Description);
                CreatePoiCommand.ChangeCanExecute();
            }
        }
        public string Country
        {
            get => _country;
            set
            {
                _country = value;
                OnPropertyChanged(Country);
                CreatePoiCommand.ChangeCanExecute();
            }
        } 
        public string Area
        {
            get => _area;
            set
            {
                _area = value;
                OnPropertyChanged(Area);
                CreatePoiCommand.ChangeCanExecute();
            }
        }

        public string StreetName
        {
            get => _streetName;
            set
            {
                _streetName = value;
                OnPropertyChanged(StreetName);
                CreatePoiCommand.ChangeCanExecute();
            }
        }
        public string Latitude
        {
            get => _latitude;
            set
            {
                _latitude = value;
                OnPropertyChanged(Latitude);
                CreatePoiCommand.ChangeCanExecute();
            }
        }
        public string Longitude
        {
            get => _longitude;
            set
            {
                _longitude = value;
                OnPropertyChanged(Longitude);
                CreatePoiCommand.ChangeCanExecute();
            }
        }
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
        #endregion
        private bool CanCreate() => !string.IsNullOrWhiteSpace(Description) && !string.IsNullOrWhiteSpace(Name) && App.LoggedInUser != null;
        private async void CreatePoi()
        {
            var poi = new AddPoiDto()
            {
                Name = Name,
                Description = Description,
                Position = new PositionPoi
                {
                    Latitude = Convert.ToDouble(Latitude),
                    Longitude = Convert.ToDouble(Longitude)
                },
                Address = new Address
                {
                    StreetName = StreetName,
                    Area = Area,
                    Country = Country
                },
                TagIds = TagButtons.Count > 0 ? TagButtons.Select(x => x.Id).ToList() : new List<int>()
            };

            var createdPoi = await _poiService.CreatePoiAsync(poi);
            if (createdPoi != null)
            {
                await _dialogService.ShowMessageAsync("Welcome!", $"Welcome to PUMA \"{createdPoi.Name}\".");
                return;
            }
        }

        private void RemoveTag(object tag)
        {
            TagButtons.Remove((Tag)tag);
        }
    
        private async void PoiCollectionPopup()
        {
            //openPoiCollectionBool = !openPoiCollectionBool;
            //openPoiCreationBool = false;
            poiCollectionVisibleBool = true;
            poiSingleVisibleBool = false;
            //openWeatherPopupBool = false;

            
            OnPropertyChanged(nameof(PoiCollectionVisible));
            OnPropertyChanged(nameof(PoiSingleVisible));
            //OnPropertyChanged(nameof(poiCollectionPopupState));
            //OnPropertyChanged(nameof(PoiCreationPopupState));
            //OnPropertyChanged(nameof(WeatherPopupState));
            PoiCollection = await _poiService.GetAllAsync();
            OnPropertyChanged(nameof(PoiCollection));
        }
        private void PoiCreationPopup()
        {
            openPoiCreationBool = !openPoiCreationBool;
            openPoiCollectionBool = false;
            openWeatherPopupBool = false;
            OnPropertyChanged(nameof(PoiCreationPopupState));
            OnPropertyChanged(nameof(poiCollectionPopupState));
            OnPropertyChanged(nameof(WeatherPopupState));
        }

        private async void WeatherPopup()
        {
            //openWeatherPopupBool = !openWeatherPopupBool;
            //openPoiCreationBool = false;
            //openPoiCollectionBool = false;
            //OnPropertyChanged(nameof(PoiCreationPopupState));
            //OnPropertyChanged(nameof(poiCollectionPopupState));
            //OnPropertyChanged(nameof(WeatherPopupState));
            //if (openWeatherPopupBool)
                ForecastCollection = await GetWeatherFromDb();
            OnPropertyChanged(nameof(ForecastCollection));
        }
        private async void PoiSinglePopup(object param)
        {
            SelectedSinglePoi = (PointOfInterest)param;
            OnPropertyChanged(nameof(SelectedSinglePoi));

            poiCollectionVisibleBool = !poiCollectionVisibleBool;
            poiSingleVisibleBool = !poiSingleVisibleBool;
            OnPropertyChanged(nameof(PoiCollectionVisible));
            OnPropertyChanged(nameof(PoiSingleVisible));
            //_poiCollection = PoiCollection;
            //OnPropertyChanged(nameof(PoiCollection));
        }
        private void SelectPoi()
        {
            
            if (SelectedSinglePoi == null)
            {
                return;
            }
            MainPage.Instance.GoToLocation(SelectedSinglePoi, .5);

            poiCollectionVisibleBool = !poiCollectionVisibleBool;
            poiSingleVisibleBool = !poiSingleVisibleBool;

            //poiCollectionVisibleBool = !poiCollectionVisibleBool;
            //poiSingleVisibleBool = !poiSingleVisibleBool;

            OnPropertyChanged(nameof(PoiCollectionVisible));
            OnPropertyChanged(nameof(PoiSingleVisible));

            MainPage.Instance.GoToLocation(SelectedSinglePoi, .5);
            SelectedSinglePoi = null;
            OnPropertyChanged(nameof(SelectedSinglePoi));

        }

        public void SetAddress(string address)
        {
            var words = address?.Split('\n') ?? Array.Empty<string>();
            if (address == null || address == "")
            {
                StreetName = "";
                Area = "No location found";
                Country = "";
                OnPropertyChanged(nameof(StreetName));
                OnPropertyChanged(nameof(Area));
                OnPropertyChanged(nameof(Country));
            }
            else if (words.Length == 2)
            {
                StreetName = "";
                Area = words[0];
                Country = words[1];
                OnPropertyChanged(nameof(StreetName));
                OnPropertyChanged(nameof(Area));
                OnPropertyChanged(nameof(Country));
            }
            else if (words.Length == 3)
            {
                StreetName = words[0];
                Area = words[1];
                Country = words[2];
                OnPropertyChanged(nameof(StreetName));
                OnPropertyChanged(nameof(Area));
                OnPropertyChanged(nameof(Country));
            }
            
        }

        public bool PoiCreationPopupState => openPoiCreationBool;
        public bool poiCollectionPopupState => openPoiCollectionBool;
        public bool PoiCollectionVisible => poiCollectionVisibleBool;
        public bool PoiSingleVisible => poiSingleVisibleBool;

        public bool WeatherPopupState => openWeatherPopupBool;

        public async void GetTagsFromDb()
        {
            Tags = await _poiService.GetTags();
        }

        public async Task<ObservableCollection<IGrouping<DateTime, ForecastItem>>> GetWeatherFromDb()
        {
            if (Latitude == null || Longitude == null)
                return null;

            Forecast forecast = null;
            try
            {
                forecast = await _weatherService.GetForecastAsync(Latitude, Longitude);
            }
            catch (Exception e)
            {
                await _dialogService.ShowMessageAsync("Error", e.Message);
            }

            if (forecast == null)
                return null;

            GroupedForecast groupedForecast = new GroupedForecast
            {
                Items = forecast?.Items.GroupBy(f => f.DateTime.Date)
            };

            AvgIconUriToday = forecast.AverageIconTodayUrl;
            AvgIconUriTomorrow = forecast.AverageIconTomorrowUrl;
            AvgTempToday = forecast.AverageTemperatureToday;
            AvgTempTomorrow = forecast.AverageTemperatureTomorrow;


            return new ObservableCollection<IGrouping<DateTime, ForecastItem>>(groupedForecast.Items);
        }
    }
}
