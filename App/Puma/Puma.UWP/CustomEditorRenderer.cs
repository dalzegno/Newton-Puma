using Puma.CustomRenderer;
using Puma.UWP;
using Windows.UI;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;
using SolidColorBrush = Windows.UI.Xaml.Media.SolidColorBrush;
using CornerRadius = Windows.UI.Xaml.CornerRadius;

[assembly: ExportRenderer(typeof(CustomEditor), typeof(CustomEditorRenderer))]
namespace Puma.UWP
{
    public class CustomEditorRenderer : EditorRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                //Control.BorderBrush = new SolidColorBrush(Colors.Transparent);
                Control.BorderThickness = new Windows.UI.Xaml.Thickness(1);
                Control.BorderBrush = new SolidColorBrush(Colors.Gray);
                Control.CornerRadius = new CornerRadius(10);
                //Control.CornerRadius = new CornerRadius(10);
                //Control.Background = new SolidColorBrush(Colors.Green);
                //Control.BackgroundFocusBrush = new SolidColorBrush(Colors.LightBlue);
            }
        }
    }
}
