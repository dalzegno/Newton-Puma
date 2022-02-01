using Puma.CustomRenderer;
using Puma.Models;
using Puma.Services;
using Puma.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;


namespace Puma.Views
{

    public partial class MainPage : ContentPage
    {
        IUserApiService UserApiService => DependencyService.Get<IUserApiService>();
        IDialogService DialogService => DependencyService.Get<IDialogService>();
        IPoiService PoiService => DependencyService.Get<IPoiService>();
        IOpenWeatherService WeatherService => DependencyService.Get<IOpenWeatherService>();

        private readonly IPoiService _poiService;

        public static MainPage Instance { get; set; }
        internal MainViewModel MainViewModel { get; }
        internal PoiViewModel poiViewModel { get; }

        Geocoder geoCoder;
        public MainPage()
        {
            InitializeComponent();
            Instance = this;
            MainViewModel = new MainViewModel(DialogService);
            // Implementing dependecy injection
            BindingContext = MainViewModel;

            slCreateUserViewModel.BindingContext = new NewUserViewModel(UserApiService, DialogService);
            slLogIn.BindingContext = new LoginViewModel(UserApiService, DialogService);

            poiViewModel = new PoiViewModel(PoiService, DialogService, WeatherService);
            slPoiPopover.BindingContext = poiViewModel;
            slPoiPopup.BindingContext = poiViewModel;
            poiCollectionView.BindingContext = poiViewModel;
            poiCreationPopup.BindingContext = poiViewModel;
            slPoiMenuButtons.BindingContext = poiViewModel;
            weatherCollectionView.BindingContext = poiViewModel;
            slSettings.BindingContext = new SettingsViewModel();

            geoCoder = new Geocoder();  

            _poiService = PoiService;

        }

        async void OnMapClicked(object sender, MapClickedEventArgs e)
        {
            map.Pins.Clear();
            Pin pin = new Pin
            {

                Label = "",
                Address = "",
                Type = PinType.Generic,
                Position = new Position(e.Position.Latitude, e.Position.Longitude)
            };
            map.Pins.Add(pin);
            System.Diagnostics.Debug.WriteLine($"MapClick: {e.Position.Latitude}, {e.Position.Longitude}");


            lbl_longitude.Text = $"{e.Position.Longitude}";
            lbl_latitude.Text = $"{e.Position.Latitude}";

            if (e.Position.Latitude != 0 && e.Position.Longitude != 0)
            {
                Position position = new Position(e.Position.Latitude, e.Position.Longitude);
                IEnumerable<string> possibleAddresses = await geoCoder.GetAddressesForPositionAsync(position);
                string address = possibleAddresses.FirstOrDefault();


                System.Diagnostics.Debug.WriteLine("address:" + address);

                poiViewModel.SetAddress(address);
            }
        }

        async void btn_SearchLocation_Clicked(object sender, EventArgs e)
        {

            var search = entry_address.Text + entry_zip.Text + entry_country.Text;

            List<Position> postionList = new List<Position>(await new Geocoder().GetPositionsForAddressAsync(search));

            map.Pins.Clear();
            if (postionList.Count != 0)
            {
                var position = postionList.FirstOrDefault<Position>();
                var adress = await new Geocoder().GetAddressesForPositionAsync(position);

                map.Pins.Add(new Pin
                {
                    Address = adress.First(),
                    Label = adress.First(),
                    Type = PinType.SearchResult,
                    Position = position
                });

                map.MoveToRegion(MapSpan.FromCenterAndRadius(position, Distance.FromKilometers(1)));

            }
        }

        //async void TestMap(object sender, MapClickedEventArgs e)
        //{
        //}


        void OnButtonClicked(object sender, EventArgs e)
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

        private async void Button_Clicked(object sender, System.EventArgs e)
        {
            if (SearchField.Text == null)
                return;


            List<Position> postionList = new List<Position>(await new Geocoder().GetPositionsForAddressAsync(SearchField.Text));

            map.Pins.Clear();

            if (postionList.Count == 0)
                return;

            var position = postionList.FirstOrDefault<Position>();
            var address = await new Geocoder().GetAddressesForPositionAsync(position);
            poiViewModel.SetAddress(address.First());
            var pois = new List<PointOfInterest>() { };
            try
            {
                pois = await _poiService.GetAsync(position);
            }
            catch (Exception ex) 
            {
                await DialogService.ShowErrorAsync("Error", ex, "OK");
            }

            if (pois == null || pois.Count == 0)
            {
                map.Pins.Add(new Pin
                {
                    Address = address.First(),
                    Label = address.First(),
                    Type = PinType.SearchResult,
                    Position = position
                });
                map.MoveToRegion(MapSpan.FromCenterAndRadius(position, Distance.FromKilometers(1)));
                return;
            }

            foreach (var poi in pois)
            {
                var pin = new Pin
                {
                    Address = poi.Description,
                    Type = PinType.Place,
                    Position = new Position(poi.Position.Latitude, poi.Position.Longitude),
                    Label = poi.Name
                };
                map.Pins.Add(pin);

                pin.MarkerClicked += (sender2, args) =>
                {
                    DisplayAlert("Tapped!", $"{pin.Label}", "OK");
                };
            }

            // Sökta positionen
            map.Pins.Add(new Pin
            {
                Address = address.First(),
                Label = address.First(),
                Type = PinType.SearchResult,
                Position = position
            });

            map.MoveToRegion(MapSpan.FromCenterAndRadius(position, Distance.FromKilometers(1)));
        }


        public void GoToLocation(PointOfInterest poi)
        {
            map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(poi.Position.Latitude, poi.Position.Longitude), Distance.FromKilometers(.5)));
            var pin = new Pin
            {
                Address = poi.Description,
                Type = PinType.Place,
                Position = new Position(poi.Position.Latitude, poi.Position.Longitude),
                Label = poi.Name
            };
            map.Pins.Add(pin);

            pin.MarkerClicked += (sender2, args) =>
            {

                DisplayAlert("Tapped!", $"{pin.Label}", "OK");
            };
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
    }
}
