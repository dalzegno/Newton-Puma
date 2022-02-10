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
                default:
                    App.Current.UserAppTheme = OSAppTheme.Light;
                    break;
                //DarkMode
                case 0:
                    App.Current.UserAppTheme = OSAppTheme.Dark;
                    break;
            }
        }
    }
}
