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
        // Create
        Task<PointOfInterestDto> CreateAsync(AddPoiDto pointOfInterest);
        Task<PointOfInterestDto> AddCommentAsync(AddCommentDto comment);
        Task<PointOfInterestDto> AddGrade(AddGradeDto addGradeDto);
        Task<TagDto> CreateTagAsync(string name);
        // Read
        Task<PointOfInterestDto> GetAsync(int id);
        Task<IEnumerable<PointOfInterestDto>> GetAsync(double lat, double lon);
        Task<IEnumerable<PointOfInterestDto>> GetAllAsync();
        Task<IEnumerable<TagDto>> GetTagsAsync();
        // Update
        Task<PointOfInterestDto> UpdateAsync(PointOfInterestDto pointOfInterest);
        // Delete
        Task<PointOfInterestDto> DeleteAsync(int pointOfInterest);
        Task<CommentDto> DeleteCommentAsync(int userId, int commentId);

    }
}
