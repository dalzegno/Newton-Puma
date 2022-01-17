using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Client.Services
{
    public class DialogService : IDialogService
    {

        public async Task ShowErrorAsync(string title, string message, string buttonText)
        {
            await Application.Current.MainPage.DisplayAlert(message, title, buttonText);
        }

        public async Task ShowErrorAsync(string title, Exception error, string buttonText)
        {
            await Application.Current.MainPage.DisplayAlert(title, error.Message, buttonText);
        }

        public async Task ShowMessageAsync(string title, string message)
        {
            await Application.Current.MainPage.DisplayAlert(title, message, "OK");
        }

        public async Task ShowMessageAsync(string title, string message, string buttonText)
        {
            await Application.Current.MainPage.DisplayAlert(title, message, buttonText);
        }
    }
}
