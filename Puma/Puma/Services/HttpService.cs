using Client.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

using System.Net.Http.Json; //Requires nuget package System.Net.Http.Json
using System.Threading.Tasks;

namespace Client.Services
{
    public class HttpService
    {
        HttpClient httpClient;
        HttpClientHandler httpClientHandler;
        //string userApiUri = "https://localhost:44397/api/User/GetAllUsers";

        public HttpService()
        {
            ////httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
            //httpClient = new HttpClient(httpClientHandler);

            //httpClientHandler = new HttpClientHandler
            //{
            //    ClientCertificateOptions = ClientCertificateOption.Manual,
            //    ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) =>
            //    {
            //        return true;
            //    }
            //};


        }

        //public async Task<List<UserDto>> GetUsersAsync()
        //{
        //    var response = await httpClient.GetAsync(userApiUri);

        //    if (!response.IsSuccessStatusCode)
        //    {
        //        System.Diagnostics.Debug.WriteLine($"Error: {response}");
        //        return null;
        //    }

        //    var users = await response.Content.ReadFromJsonAsync<List<UserDto>>();

        //    return users;
        //}


    }
}
