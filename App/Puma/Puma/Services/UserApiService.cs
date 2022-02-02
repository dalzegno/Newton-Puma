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
using Puma.Helpers;

[assembly: Dependency(typeof(UserApiService))]
namespace Puma.Services
{
    public class UserApiService : IUserApiService
    {
        readonly IDialogService _dialogService = DependencyService.Get<IDialogService>();
        readonly HttpResponseHelper _httpResponseHelper = DependencyService.Get<HttpResponseHelper>();
        readonly HttpClient _httpClient = new HttpClient();
        readonly string _userApiUri = "http://localhost:64500/api/User";
        public User CurrentUser;

        public async Task<User> LogIn(string email, string password)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_userApiUri}/LogIn?email={email}&password={password}");

                if (!await _httpResponseHelper.IsResponseSuccess(response))
                    return null;

                return await response.Content.ReadFromJsonAsync<User>();
            }
            catch (Exception e)
            {
                await _dialogService.ShowErrorAsync(e);
                return null;
            }
        }
        public async Task<User> GetUserAsync(string email)
        {
            _httpResponseHelper.SetHeader(_httpClient);

            try
            {
                var response = await _httpClient.GetAsync($"{_userApiUri}/GetUserByEmail?email={email}");
                if (!await _httpResponseHelper.IsResponseSuccess(response))
                    return null;

                return await response.Content.ReadFromJsonAsync<User>();

            }
            catch (Exception e)
            {
                await _dialogService.ShowErrorAsync(e);
                return null;
            }
        }
        public async Task<User> CreateUserAsync(AddUserDto userToCreate)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(_userApiUri, userToCreate);

                if (!await _httpResponseHelper.IsResponseSuccess(response))
                    return null;

                return await response.Content.ReadFromJsonAsync<User>();
            }
            catch (Exception e)
            {
                await _dialogService.ShowErrorAsync(e);
                return null;
            }
        }
        public async Task<User> UpdateUserAsync(UpdateUserDto userToUpdate)
        {
            _httpResponseHelper.SetHeader(_httpClient);

            try
            {
                var response = await _httpClient.PutAsJsonAsync(_userApiUri, userToUpdate);

                if (!await _httpResponseHelper.IsResponseSuccess(response))
                    return null;

                return await response.Content.ReadFromJsonAsync<User>();
            }
            catch (Exception e)
            {
                await _dialogService.ShowErrorAsync(e);
                return null;
            }

        }
        public async Task<User> DeleteUserAsync(int id)
        {
            _httpResponseHelper.SetHeader(_httpClient);

            try
            {
                var response = await _httpClient.DeleteAsync($"{_userApiUri}?id={id}");

                if (!await _httpResponseHelper.IsResponseSuccess(response))
                    return null;

                return await response.Content.ReadFromJsonAsync<User>();
            }
            catch (Exception e)
            {
                await _dialogService.ShowErrorAsync(e);
                return null;
            }
        }
    }
}
