using Client.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

using System.Net.Http.Json; //Requires nuget package System.Net.Http.Json
using System.Threading.Tasks;

namespace Client.Services
{
    public class UserApiService
    {
        readonly HttpClient _httpClient = new HttpClient();
        readonly string _userApiUri = "http://localhost:64500/api/User";
        public EventHandler<string> ErrorMessage;
        protected virtual void OnErrorMessage(string e) => ErrorMessage?.Invoke(this, e);

        public async Task<UserDto> LogIn(string email, string password)
        {
            var response = await _httpClient.GetAsync($"{_userApiUri}/LogIn?email={email}&password={password}");
            if (!await IsResponseSuccess(response))
                return null;

            return await response.Content.ReadFromJsonAsync<UserDto>();
        }

        public async Task<UserDto> GetUserAsync(string email)
        {
            var response = await _httpClient.GetAsync($"{_userApiUri}/GetUserByEmail?email={email}");

            if (!await IsResponseSuccess(response))
                return null;

            return await response.Content.ReadFromJsonAsync<UserDto>();

        }

        public async Task<UserDto> CreateUserAsync(UserDto userToCreate)
        {

            var response = await _httpClient.PostAsJsonAsync(_userApiUri, userToCreate);

            if (!await IsResponseSuccess(response))
                return null;

            return await response.Content.ReadFromJsonAsync<UserDto>();
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
