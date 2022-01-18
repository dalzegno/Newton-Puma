using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Client.Models;
using Client.Services;
using Client.Views;
using Client.CustomRenderer;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Client.ViewModels
{
    class PopUp
    {
       public static void Pinmethod(Map map, MapClickedEventArgs e) {
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
            }

        public static void PublicVisibilityforTwo(Frame Visible1, Frame Invisible1)
        {
            Visible1.IsVisible = true;
            Invisible1.IsVisible = false;
        }

        public async static void RegisterUser(RoundedEntry email, RoundedEntry password, RoundedEntry displayname, RoundedEntry firstname, RoundedEntry surname, UserApiService _userApiService)
        {
            var user = new UserDto()
            {
                Email = email?.Text,
                Password = password?.Text,
                DisplayName = displayname?.Text,
                FirstName = firstname?.Text ?? "",
                LastName = surname?.Text ?? ""
            };


            var createdUser = await _userApiService.CreateUserAsync(user);
            if (createdUser != null)
                System.Diagnostics.Debug.WriteLine($"User created!: {createdUser.FirstName}, User lastname: {createdUser.LastName}");
        }
    }
}
