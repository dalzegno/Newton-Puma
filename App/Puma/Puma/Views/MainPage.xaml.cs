using Puma.CustomRenderer;
using Puma.Services;
using Puma.ViewModels;
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
