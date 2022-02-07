using Puma.Models;
using System;
using System.Net.Http;

using System.Net.Http.Json; //Requires nuget package System.Net.Http.Json
using System.Threading.Tasks;
using Puma.Services;
using Xamarin.Forms;
using Puma.Extensions;

[assembly: Dependency(typeof(UserApiService))]
namespace Puma.Services
{
    public class UserApiService : IUserApiService
    {
        readonly IDialogService _dialogService = DependencyService.Get<IDialogService>();
        readonly HttpClient _httpClient = new HttpClient();
        readonly string _userApiUri = "http://localhost:64500/api/User";
        public User CurrentUser;

        public async Task<User> LogIn(string email, string password)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_userApiUri}/LogIn?email={email}&password={password}");

                if (!await response.IsResponseSuccessAsync(_dialogService))
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
            _httpClient.SetHeader();

            try
            {
                var response = await _httpClient.GetAsync($"{_userApiUri}/GetByEmail?email={email}");
                if (!await response.IsResponseSuccessAsync(_dialogService))
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

                if (!await response.IsResponseSuccessAsync(_dialogService))
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
            _httpClient.SetHeader();

            try
            {
                var response = await _httpClient.PutAsJsonAsync(_userApiUri, userToUpdate);

                if (!await response.IsResponseSuccessAsync(_dialogService))
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
            _httpClient.SetHeader();

            try
            {
                var response = await _httpClient.DeleteAsync($"{_userApiUri}?id={id}");

                if (!await response.IsResponseSuccessAsync(_dialogService))
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
