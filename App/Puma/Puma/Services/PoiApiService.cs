
using Puma.Models;
using Puma.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

[assembly: Dependency(typeof(PoiApiService))]
namespace Puma.Services
{
    public class PoiApiService : IPoiService
    {
        readonly HttpClient _httpClient = new HttpClient();
        readonly string _poiApiUri = "http://localhost:64500/api/Poi";

        public EventHandler<string> ErrorMessage;
        protected virtual void OnErrorMessage(string e) => ErrorMessage?.Invoke(this, e);

        #region Create
        public async Task<PointOfInterest> CreatePoiAsync(AddPoiDto poi)
        {
            SetHeader();

            var response = await _httpClient.PostAsJsonAsync(_poiApiUri, poi);

            if (!await IsResponseSuccess(response))
                return null;

            return await response.Content.ReadFromJsonAsync<PointOfInterest>();
        }
        public async Task<Tag> CreateTagAsync(string name)
        {
            SetHeader();

            StringContent query = new StringContent(name);

            var response = await _httpClient.PostAsync($"{_poiApiUri}/AddTag", query);

            if (!await IsResponseSuccess(response))
                return null;

            return await response.Content.ReadFromJsonAsync<Tag>();
        }

        public async Task<PointOfInterest> AddCommentAsync(AddCommentDto addCommentDto)
        {
            SetHeader();

            var response = await _httpClient.PostAsJsonAsync($"{_poiApiUri}/AddComment", addCommentDto);

            if (!await IsResponseSuccess(response))
                return null;

            return await response.Content.ReadFromJsonAsync<PointOfInterest>();
        }

        public async Task<PointOfInterest> AddGradeAsync(AddGradeDto addGradeDto)
        {
            SetHeader();

            var response = await _httpClient.PostAsJsonAsync($"{_poiApiUri}/AddGrade", addGradeDto);

            if (!await IsResponseSuccess(response))
                return null;

            return await response.Content.ReadFromJsonAsync<PointOfInterest>();
        }
        #endregion

        #region Read
        public async Task<PointOfInterest> GetAsync(int id)
        {
            SetHeader();

            try
            {
                var response = await _httpClient.GetAsync($"{_poiApiUri}?id={id}");

                if (!await IsResponseSuccess(response))
                    return null;

                return await response.Content.ReadFromJsonAsync<PointOfInterest>();
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<List<PointOfInterest>> GetAsync(Position searchedPosition)
        {
            SetHeader();

            var response = await _httpClient.GetAsync($"{_poiApiUri}/GetPoisFromLatAndLon?lat={searchedPosition.Latitude}&lon={searchedPosition.Longitude}");

            if (!await IsResponseSuccess(response))
                return null;

            return await response.Content.ReadFromJsonAsync<List<PointOfInterest>>();
        }
        public async Task<ObservableCollection<PointOfInterest>> GetAllAsync()
        {
            SetHeader();
            try
            {
                var response = await _httpClient.GetAsync($"{_poiApiUri}/GetAllPoi");

                if (!await IsResponseSuccess(response))
                    return null;

                return await response.Content.ReadFromJsonAsync<ObservableCollection<PointOfInterest>>();
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<List<Tag>> GetTags()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_poiApiUri}/GetTags");

                if (!await IsResponseSuccess(response))
                    return null;

                return await response.Content.ReadFromJsonAsync<List<Tag>>();
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        #region Delete
        public async Task<PointOfInterest> Delete(int poiId)
        {
            SetHeader();

            var response = await _httpClient.DeleteAsync($"{_poiApiUri}/?id={poiId}");

            if (!await IsResponseSuccess(response))
                return null;

            return await response.Content.ReadFromJsonAsync<PointOfInterest>();
        }
        #endregion
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

        private void SetHeader()
        {
            if (App.LoggedInUser == null)
                return;

            // TODO: Blir alltid null
            //var header = _httpClient.DefaultRequestHeaders.FirstOrDefault(a => a.Key == "apiKey");

            //if (header.Value == null)
            _httpClient.DefaultRequestHeaders.Add("apiKey", App.LoggedInUser.ApiKey);
        }

    }
}
