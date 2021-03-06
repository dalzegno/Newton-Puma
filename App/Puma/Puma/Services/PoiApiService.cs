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
using Puma.Extensions;
using System.Globalization;

[assembly: Dependency(typeof(PoiApiService))]
namespace Puma.Services
{
    public class PoiApiService : IPoiService
    {
        readonly IDialogService _dialogService = DependencyService.Get<IDialogService>();
        readonly HttpClient _httpClient = new HttpClient();
        readonly string _poiApiUri = "https://localhost:44314/api/Poi";

        #region Create
        public async Task<PointOfInterest> CreatePoiAsync(AddPoiDto poi)
        {
            _httpClient.SetHeader();
            try
            {
                var response = await _httpClient.PostAsJsonAsync(_poiApiUri, poi);

                if (!await response.IsResponseSuccessAsync(_dialogService))
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
            _httpClient.SetHeader();

            try
            {
                StringContent query = new StringContent(name);

                var response = await _httpClient.PostAsync($"{_poiApiUri}/AddTag", query);

                if (!await response.IsResponseSuccessAsync(_dialogService))
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
            _httpClient.SetHeader();
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"{_poiApiUri}/AddComment", addCommentDto);

                if (!await response.IsResponseSuccessAsync(_dialogService))
                    return null;

                var poi = await response.Content.ReadFromJsonAsync<PointOfInterest>();
                if (poi != null)
                {
                    AdjustCommentRemoveVisibility(poi, true);
                }

                return poi;
            }
            catch (Exception e)
            {
                await _dialogService.ShowErrorAsync(e);
                return null;
            }
        }

        public async Task<PointOfInterest> AddGradeAsync(AddGradeDto addGradeDto)
        {
            _httpClient.SetHeader();

            try
            {
                var response = await _httpClient.PostAsJsonAsync($"{_poiApiUri}/AddGrade", addGradeDto);

                if (!await response.IsResponseSuccessAsync(_dialogService))
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
            _httpClient.SetHeader();

            try
            {
                var response = await _httpClient.GetAsync($"{_poiApiUri}?id={id}");

                if (!await response.IsResponseSuccessAsync())
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
            try
            {
                _httpClient.SetHeader();

                var response = await _httpClient.GetAsync($"{_poiApiUri}/GetFromLatAndLon?lat={searchedPosition.Latitude.ToString(CultureInfo.InvariantCulture)}&lon={searchedPosition.Longitude.ToString(CultureInfo.InvariantCulture)}");

                if (!await response.IsResponseSuccessAsync())
                    return null;

                return await response.Content.ReadFromJsonAsync<List<PointOfInterest>>();
            }
            catch (Exception e)
            {
                await _dialogService.ShowErrorAsync(e);
                return null;
            }

        }
        public async Task<ObservableCollection<PointOfInterest>> GetAsync(double lat, double lon)
        {
            try
            {
                _httpClient.SetHeader();

                var latStr = lat.ToString(CultureInfo.InvariantCulture);
                var lonStr = lon.ToString(CultureInfo.InvariantCulture);
                var response = await _httpClient.GetAsync($"{_poiApiUri}/GetFromLatAndLon?lat={latStr}&lon={lonStr}");

                if (!await response.IsResponseSuccessAsync())
                    return null;

                var pois = await response.Content.ReadFromJsonAsync<ObservableCollection<PointOfInterest>>();

                if (App.LoggedInUser != null)
                {
                    foreach (var poi in pois)
                    {
                        AdjustCommentRemoveVisibility(poi);
                    }
                }

                return pois;
            }
            catch (Exception e)
            {
                await _dialogService.ShowErrorAsync(e);
                return null;
            }

        }
        public async Task<ObservableCollection<PointOfInterest>> GetAllAsync()
        {
            _httpClient.SetHeader();
            try
            {
                var response = await _httpClient.GetAsync($"{_poiApiUri}/GetAll");

                if (!await response.IsResponseSuccessAsync())
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

                if (!await response.IsResponseSuccessAsync())
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
            _httpClient.SetHeader();
            try
            {
                var response = await _httpClient.DeleteAsync($"{_poiApiUri}/?id={poiId}");

                if (!await response.IsResponseSuccessAsync(_dialogService))
                    return null;

                return await response.Content.ReadFromJsonAsync<PointOfInterest>();
            }
            catch (Exception e)
            {
                await _dialogService.ShowErrorAsync(e);
                return null;
            }

        }

        public async Task<Comment> DeleteComment(int userId, int commentId)
        {
            _httpClient.SetHeader();
            try
            {
                var response = await _httpClient.DeleteAsync($"{_poiApiUri}/DeleteComment/?commentid={commentId}&userid={userId}");

                if (!await response.IsResponseSuccessAsync(_dialogService))
                    return null;

                return await response.Content.ReadFromJsonAsync<Comment>();
            }
            catch (Exception e)
            {
                await _dialogService.ShowErrorAsync(e);
                return null;
            }
        }
        #endregion

        private static void AdjustCommentRemoveVisibility(PointOfInterest poi, bool addedNow = false)
        {
            foreach (var comment in poi.Comments)
            {
                if (comment.UserId == App.LoggedInUser.Id)
                {
                    comment.IsRemoveVisible = true;
                    if (addedNow)
                        comment.UserDisplayName = App.LoggedInUser.DisplayName;
                }
            }
        }
    }
}