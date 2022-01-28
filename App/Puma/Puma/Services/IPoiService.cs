

using Puma.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;

namespace Puma.Services
{
    internal interface IPoiService
    {
        Task<PointOfInterest> CreatePoiAsync(AddPoiDto poi);
        Task<Tag> CreateTagAsync(string name);
        Task<PointOfInterest> AddCommentAsync(AddCommentDto addCommentDto);
        Task<PointOfInterest> AddGradeAsync(AddGradeDto addGradeDto);
        Task<PointOfInterest> GetAsync(int id);
        Task<List<PointOfInterest>> GetAsync(Position searchedPosition);
        Task<ObservableCollection<PointOfInterest>> GetAllAsync();
        Task<List<Tag>> GetTags();
        Task<PointOfInterest> Delete(int poiId);
    }
}
