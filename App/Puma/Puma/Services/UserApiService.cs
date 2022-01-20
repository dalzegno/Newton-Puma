using Puma.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

using System.Net.Http.Json; //Requires nuget package System.Net.Http.Json
using System.Threading.Tasks;
using Puma.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(UserApiService))]
namespace Puma.Services
{
    public class UserApiService : IUserApiService
    {
        readonly HttpClient _httpClient = new HttpClient();
        readonly string _userApiUri = "http://localhost:64500/api/User";
        //readonly string _userApiUri = "http://localhost:44329/api/User";
        public EventHandler<string> ErrorMessage;

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

        public async Task<User> CreateUserAsync(User userToCreate)
        {

            var response = await _httpClient.PostAsJsonAsync(_userApiUri, userToCreate);

            if (!await IsResponseSuccess(response))
                return null;

            return await response.Content.ReadFromJsonAsync<User>();
        }

        public async Task<User> UpdateUserAsync(User userToUpdate)
        {
            var response = await _httpClient.PutAsJsonAsync(_userApiUri, userToUpdate);

            if (!await IsResponseSuccess(response))
                return null;

            return await response.Content.ReadFromJsonAsync<User>();
        }

        public Task<User> DeleteUserAsync(string email)
        {
            throw new NotImplementedException();
        }

        private async Task<bool> IsResponseSuccess(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                OnErrorMessage(responseBody);
                return false;
            }

            return true;
        }
    }
}
