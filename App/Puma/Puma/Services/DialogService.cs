using Puma.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(DialogService))]
namespace Puma.Services
{
    public class DialogService : IDialogService
    {
        public async Task ShowErrorAsync(string message)
        {
            await Application.Current.MainPage.DisplayAlert("Error", message, "Ok");
        }

        public async Task ShowErrorAsync(Exception error)
        {
            await Application.Current.MainPage.DisplayAlert("Error", error.Message, "Ok");
        }

        public async Task ShowMessageAsync(string title, string message)
        {
            await Application.Current.MainPage.DisplayAlert(title, message, "OK");
        }

        public async Task ShowMessageAsync(string title, string message, string buttonText)
        {
            await Application.Current.MainPage.DisplayAlert(title, message, buttonText);
        }

        public async Task<string> ShowYesNoActionSheet(string title)
        {
            return await Application.Current.MainPage.DisplayActionSheet(title, "No", "Yes");
        }
    }
}
