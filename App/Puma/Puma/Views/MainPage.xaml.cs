using Puma.CustomRenderer;
using Puma.Models;
using Puma.Services;
using Puma.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;


namespace Puma.Views
{

    public partial class MainPage : ContentPage
    {
        private readonly Geocoder _geoCoder;

        public static MainPage Instance { get; set; }
        internal MainViewModel MainViewModel { get; }
        internal PoiViewModel PoiViewModel { get; }
        IUserApiService UserApiService => DependencyService.Get<IUserApiService>();
        IDialogService DialogService => DependencyService.Get<IDialogService>();
        IPoiService PoiService => DependencyService.Get<IPoiService>();
        IOpenWeatherService WeatherService => DependencyService.Get<IOpenWeatherService>();

        

        public MainPage()
        {
            InitializeComponent();
            Instance = this;
            // Internal viewmodels that can be reached globally
            MainViewModel = new MainViewModel(DialogService);
            PoiViewModel = new PoiViewModel(PoiService, DialogService, WeatherService);
            BindingContext = MainViewModel;

            SetBindingContexts();

            _geoCoder = new Geocoder();

            }

      
        #region Events
        async void OnMapClicked(object sender, MapClickedEventArgs e)
        {
            map.Pins.Clear();
            var position = e.Position;
            var pin = CreatePin(position);
            map.Pins.Add(pin);

            Debug.WriteLine($"MapClick: {position.Latitude}, {position.Longitude}");

            lbl_longitude.Text = $"{position.Longitude}";
            lbl_latitude.Text = $"{position.Latitude}";

            IEnumerable<string> possibleAddresses = await _geoCoder.GetAddressesForPositionAsync(position);
            string address = possibleAddresses.FirstOrDefault();
            PoiViewModel.SetAddress(address);


            Debug.WriteLine("address: " + address);
        }
        private void ViewOptionButton_Clicked(object sender, EventArgs e)
        {
            Button button = sender as Button;
            switch (button.Text)
            {
                case "Street":
                    map.MapType = MapType.Street;
                    break;
                case "Satellite":
                    map.MapType = MapType.Satellite;
                    break;
                case "Hybrid":
                    map.MapType = MapType.Hybrid;
                    break;
            }
        }
        private async void SearchButton_Clicked(object sender, EventArgs e)
        {

            if (SearchField.Text == null)
                return;

            map.Pins.Clear();
            List<Position> positionList = await GetPositionsFromSearch(SearchField.Text);

            if (positionList.Count == 0)
            {
                await DialogService.ShowMessageAsync("Error", $"Could not find any location named \"{SearchField.Text}\"");
                return;
            }

            Location searchedLocation = await GetLocation(positionList);
            PoiViewModel.SetAddress(searchedLocation.Addresses.FirstOrDefault());
            var pin = CreatePin(searchedLocation);
            map.Pins.Add(pin);

            List<PointOfInterest> pois = await GetPoisFromDb(searchedLocation);

            if (pois == null || pois.Count == 0)
            {
                MoveToRegion(searchedLocation, 1);
                return;
            }

            foreach (var poi in pois)
            {
                CreatePin(poi);
                map.Pins.Add(pin);
            }

            MoveToRegion(searchedLocation, 1);
        }
        private void LblTemperature_BindingContextChanged(object sender, EventArgs e)
        {
            var lbl = (Label)sender;

            if (lbl.BindingContext == null)
                return;

            var temp = ((ForecastItem)lbl.BindingContext).Temperature;

            if (temp < 5)
                lbl.TextColor = Color.LightBlue;

            else if (temp > 5 && temp <= 15)
                lbl.TextColor = Color.LightGreen;

            else if (temp > 15 && temp <= 20)
                lbl.TextColor = Color.Orange;

            else if (temp > 20)
                lbl.TextColor = Color.Red;
        }
        #endregion

        #region Local methods
        private void SetBindingContexts()
        {
            slCreateUserViewModel.BindingContext = new NewUserViewModel(UserApiService, DialogService);
            slLogIn.BindingContext = new LoginViewModel(UserApiService, DialogService);
            slSettings.BindingContext = new SettingsViewModel();

            slPoiPopover.BindingContext = PoiViewModel;
            slPoiPopup.BindingContext = PoiViewModel;
            poiCollectionView.BindingContext = PoiViewModel;
            //poiCreationPopup.BindingContext = PoiViewModel;
            slPoiMenuButtons.BindingContext = PoiViewModel;
            weatherCollectionView.BindingContext = PoiViewModel;
        }
        private async Task<List<PointOfInterest>> GetPoisFromDb(Location searchedLocation)
        {
            var pois = new List<PointOfInterest>();
            try
            {
                pois = await PoiService.GetAsync(searchedLocation.Position);
            }
            catch (Exception ex)
            {
                await DialogService.ShowErrorAsync("Error", ex, "OK");
            }

            return pois;
        }
        private async Task<Location> GetLocation(List<Position> positions)
        {
            return new Location()
            {
                Position = positions.FirstOrDefault(),
                Addresses = await _geoCoder.GetAddressesForPositionAsync(positions.FirstOrDefault())
            };
        }
        private void MoveToRegion(Location searchedLocation, double distanceKm)
        {
            map.MoveToRegion(MapSpan.FromCenterAndRadius(searchedLocation.Position, Distance.FromKilometers(distanceKm)));
        }
        private void MoveToRegion(PointOfInterest poi, double distanceKm)
        {
            var position = new Position(poi.Position.Latitude, poi.Position.Longitude);
            map.MoveToRegion(MapSpan.FromCenterAndRadius(position, Distance.FromKilometers(distanceKm)));
        }
        private async Task<List<Position>> GetPositionsFromSearch(string search)
        {
            return new List<Position>(await _geoCoder.GetPositionsForAddressAsync(search));
        }
        private Pin CreatePin(Location location)
        {
            var pin = new Pin
            {
                Address = location.Addresses.First(),
                Label = location.Addresses.First(),
                Type = PinType.SearchResult,
                Position = location.Position
            };

            pin.MarkerClicked += (sender2, args) =>
            {
                DialogService.ShowMessageAsync("Tapped!", $"{pin.Label}");
            };

            return pin;
        }
        private Pin CreatePin(PointOfInterest poi)
        {
            var pin = new Pin
            {
                Address = poi.Description,
                Type = PinType.Place,
                Position = new Position(poi.Position.Latitude, poi.Position.Longitude),
                Label = poi.Name
            };

            pin.MarkerClicked += (sender2, args) =>
            {
                DialogService.ShowMessageAsync("Tapped!", $"{pin.Label}");
            };
            return pin;
        }
        private Pin CreatePin(Position position)
        {
            Pin pin = new Pin
            {

                Label = "",
                Address = "",
                Type = PinType.Generic,
                Position = new Position(position.Latitude, position.Longitude)
            };

            pin.MarkerClicked += (sender2, args) =>
            {
                DialogService.ShowMessageAsync("Tapped!", $"{pin.Label}");
            };
            return pin;
        }
        public void GoToLocation(PointOfInterest poi, double distanceKm)
        {
            MoveToRegion(poi, distanceKm);
            var pin = CreatePin(poi);
            map.Pins.Add(pin);
        }

        #endregion

        #region Slider methods
        public List<Frame> GetMenuPanels()
        {
            return new List<Frame>()
            {
                signupPopup,
                loginPopup,
                poiCollectionView,
                poiCreationView,
                weatherCollectionView,
                settingsPopup
            };
        }
        private void slider_MenuButtonClicked(object sender, EventArgs e)
        {
            var xnameofstack = sender as Button;
            List<Frame> MenuItems = GetMenuPanels();
            Frame frame = MenuItems.FirstOrDefault(x => x.ClassId == xnameofstack.ClassId);
            SlideInMenuPanel(frame);
        }
        private async void SlideInMenuPanel(Frame selectedMenuPanel)
        {
            double ScreenWidth = Application.Current.MainPage.Width;

            List<Frame> MenuItemFrames = GetMenuPanels();

            // Slidear ut den synliga menydelen
            foreach(var menuFrame in MenuItemFrames)
            {
                if(menuFrame.ClassId != selectedMenuPanel.ToString() && menuFrame.IsVisible == true)
                {
                    await menuFrame.TranslateTo(ScreenWidth * -1, 0, 300, Easing.SpringOut);
                    menuFrame.IsVisible = false;
                }
            }
            //Slidear in den valda menyn
            selectedMenuPanel.IsVisible = true;
            selectedMenuPanel.TranslationX = ScreenWidth;
            await selectedMenuPanel.TranslateTo(0, 0, 300, Easing.SpringIn);
            
        }
        
        async private void StartSlidePanel()
        {
            
            //switch (Device.RuntimePlatform)
            //{
            //    default: slider_navbar.TranslationY = slider_navbar.TranslationY = 435;
            //        //poiCreationPopup.TranslationY = poiCreationPopup.TranslationY = ScreenHeight;
            //        break;
            //}
        }
        
        async void SliderUpDown(object sender, System.EventArgs e)
            {

            double ScreenHeight = Application.Current.MainPage.Height;
            double ScreenWidth = Application.Current.MainPage.Width;
            //var initialPosition = mainStack.Height;
            //var currentPosition = body.Height;
            switch (Device.RuntimePlatform)
                {
                    case Device.Android:

                        if (slider_navbar.TranslationY == 0)
                        {
                            await slider_navbar.TranslateTo(0, ScreenHeight * -0.6, 500, Easing.SinInOut);
                            slider_menu.Margin = new Thickness(0, slider_navbar.Height);
                            slider_menu.IsVisible = true;
                            slider_menu.HeightRequest = ScreenHeight * 0.6;
                            slider_menu.WidthRequest = ScreenWidth;
                    }
                        else
                        {
                            await slider_navbar.TranslateTo(0, 0, 500, Easing.SpringIn);
                        }

                        break;
                    default:
                        if (slider_navbar.TranslationY == 0)
                        {
                        slider_menu.Margin = new Thickness(0, slider_navbar.Height);
                        slider_menu.IsVisible = true;
                        slider_menu.HeightRequest = ScreenHeight * 0.4;
                        slider_menu.WidthRequest = ScreenWidth;
                        await slider_navbar.TranslateTo(0, ScreenHeight * -0.4, 500, Easing.SinInOut);
                        }
                        else
                        {
                        await slider_navbar.TranslateTo(0, 0, 500, Easing.SpringIn);
                        }
                        break;
                }
            }
        
        #endregion
        // Maybe move this class outside, but it's only interesting here.
        private class Location
        {
            public Position Position { get; set; }
            public IEnumerable<string> Addresses { get; set; }
        }

        
    }
}