﻿using Puma.CustomRenderer;
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
               

                //ClearPoiEntries();
                //FillAddressBoxes(address);
                poiViewModel.SetAddress(address);
            }

            void FillAddressBoxes(string address)
            {
                var words = address?.Split('\n') ?? Array.Empty<string>();
                if (words.Length == 2)
                {
                    poiViewModel.Area = words[0];
                    poiViewModel._country = words[1];
                    OnPropertyChanged(nameof(poiViewModel.Country));
                    //lbl_Area.Text = words[0];
                    //lbl_Country.Text = words[1];

                    //entry_zip.Text = words[0];
                    //entry_country.Text = words[1];
                }
                else if (words.Length == 3)
                {
                    poiViewModel.StreetName = words[0];
                    poiViewModel.Area = words[1];
                    poiViewModel._country = words[2];
                    OnPropertyChanged(nameof(poiViewModel.Country));
                    //lbl_StreetName.Text = words[0];
                    //lbl_Area.Text = words[1];
                    //lbl_Country.Text = words[2];

                    //entry_address.Text = words[0];
                    //entry_zip.Text = words[1];
                    //entry_country.Text = words[2];
                }
                //lbl_adress.Text = address;
            }

            void ClearPoiEntries()
            {
                //lbl_StreetName.Text = "";
                //lbl_Area.Text = "";
                //lbl_Country.Text = "";

                //entry_address.Text = "";
                //entry_zip.Text = "";
                //entry_country.Text = "";
            }

            //Circle circle = new Circle
            //{
            //    Center = e.Position,
            //    Radius = new Distance(250),
            //    StrokeColor = Color.FromHex("#88FF0000"),
            //    StrokeWidth = 8,
            //    FillColor = Color.FromHex("#88FFC0CB")
            //};
            //map.MapElements.Add(circle);


            //Tag Button Generator
            //var myList = new List<string>(); //Replace string with tags event
            //TagsList.Children.Clear(); //just in case so you can call this code several times np..
            //foreach (var item in myList)
            //{
            //    var btn = new Button()
            //    {
            //        Text = item.id, //Whatever prop you wonna put as title;
            //        StyleId = item.name //use a property from event as id to be passed to handler
            //        };
            //    btn.Clicked += OnDynamicBtnClicked;
            //    TagsList.Children.Add(btn);
            //}

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

            
            //var poiService = new PoiApiService();

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
            catch (Exception ex) { }

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
