using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Puma;
using Puma.CustomRenderer;
using Puma.UWP;
using Windows.UI;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;
using SolidColorBrush = Windows.UI.Xaml.Media.SolidColorBrush;
using CornerRadius = Windows.UI.Xaml.CornerRadius;

[assembly: ExportRenderer(typeof(RoundedEntry), typeof(RoundedEntryRenderer))]
namespace Puma.UWP
{
    class RoundedEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            
            if (Control != null)
            {
                Control.BorderBrush = new SolidColorBrush(Colors.Transparent);
                Control.BorderThickness = new Windows.UI.Xaml.Thickness(0);
                Control.CornerRadius = new CornerRadius(10);
                //Control.CornerRadius = new CornerRadius(10);
                //Control.Background = new SolidColorBrush(Colors.Green);
                //Control.BackgroundFocusBrush = new SolidColorBrush(Colors.LightBlue);
            }
        }
    }
}
