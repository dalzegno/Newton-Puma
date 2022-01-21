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
        IPoiService PoiService => DependencyService.Get<IPoiService>();
        IDialogService DialogService => DependencyService.Get<IDialogService>();

        Geocoder geoCoder;
        public MainPage()
        {
            InitializeComponent();

            // Implementing dependecy injection
            BindingContext = new MainViewModel(UserApiService);
            
            slCreateUserViewModel.BindingContext = new NewUserViewModel(UserApiService, DialogService);
            slLogIn.BindingContext = new LoginViewModel(UserApiService, DialogService);

            slSettings.BindingContext = new SettingsViewModel();

            geoCoder = new Geocoder();
            
        }
        
        async void OnMapClicked(object sender, MapClickedEventArgs e)
        {
            var poiService = new PoiApiService();
            var tags = await poiService.GetTags();
            List<string>tagList = new List<string>();
            foreach (var t in tags)
                 tagList.Add(t.Name);
            TagsList.ItemsSource = tagList;

          

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


            lbl_longitude.Text = $"longitude: {e.Position.Longitude}";
            lbl_latitude.Text = $"latitude: {e.Position.Latitude}";

            if (e.Position.Latitude != null && e.Position.Longitude != null)
            {
                Position position = new Position(e.Position.Latitude, e.Position.Longitude);
                IEnumerable<string> possibleAddresses = await geoCoder.GetAddressesForPositionAsync(position);
                string address = possibleAddresses.FirstOrDefault();


                System.Diagnostics.Debug.WriteLine("address:" + address);
                var words = address?.Split('\n') ?? Array.Empty<string>();
                foreach (var word in words)
                    System.Diagnostics.Debug.WriteLine("w" + word);

                ClearPoiEntries();
                if (words.Length == 2)
                {
                    entry_zip.Text = words[0];
                    entry_country.Text = words[1];
                }
                else if(words.Length == 3)
                {
                    entry_address.Text = words[0];
                    entry_zip.Text = words[1];
                    entry_country.Text = words[2];
                }
                else
                {

                }
                lbl_adress.Text = address;
            }

            void ClearPoiEntries()
            {
                entry_address.Text = "";
                entry_zip.Text = "";
                entry_country.Text = "";
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

        async void TestMap(object sender, MapClickedEventArgs e)
        {

        }

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
            List<Position> postionList = new List<Position>(await new Geocoder().GetPositionsForAddressAsync(SearchField.Text));

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

                // Om ni vill testa poiService :) 
                //    var poiService = new PoiApiService();


                //var response = await poiService.CreatePoiAsync(new AddPoiDto
                //{
                //    Name = "Test test",
                //    Description = "Yo description",
                //    Position = new PositionPoi
                //    {
                //        Latitude = 59.37787,
                //        Longitude = 17.02502
                //    },
                //    Address = new Address
                //    {
                //        Country = "Sverige",
                //        City = "Test",
                //        StreetName = "Cool gata",
                //        ZipCode = "Zipcode"
                //    },
                //    TagIds = new List<int> { 1, 2 }
                //});
            }
        }

        private void TagsList_SelectedIndexChanged(object sender, EventArgs e)
        {
              
        }
    }
}
