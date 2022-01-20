﻿

using Puma.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Puma.Services
{
    internal interface IPoiService
    {
        Task<PointOfInterest> CreatePoiAsync(AddPoiDto poi);
        Task<Tag> CreateTagAsync(string name);
        Task<PointOfInterest> AddCommentAsync(AddCommentDto addCommentDto);
        Task<PointOfInterest> AddGradeAsync(AddGradeDto addGradeDto);
        Task<PointOfInterest> GetAsync(int id);
        Task<List<Tag>> GetTags();
        Task<PointOfInterest> Delete(int poiId);
    }
}