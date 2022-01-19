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

        Geocoder geoCoder;

        public MainPage()
        {
            InitializeComponent();

            // Implementing dependecy injection
            BindingContext = new MainViewModel(UserApiService);
            
            slCreateUserViewModel.BindingContext = new NewUserViewModel(UserApiService, DialogService);
            slLogIn.BindingContext = new LoginViewModel(UserApiService, DialogService);

            geoCoder = new Geocoder();
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
    }
}
