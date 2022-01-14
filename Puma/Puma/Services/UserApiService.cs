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

        public async Task<List<UserDto>> GetUsersAsync()
        {
            var response = await _httpClient.GetAsync($"{_userApiUri}/GetAllUsers");

            if (!response.IsSuccessStatusCode)
            {
                System.Diagnostics.Debug.WriteLine($"Error: {response}");
                OnErrorMessage($"Could not get users. ResponseCode: {response.StatusCode}, message: {response}");
                return null;
            }

            return await response.Content.ReadFromJsonAsync<List<UserDto>>();
        }

        public async Task<UserDto> CreateUserAsync(UserDto userToCreate)
        {
            var userContent = JsonContent.Create(userToCreate);
            var response = await _httpClient.PostAsync(_userApiUri, userContent);

            if (!response.IsSuccessStatusCode)
            {
                OnErrorMessage($"Create user failed. Responsecode: {response.StatusCode}");
                return null;
            }

            return await response.Content.ReadFromJsonAsync<UserDto>();
        }

    }
}
