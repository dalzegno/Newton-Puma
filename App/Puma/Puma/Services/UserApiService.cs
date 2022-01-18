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
        readonly HttpClient _HttpClient = new HttpClient();
        readonly string _userApiUri = "http://localhost:64500/api/User";
        //readonly string _userApiUri = "http://localhost:44329/api/User";
        public EventHandler<string> ErrorMessage;

        protected virtual void OnErrorMessage(string e) => ErrorMessage?.Invoke(this, e);

        public async Task<UserDto> LogIn(string email, string password)
        {
            var response = await _HttpClient.GetAsync($"{_userApiUri}/LogIn?email={email}&password={password}");
            if (!await IsResponseSuccess(response))
                return null;

            return await response.Content.ReadFromJsonAsync<UserDto>();
        }

        public async Task<UserDto> GetUserAsync(string email)
        {
            var response = await _HttpClient.GetAsync($"{_userApiUri}/GetUserByEmail?email={email}");

            if (!await IsResponseSuccess(response))
                return null;

            return await response.Content.ReadFromJsonAsync<UserDto>();

        }

        public async Task<UserDto> CreateUserAsync(UserDto userToCreate)
        {

            var response = await _HttpClient.PostAsJsonAsync(_userApiUri, userToCreate);

            if (!await IsResponseSuccess(response))
                return null;

            return await response.Content.ReadFromJsonAsync<UserDto>();
        }

        public async Task<UserDto> UpdateUserAsync(UserDto userToUpdate)
        {
            var response = await _HttpClient.PutAsJsonAsync(_userApiUri, userToUpdate);

            if (!await IsResponseSuccess(response))
                return null;

            return await response.Content.ReadFromJsonAsync<UserDto>();
        }

        public Task<UserDto> DeleteUserAsync(string email)
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
