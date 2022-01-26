
using Puma.Models;
using Puma.Services;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xamarin.Forms;

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
            var response = await _httpClient.PostAsJsonAsync(_poiApiUri, poi);

            if (!await IsResponseSuccess(response))
                return null;

            return await response.Content.ReadFromJsonAsync<PointOfInterest>();
        }
        public async Task<Tag> CreateTagAsync(string name)
        {
            StringContent query = new StringContent(name);

            var response = await _httpClient.PostAsync($"{_poiApiUri}/AddTag", query);

            if (!await IsResponseSuccess(response))
                return null;

            return await response.Content.ReadFromJsonAsync<Tag>();
        }

        public async Task<PointOfInterest> AddCommentAsync(AddCommentDto addCommentDto)
        {

            var response = await _httpClient.PostAsJsonAsync($"{_poiApiUri}/AddComment", addCommentDto);

            if (!await IsResponseSuccess(response))
                return null;

            return await response.Content.ReadFromJsonAsync<PointOfInterest>();
        }

        public async Task<PointOfInterest> AddGradeAsync(AddGradeDto addGradeDto)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_poiApiUri}/AddGrade", addGradeDto);

            if (!await IsResponseSuccess(response))
                return null;

            return await response.Content.ReadFromJsonAsync<PointOfInterest>();
        }
        #endregion

        #region Read
        public async Task<PointOfInterest> GetAsync(int id)
        {
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
    }
}
