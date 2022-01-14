using System;
using Client.Services;
using Client.Views;
using Microsoft.Extensions.DependencyInjection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Puma
{
    public partial class App : Application
    {
        public IServiceProvider Container;
        public App()
        {
            InitializeComponent();
            Container = ConfigureDependencyInjection();
            MainPage = new MainPage();
        }
        IServiceProvider ConfigureDependencyInjection()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddScoped<UserApiService>();

            return serviceCollection.BuildServiceProvider();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
