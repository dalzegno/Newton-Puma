
using Puma.Helpers;
using Puma.Services;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(HttpResponseHelper))]
namespace Puma.Helpers
{
    internal class HttpResponseHelper
    {
        readonly IDialogService _dialogService = DependencyService.Get<IDialogService>();

        public async Task<bool> IsResponseSuccess(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
                return true;

            string responseBody = await response.Content.ReadAsStringAsync();
            await _dialogService.ShowErrorAsync($"{(int)response.StatusCode} - {response.StatusCode}: {responseBody}");
            return false;
        }

        public void SetHeader(HttpClient httpClient)
        {
            if (App.LoggedInUser == null)
                return;

            var header = httpClient.DefaultRequestHeaders.FirstOrDefault(a => a.Key == "apiKey");

            if (header.Value == null)
                httpClient.DefaultRequestHeaders.Add("apiKey", App.LoggedInUser.ApiKey);
        }
    }
}
