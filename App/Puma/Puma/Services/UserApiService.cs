using Puma.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

using System.Net.Http.Json; //Requires nuget package System.Net.Http.Json
using System.Threading.Tasks;
using Puma.Services;
using Xamarin.Forms;
using System.Linq;

[assembly: Dependency(typeof(UserApiService))]
namespace Puma.Services
{
    public class UserApiService : IUserApiService
    {
        IDialogService DialogService => DependencyService.Get<IDialogService>();
        readonly HttpClient _httpClient = new HttpClient();
        readonly string _userApiUri = "http://localhost:64500/api/User";
        //readonly string _userApiUri = "http://localhost:44329/api/User";
        public EventHandler<string> ErrorMessage;
        public User CurrentUser;
        protected virtual void OnErrorMessage(string e) => ErrorMessage?.Invoke(this, e);

        public async Task<User> LogIn(string email, string password)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_userApiUri}/LogIn?email={email}&password={password}");
   
                if (!await IsResponseSuccess(response))
                    return null;

                return await response.Content.ReadFromJsonAsync<User>();
            }
            catch (Exception e)
            {
                // TODO: Error handling
                // TODO: Borde injecta DialogService på något sätt här istället
                await  App.Current.MainPage.DisplayAlert("Error",  e.Message,"Ok");
                return null;
            }

        }

        public async Task<User> GetUserAsync(string email)
        {
            SetHeader();

            try
            {
                var response = await _httpClient.GetAsync($"{_userApiUri}/GetUserByEmail?email={email}");
                if (!await IsResponseSuccess(response))
                    return null;

                return await response.Content.ReadFromJsonAsync<User>();

            }
            catch (Exception)
            {
                return null;
            }

        }

        public async Task<User> CreateUserAsync(AddUserDto userToCreate)
        {
            var response = await _httpClient.PostAsJsonAsync(_userApiUri, userToCreate);

            if (!await IsResponseSuccess(response))
                return null;

            return await response.Content.ReadFromJsonAsync<User>();
        }

        public async Task<User> UpdateUserAsync(UpdateUserDto userToUpdate)
        {
            SetHeader();

            var response = await _httpClient.PutAsJsonAsync(_userApiUri, userToUpdate);

            if (!await IsResponseSuccess(response))
                return null;

            return await response.Content.ReadFromJsonAsync<User>();
        }

        public async Task<User> DeleteUserAsync(int id)
        {
            SetHeader();

            var response = await _httpClient.DeleteAsync($"{_userApiUri}?id={id}");

            if (!await IsResponseSuccess(response))
                return null;

            return await response.Content.ReadFromJsonAsync<User>();
        }

        private async Task<bool> IsResponseSuccess(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                await DialogService.ShowErrorAsync("Error!", $"{response.StatusCode}: {responseBody}", "OK");
                return false;
            }

            return true;
        }
        public async Task<User> GetCurrentUserAsync(string email, User currentUser)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_userApiUri}/GetUserByEmail?email={email}");
                currentUser = await response.Content.ReadFromJsonAsync<User>();
                CurrentUser = currentUser;
                if (!await IsResponseSuccess(response))
                    return null;

                return await response.Content.ReadFromJsonAsync<User>();

            }
            catch (Exception)
            {
                return null;
            }

        }

        private void SetHeader()
        {
            if (App.LoggedInUser == null)
                return;

            var header = _httpClient.DefaultRequestHeaders.FirstOrDefault(a => a.Key == "apiKey");

            if (header.Value == null)
                _httpClient.DefaultRequestHeaders.Add("apiKey", App.LoggedInUser.ApiKey);
        }
    }
}
