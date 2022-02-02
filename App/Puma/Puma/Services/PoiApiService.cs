
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
using Puma.Helpers;

[assembly: Dependency(typeof(PoiApiService))]
namespace Puma.Services
{
    public class PoiApiService : IPoiService
    {
        readonly IDialogService _dialogService = DependencyService.Get<IDialogService>();
        readonly HttpResponseHelper _httpResponseHelper = DependencyService.Get<HttpResponseHelper>();
        readonly HttpClient _httpClient = new HttpClient();
        readonly string _poiApiUri = "http://localhost:64500/api/Poi";

        #region Create
        public async Task<PointOfInterest> CreatePoiAsync(AddPoiDto poi)
        {
            _httpResponseHelper.SetHeader(_httpClient);
            try
            {
                var response = await _httpClient.PostAsJsonAsync(_poiApiUri, poi);

                if (!await _httpResponseHelper.IsResponseSuccess(response))
                    return null;

                return await response.Content.ReadFromJsonAsync<PointOfInterest>();
            }
            catch (Exception e)
            {
                await _dialogService.ShowErrorAsync(e);
                return null;
            }

        }
        public async Task<Tag> CreateTagAsync(string name)
        {
            _httpResponseHelper.SetHeader(_httpClient);

            try
            {
                StringContent query = new StringContent(name);

                var response = await _httpClient.PostAsync($"{_poiApiUri}/AddTag", query);

                if (!await _httpResponseHelper.IsResponseSuccess(response))
                    return null;

                return await response.Content.ReadFromJsonAsync<Tag>();
            }
            catch (Exception e)
            {
                await _dialogService.ShowErrorAsync(e);
                return null;
            }
        }

        public async Task<PointOfInterest> AddCommentAsync(AddCommentDto addCommentDto)
        {
            _httpResponseHelper.SetHeader(_httpClient);
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"{_poiApiUri}/AddComment", addCommentDto);

                if (!await _httpResponseHelper.IsResponseSuccess(response))
                    return null;

                return await response.Content.ReadFromJsonAsync<PointOfInterest>();
            }
            catch (Exception e)
            {
                await _dialogService.ShowErrorAsync(e);
                return null;
            }
        }

        public async Task<PointOfInterest> AddGradeAsync(AddGradeDto addGradeDto)
        {
            _httpResponseHelper.SetHeader(_httpClient);

            try
            {
                var response = await _httpClient.PostAsJsonAsync($"{_poiApiUri}/AddGrade", addGradeDto);

                if (!await _httpResponseHelper.IsResponseSuccess(response))
                    return null;

                return await response.Content.ReadFromJsonAsync<PointOfInterest>();
            }
            catch (Exception e)
            {
                await _dialogService.ShowErrorAsync(e);
                return null;
            }
        }
        #endregion

        #region Read
        public async Task<PointOfInterest> GetAsync(int id)
        {
            _httpResponseHelper.SetHeader(_httpClient);

            try
            {
                var response = await _httpClient.GetAsync($"{_poiApiUri}?id={id}");

                if (!await _httpResponseHelper.IsResponseSuccess(response))
                    return null;

                return await response.Content.ReadFromJsonAsync<PointOfInterest>();
            }
            catch (Exception e)
            {
                await _dialogService.ShowErrorAsync(e);
                return null;
            }
        }
        public async Task<List<PointOfInterest>> GetAsync(Position searchedPosition)
        {
            _httpResponseHelper.SetHeader(_httpClient);

            try
            {
                var response = await _httpClient.GetAsync($"{_poiApiUri}/GetPoisFromLatAndLon?lat={searchedPosition.Latitude}&lon={searchedPosition.Longitude}");

                if (!await _httpResponseHelper.IsResponseSuccess(response, false))
                    return null;

                return await response.Content.ReadFromJsonAsync<List<PointOfInterest>>();
            }
            catch (Exception e)
            {
                await _dialogService.ShowErrorAsync(e);
                return null;
            }

        }
        public async Task<ObservableCollection<PointOfInterest>> GetAllAsync()
        {
            _httpResponseHelper.SetHeader(_httpClient);
            try
            {
                var response = await _httpClient.GetAsync($"{_poiApiUri}/GetAllPoi");

                if (!await _httpResponseHelper.IsResponseSuccess(response, false))
                    return null;

                return await response.Content.ReadFromJsonAsync<ObservableCollection<PointOfInterest>>();
            }
            catch (Exception e)
            {
                await _dialogService.ShowErrorAsync(e);
                return null;
            }
        }
        public async Task<List<Tag>> GetTags()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_poiApiUri}/GetTags");

                if (!await _httpResponseHelper.IsResponseSuccess(response, false))
                    return null;

                return await response.Content.ReadFromJsonAsync<List<Tag>>();
            }
            catch (Exception e)
            {
                await _dialogService.ShowErrorAsync(e);
                return null;
            }
        }
        #endregion

        #region Delete
        public async Task<PointOfInterest> Delete(int poiId)
        {
            _httpResponseHelper.SetHeader(_httpClient);
            try
            {
                var response = await _httpClient.DeleteAsync($"{_poiApiUri}/?id={poiId}");

                if (!await _httpResponseHelper.IsResponseSuccess(response))
                    return null;

                return await response.Content.ReadFromJsonAsync<PointOfInterest>();
            }
            catch (Exception e)
            {
                await _dialogService.ShowErrorAsync(e);
                return null;
            }
            
        }
        #endregion

    }
}
