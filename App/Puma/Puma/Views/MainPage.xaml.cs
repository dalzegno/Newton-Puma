using Puma.CustomRenderer;
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

            //Button Generator
            var myList = new List<string>(); //replace string with peters event
            TagsList.Children.Clear(); //Just in case it needs to be cleared..
            foreach (var item in myList)
            {
                var btn = new Button()
                {
                    Text = item.Name, //Tag Name;
                    StyleId = item.Id //Id to be passed to handler
                };

                btn.Clicked += OnDynamicBtnClicked;
                TagsList.Children.Add(btn);
            }
        }
        public void OnDynamicBtnClicked(object sender, EventArgs e)
        {
            var dynamicbtn = sender as Button; //Sender contains Text+StyleId
            var myId = dynamicbtn.StyleId; //this was set during dynamic creation

            if (dynamicbtn.TextColor == Color.Azure)
            {
                //Add Id to the list of Tags for PoI
                dynamicbtn.TextColor = Color.LightBlue;
            }
            else
            {
                //Remove Id from the list of Tags for PoI
                dynamicbtn.TextColor = Color.Azure;
            }
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
            }
        }
    }
}
