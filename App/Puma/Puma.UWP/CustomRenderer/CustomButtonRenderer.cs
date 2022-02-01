using Puma.UWP.CustomRenderer;
using Puma.CustomRenderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Xamarin.Forms.Platform.UWP;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(CustomButton), typeof(CustomButtonRenderer))]
namespace Puma.UWP.CustomRenderer
{
    public class CustomButtonRenderer : ButtonRenderer
    {
        private readonly Windows.UI.Core.CoreCursor OrigHandCursor = Window.Current.CoreWindow.PointerCursor;

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);

                if (e.OldElement == null)
            {
                Control.PointerExited += Control_PointerExited;
                Control.PointerMoved += Control_PointerMoved;
            }
        }

        private void Control_PointerMoved(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            Windows.UI.Core.CoreCursor handCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Hand, 1);
            if (handCursor != null)
                Window.Current.CoreWindow.PointerCursor = handCursor;
        }

        private void Control_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (OrigHandCursor != null)
                Window.Current.CoreWindow.PointerCursor = OrigHandCursor;
        }
    }
}
