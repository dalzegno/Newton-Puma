using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Puma.CustomRenderer
{
    public static class Theme
    {
        public static void SetTheme()
        {
            switch(ThemeSettings.Theme)
            {
                //LightMode
                case 0:
                    App.Current.UserAppTheme = OSAppTheme.Light;
                    break;
                //DarkMode
                case 1:
                    App.Current.UserAppTheme = OSAppTheme.Dark;
                    break;
                default:
                    App.Current.UserAppTheme = OSAppTheme.Unspecified;
                    break;
            }
        }
    }
}
