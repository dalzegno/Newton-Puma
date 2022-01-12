using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Puma
{
    public partial class MainPage : ContentPage
    {
       
        public MainPage()
        {
            
            InitializeComponent();
        }
        void OnMapClicked(object sender, MapClickedEventArgs e)
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
        }
    }
}
