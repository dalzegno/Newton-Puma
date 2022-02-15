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
using Xamarin.Essentials;
using System.Threading;

namespace Puma.Views
{

    public partial class MainPage : ContentPage
    {
        private readonly Geocoder _geoCoder;

        public static MainPage Instance { get; set; }
        internal MainViewModel MainViewModel { get; }
        internal PoiViewModel PoiViewModel { get; }
        internal SettingsViewModel SettingsViewModel { get; }
        internal WeatherViewModel WeatherViewModel1 { get; }
        internal NewUserViewModel NewUserViewModel { get; }
        internal LoginViewModel LoginViewModel { get; }
       
        IUserApiService UserApiService => DependencyService.Get<IUserApiService>();
        IDialogService DialogService => DependencyService.Get<IDialogService>();
        IPoiService PoiService => DependencyService.Get<IPoiService>();
        IOpenWeatherService WeatherService => DependencyService.Get<IOpenWeatherService>();

        public MainPage()
        {
            InitializeComponent();
            Instance = this;
            // Internal viewmodels that can be reached globally
            LoginViewModel = new LoginViewModel(UserApiService, DialogService);
            MainViewModel = new MainViewModel(DialogService);
            PoiViewModel = new PoiViewModel(PoiService, DialogService);
            WeatherViewModel1 = new WeatherViewModel(WeatherService, DialogService);
            SettingsViewModel = new SettingsViewModel(UserApiService, DialogService);
            NewUserViewModel = new NewUserViewModel(UserApiService, DialogService);
            BindingContext = MainViewModel;

            SetBindingContexts();

            _geoCoder = new Geocoder();

            GetCurrentLocation();
        }

        #region Events
        async void OnMapClicked(object sender, MapClickedEventArgs e)
        {
            map.IsShowingUser = false;

            map.Pins.Clear();
            var position = e.Position;
            var pin = CreatePin(position);
            AddPin(pin);

            Debug.WriteLine($"MapClick: {position.Latitude}, {position.Longitude}");

            lbl_longitude.Text = $"{position.Longitude}";
            lbl_latitude.Text = $"{position.Latitude}";

            IEnumerable<string> possibleAddresses = await _geoCoder.GetAddressesForPositionAsync(position);
            string address = possibleAddresses.FirstOrDefault();

            PoiViewModel.SetAddress(address);
            await PoiViewModel.SetLatAndLon(position.Latitude, position.Longitude);
            WeatherViewModel1.SetWeather(position.Latitude, position.Longitude);

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

            map.IsShowingUser = false;
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
            MoveToRegion(searchedLocation, 1);

            PoiViewModel.SetAddress(searchedLocation.Addresses.FirstOrDefault());
            await PoiViewModel.SetLatAndLon(searchedLocation.Position.Latitude, searchedLocation.Position.Longitude);
            WeatherViewModel1.SetWeather(searchedLocation.Position.Latitude, searchedLocation.Position.Longitude);

            List<PointOfInterest> pois = await GetPoisFromDb(searchedLocation);

            if (pois == null || pois.Count == 0)
                return;

            foreach (var poi in pois)
            {
                CreatePin(poi);
                map.Pins.Add(pin);
            }
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
        private void btn_OpenSettings(object sender, EventArgs e)
        {
            var user = App.LoggedInUser;
            if (user == null)
                return;

            SettingsViewModel.SetUserToEdit(user.DisplayName, user.Email, user.FirstName, user.LastName, user.Password);
        }
        #endregion

        #region Slider methods
        public List<Frame> GetMenuPanels()
        {
            return new List<Frame>()
            {
                poiCollectionFrame,
                poiCreationView,
                weatherCollectionView
            };
        }
        public async void SlideInMenuPanel(Frame selectedMenuPanel)
        {
            double ScreenWidth = Application.Current.MainPage.Width;
            double ScreenHeight = Application.Current.MainPage.Height;

            List<Frame> MenuItemFrames = GetMenuPanels();

            if (selectedMenuPanel != null)
            {
                // Slidear ut den synliga menydelen
                foreach (var menuFrame in MenuItemFrames)
                {
                    if (menuFrame.ClassId != selectedMenuPanel.ToString() && menuFrame.IsVisible == true)
                    {
                        //slide to left
                        //await menuFrame.TranslateTo(-ScreenWidth, 0, 300, Easing.SpringOut);

                        //slide down
                        await menuFrame.TranslateTo(0, ScreenHeight, 300, Easing.SpringOut);
                        menuFrame.IsVisible = false;
                    }
                }
                //Slidear in den valda menyn
                selectedMenuPanel.IsVisible = true;
                //slide left to right
                //selectedMenuPanel.TranslationX = -ScreenWidth;
                //slide up
                selectedMenuPanel.TranslationY = ScreenHeight;
                await selectedMenuPanel.TranslateTo(0, 0, 300, Easing.SpringIn);
            }
        }
        private async void slider_MenuButtonClicked(object sender, EventArgs e)
        {
            var xnameofstack = sender as Button;
            List<Frame> MenuItems = GetMenuPanels();
            Frame frame = MenuItems.FirstOrDefault(x => x.ClassId == xnameofstack.ClassId);

            if (xnameofstack.ClassId == "poiCollectionFrame")
                await PoiViewModel.SetLatAndLon(Convert.ToDouble(PoiViewModel.Latitude), Convert.ToDouble(PoiViewModel.Longitude));

            SlideInMenuPanel(frame);
        }
        //Adapt navbar slideout menu to screen size
        async void SliderUpDown(object sender, EventArgs e)
        {

            double ScreenHeight = Application.Current.MainPage.Height;
            double ScreenWidth = Application.Current.MainPage.Width;
            switch (Device.RuntimePlatform)
            {
                case Device.Android:

                    if (slider_navbar.TranslationY == 0)
                    {
                        await AdaptNavbarSliderToScreenSize(ScreenHeight, ScreenWidth, -0.6);
                    }
                    else
                    {
                        await slider_navbar.TranslateTo(0, 0, 500, Easing.SpringIn);
                    }

                    break;
                default:
                    if (slider_navbar.TranslationX == 0)
                    {
                        globeIcon.RotateTo(180, 400, Easing.Linear);
                        await AdaptNavbarSliderToScreenSize(ScreenHeight, ScreenWidth, 0.3);
                    }
                    else
                    {
                        globeIcon.RotateTo(360, 400, Easing.Linear);
                        await slider_navbar.TranslateTo(0, 0, 500, Easing.CubicInOut);
                        globeIcon.Rotation = 0;
                    }
                    break;
            }
        }

        private async Task AdaptNavbarSliderToScreenSize(double screenHeight, double screenWidth, double sliderValue)
        {
            slider_menu.TranslationX = -screenWidth * sliderValue;
            //slider_menu.Margin = new Thickness(-slider_navbar.Width * 0.6 + (screenWidth * sliderValue), 0);
            slider_menu.IsVisible = true;
            slider_menu.HeightRequest = screenHeight;
            slider_menu.WidthRequest = screenWidth * sliderValue;
            await slider_navbar.TranslateTo(screenWidth * sliderValue, 0, 500, Easing.CubicInOut);
        }
        async void SliderDown(object sender, EventArgs e)
        {
            await slider_navbar.TranslateTo(0, 0, 500, Easing.SpringIn);
        }


        #endregion

        #region Local methods
        private void SetBindingContexts()
        {
            slCreateUserViewModel.BindingContext = NewUserViewModel;
            slLogIn.BindingContext = new LoginViewModel(UserApiService, DialogService);
            slPoiPopover.BindingContext = PoiViewModel;
            slPoiPopup.BindingContext = PoiViewModel;
            poiCollectionFrame.BindingContext = PoiViewModel;
            poiCreationView.BindingContext = PoiViewModel;
            slPoiMenuButtons.BindingContext = PoiViewModel;
            weatherCollectionView.BindingContext = WeatherViewModel1;
            settingsInputs.BindingContext = SettingsViewModel;

            sl_Weather.BindingContext = WeatherViewModel1;
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
                await DialogService.ShowErrorAsync(ex);
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

        private void MoveToRegion(Pin pin, double distanceKm)
        {
            var position = new Position(pin.Position.Latitude, pin.Position.Longitude);
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
                Label= location.Addresses.First(),
                Type = PinType.SearchResult,
                Position = location.Position
            };

            pin.MarkerClicked += (sender2, args) =>
            {
                DialogService.ShowMessageAsync("Tapped!", $"{pin.Label}");
            };

            return pin;
        }
        public Pin CreatePin(PointOfInterest poi)
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
                //DialogService.ShowMessageAsync("Tapped!", $"{pin.Label Style="{StaticResource labelStyle}"}");
                SliderUpDown(null, null);
                PoiViewModel.SelectedSinglePoi = poi;
                PoiViewModel.PoiSinglePopup();
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
        public void AddPin(Pin pin)
        {
            map.Pins.Add(pin);
        }
        public void GoToLocation(PointOfInterest poi, double distanceKm)
        {
            MoveToRegion(poi, distanceKm);
            var pin = CreatePin(poi);
            map.Pins.Add(pin);
        }

        #endregion
        // Maybe move this class outside, but it's only interesting here.
        private class Location
        {
            public Position Position { get; set; }
            public IEnumerable<string> Addresses { get; set; }
        }



        #region Currents location
        CancellationTokenSource cts;

        async void GetCurrentLocation()
        {
            //var checkStatus = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            //if (checkStatus != PermissionStatus.Granted) 
            //{
            //    await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            //}
           
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                cts = new CancellationTokenSource();
                var location = await Geolocation.GetLocationAsync(request, cts.Token);

                if (location != null)
                {

                    map.Pins.Clear();
                    var position = new Position(location.Latitude, location.Longitude);
                    map.MoveToRegion(MapSpan.FromCenterAndRadius(position, Distance.FromKilometers(1)));
                    var pin = CreatePin(position);
                    map.Pins.Add(pin);

                    IEnumerable<string> possibleAddresses = await _geoCoder.GetAddressesForPositionAsync(position);
                    string address = possibleAddresses.FirstOrDefault();

                    

                    PoiViewModel.SetAddress(address);
                    await PoiViewModel.SetLatAndLon(position.Latitude, position.Longitude);
                    WeatherViewModel1.SetWeather(position.Latitude, position.Longitude);

                   // map.IsShowingUser = true;
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
                await DisplayAlert("Alert", "This device do not have GPS ", "Ok");

            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception

            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
                await DisplayAlert("Alert", "permission denied", "Ok");
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
        }

        protected override void OnDisappearing()
        {
            if (cts != null && !cts.IsCancellationRequested)
                cts.Cancel();
            base.OnDisappearing();
        }
        #endregion

        private void MyPositionButton_Clicked(object sender, EventArgs e)
        {
            GetCurrentLocation();
        }
    }
}