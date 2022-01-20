using Puma.CustomRenderer;
using Puma.Models;
using Puma.Services;
using Puma.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Puma.Views
{
    public partial class MainPage : ContentPage
    {
        IUserApiService UserApiService => DependencyService.Get<IUserApiService>();
        IPoiService PoiService => DependencyService.Get<IPoiService>();
        IDialogService DialogService => DependencyService.Get<IDialogService>();

        public MainPage()
        {
            InitializeComponent();

            // Implementing dependecy injection
            BindingContext = new MainViewModel(UserApiService);
            slCreateUserViewModel.BindingContext = new NewUserViewModel(UserApiService, DialogService);
            slLogIn.BindingContext = new LoginViewModel(UserApiService, DialogService);
            slSettings.BindingContext = new SettingsViewModel();
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
    }
}
