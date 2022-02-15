using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using AutoMapper;
using Logic.Models;
using PumaDbLibrary;
using PumaDbLibrary.Entities;

namespace Logic.Services
{
    public class PoiService : IPoiService
    {
        readonly PumaDbContext _context;
        readonly IMapper _mapper;
        public PoiService(PumaDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #region Create
        public async Task<PointOfInterestDto> CreateAsync(AddPoiDto pointOfInterest)
        {
            PointOfInterest foundPoi = await GetPoiFromDbAsync(pointOfInterest.Name, pointOfInterest.Position.Latitude, pointOfInterest.Position.Longitude);

            if (foundPoi != null)
            {
                return _mapper.Map<PointOfInterestDto>(foundPoi);
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == pointOfInterest.UserId);

            if (user == null)
            {
                throw new Exception("There is no user with the provided user id.");
            }

            var dbPoi = new PointOfInterest
            {
                Description = pointOfInterest.Description ?? "",
                Name = pointOfInterest.Name,
                Address = await GetAddressAsync(pointOfInterest) ?? _mapper.Map<Address>(pointOfInterest.Address),
                Position = await GetPositionAsync(pointOfInterest) ?? _mapper.Map<Position>(pointOfInterest.Position),
                UserId = user.Id
            };

            if (pointOfInterest.TagIds?.Count > 0)
            {
                await AddTagsToPoiAsync(pointOfInterest.TagIds, dbPoi);
            }

            await _context.PointOfInterests.AddAsync(dbPoi);
            await _context.SaveChangesAsync();

            return _mapper.Map<PointOfInterestDto>(dbPoi);
        }
        public async Task<PointOfInterestDto> AddGrade(AddGradeDto grading)
        {
            var poi = await _context.PointOfInterests.Include(p => p.Gradings)
                                                     .FirstOrDefaultAsync(poi => poi.Id == grading.PoiId);

            if (poi == null)
                return null;

            var previousGrade = poi.Gradings.FirstOrDefault(g => g.UserId == grading.UserId);

            // User can either put one like or one dislike on a POI
            if (previousGrade != null && previousGrade.GradeType != grading.Grade)
            {
                previousGrade.GradeType = grading.Grade;
                await _context.SaveChangesAsync();

                return _mapper.Map<PointOfInterestDto>(poi);
            }

            // If user sent the same gradetype again, remove the previous grade 
            if (previousGrade != null && previousGrade.GradeType == grading.Grade)
            {
                poi.Gradings.Remove(previousGrade);
                await _context.SaveChangesAsync();
                return _mapper.Map<PointOfInterestDto>(poi);
            }


            poi.Gradings.Add(new Grading
            {
                GradeType = grading.Grade,
                PointOfInterestId = grading.PoiId,
                UserId = grading.UserId
            });

            await _context.SaveChangesAsync();

            return _mapper.Map<PointOfInterestDto>(poi);
        }
        public async Task<PointOfInterestDto> AddCommentAsync(AddCommentDto comment)
        {
            var pointOfInterest = await GetPoiFromDbAsync(comment.PointOfInterestId);

            if (pointOfInterest == null)
                return null;

            pointOfInterest.Comments.Add(_mapper.Map<Comment>(comment));
            await _context.SaveChangesAsync();

            return _mapper.Map<PointOfInterestDto>(pointOfInterest);
        }

        public async Task<TagDto> CreateTagAsync(string name)
        {
            var foundTag = await _context.Tags.FirstOrDefaultAsync(t => t.Name.ToLower() == name.ToLower());

            if (foundTag != null)
                return _mapper.Map<TagDto>(foundTag);

            var dbTag = new Tag { Name = name };

            var entity = await _context.Tags.AddAsync(dbTag);
            dbTag = entity.Entity;

            await _context.SaveChangesAsync();

            return _mapper.Map<TagDto>(dbTag);
        }
        #endregion

        #region Read
        public async Task<PointOfInterestDto> GetAsync(int id)
        {
            var foundPoi = await GetPoiFromDbAsync(id);

            if (foundPoi == null)
                return null;

            return _mapper.Map<PointOfInterestDto>(foundPoi);
        }

        public async Task<IEnumerable<PointOfInterestDto>> GetAsync(double lat, double lon)
        {
            var pois = await _context.PointOfInterests.Include(poi => poi.Address)
                                                 .Include(poi => poi.Position)
                                                 .Include(poi => poi.Comments)
                                                 .ThenInclude(c => c.User)
                                                 .Include(poi => poi.PoiTags)
                                                 .Include(poi => poi.Gradings)
                                                 .Where(poi => poi.Position.Latitude < (lat + 0.5) && poi.Position.Latitude > (lat - 0.5) &&
                                                               poi.Position.Longitude < (lon + 0.5) && poi.Position.Longitude > (lon - 0.5))
                                                 .ToListAsync();

            if (pois == null || pois.Count == 0)
                return null;

            return _mapper.Map<IEnumerable<PointOfInterestDto>>(pois);
        }

        public async Task<IEnumerable<PointOfInterestDto>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<PointOfInterestDto>>(await GetPoisFromDbAsync());
        }

        public async Task<ICollection<TagDto>> GetTagsAsync()
        {
            var tags = await _context.Tags.ToListAsync();

            if (tags?.Count < 1)
                return null;

            return tags.Select(t => _mapper.Map<TagDto>(t)).ToList();
        }
        #endregion

        #region Update
        public Task<PointOfInterestDto> UpdateAsync(PointOfInterestDto pointOfInterest)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Delete
        public async Task<PointOfInterestDto> DeleteAsync(int id)
        {
            PointOfInterest objectToDelete = await GetPoiFromDbAsync(id);

            if (objectToDelete == null)
                return null;

            _context.PointOfInterests.Remove(objectToDelete);
            await _context.SaveChangesAsync();

            return _mapper.Map<PointOfInterestDto>(objectToDelete);
        }

        public async Task<CommentDto> DeleteCommentAsync(int userId, int commentId)
        {
            var comment = await _context.Comments.Include(c => c.User)
                                                 .FirstOrDefaultAsync(c => c.Id == commentId && c.UserId == userId);

            if (comment == null)
                return null;

            _context.Comments.Remove(comment);

            await _context.SaveChangesAsync();
            return _mapper.Map<CommentDto>(comment);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get POI from Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task<PointOfInterest> GetPoiFromDbAsync(int id)
        {
            return await _context.PointOfInterests.Include(poi => poi.Address)
                                                  .Include(poi => poi.Position)
                                                  .Include(poi => poi.Comments)
                                                  .Include(poi => poi.PoiTags)
                                                  .Include(poi => poi.Gradings)
                                                  .Include(poi => poi.User)
                                                  .FirstOrDefaultAsync(poi => poi.Id == id);
        }
        /// <summary>
        /// Get all POI from Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task<IEnumerable<PointOfInterest>> GetPoisFromDbAsync()
        {
            return await _context.PointOfInterests.Include(poi => poi.Address)
                                                  .Include(poi => poi.Position)
                                                  .Include(poi => poi.Comments)
                                                  .Include(poi => poi.PoiTags)
                                                  .Include(poi => poi.Gradings)
                                                  .Include(poi => poi.User)
                                                  .ToListAsync();
        }
        /// <summary>
        /// Get a POI on Name, Latitude and Longitude
        /// </summary>
        /// <param name="pointOfInterest"></param>
        /// <returns></returns>
        private async Task<PointOfInterest> GetPoiFromDbAsync(string name, double lat, double lon)
        {
            return await _context.PointOfInterests.Include(poi => poi.Address)
                                                                      .Include(poi => poi.Position)
                                                                      .Include(poi => poi.Comments)
                                                                      .ThenInclude(c => c.User)
                                                                      .Include(poi => poi.PoiTags)
                                                                      .Include(poi => poi.Gradings)
                                                                      .FirstOrDefaultAsync(poi => poi.Name.ToLower() == name.ToLower() &&
                                                                                           (poi.Position.Latitude == lat &&
                                                                                           poi.Position.Longitude == lon));
        }
        private async Task<Position> GetPositionAsync(AddPoiDto pointOfInterest)
        {
            return await _context.Positions.FirstOrDefaultAsync(p => p.Latitude == pointOfInterest.Position.Latitude && p.Longitude == pointOfInterest.Position.Longitude);
        }

        private async Task<Address> GetAddressAsync(AddPoiDto pointOfInterest)
        {
            return await _context.Addresses.FirstOrDefaultAsync(a => a.StreetName == pointOfInterest.Address.StreetName);
        }

        private async Task AddTagsToPoiAsync(List<int> tagIds, PointOfInterest dbPoi)
        {
            foreach (var tagId in tagIds)
            {
                var tag = new PoiTag
                {
                    PointOfInterest = dbPoi,
                    Tag = await _context.Tags.FirstOrDefaultAsync(t => t.Id == tagId)
                };

                dbPoi.PoiTags.Add(tag);
            }
        }
        #endregion
    }
}
