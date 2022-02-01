using System;
using Puma.Services;
using Puma.Views;
using Puma.CustomRenderer;
using Microsoft.Extensions.DependencyInjection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using Puma.Models;

namespace Puma
{
    public partial class App : Application
    {
        public static User LoggedInUser { get; set; }
        public App()
        {

            InitializeComponent();
            MainPage = new MainPage();
        }
      
        protected override void OnStart()
        {
            OnResume();
        }

        protected override void OnSleep()
        {
            Theme.SetTheme();
            RequestedThemeChanged -= App_RequestedThemeChanged;
        }

        protected override void OnResume()
        {
            Theme.SetTheme();
            RequestedThemeChanged += App_RequestedThemeChanged;
        }

        private void App_RequestedThemeChanged(object sender, AppThemeChangedEventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Theme.SetTheme();
            });
        }
    }
}
