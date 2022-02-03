
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

using Puma.Extensions;
using Puma.Services;

[assembly: Dependency(typeof(HttpExtension))]
namespace Puma.Extensions
{
    internal static class HttpExtension
    {
        public static async Task<bool> IsResponseSuccessAsync(this HttpResponseMessage response, IDialogService _dialogService = null)
        {
            if (response.IsSuccessStatusCode)
                return true;

            string responseBody = await response.Content.ReadAsStringAsync();

            await _dialogService?.ShowErrorAsync($"{(int)response.StatusCode} - {response.StatusCode}: {responseBody}");

            return false;
        }

        public static void SetHeader(this HttpClient httpClient)
        {
            if (App.LoggedInUser == null)
                return;

            var header = httpClient.DefaultRequestHeaders.FirstOrDefault(a => a.Key == "apiKey");

            if (header.Value == null)
                httpClient.DefaultRequestHeaders.Add("apiKey", App.LoggedInUser.ApiKey);
        }
    }
}
