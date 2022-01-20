using Logic.Enums;
using Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Services
{
    public interface IPoiService
    {
        Task<PointOfInterestDto> CreateAsync(AddPoiDto pointOfInterest);
        Task<PointOfInterestDto> AddCommentAsync(AddCommentDto comment);
        Task<PointOfInterestDto> AddGrade(int poiId, int userId, int gradeType);
        Task<TagDto> CreateTagAsync(string name);
        Task<PointOfInterestDto> GetAsync(int id);
        Task<ICollection<TagDto>> GetTagsAsync();
        Task<PointOfInterestDto> UpdateAsync(PointOfInterestDto pointOfInterest);
        Task<PointOfInterestDto> DeleteAsync(int pointOfInterest);
    }
}
