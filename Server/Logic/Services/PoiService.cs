﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Logic.Models;
using Microsoft.EntityFrameworkCore;
using PumaDbLibrary;
using PumaDbLibrary.Entities;

namespace Logic.Services
{
    public class PoiService : IPoiService
    {
        PumaDbContext _context;
        IMapper _mapper;
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
                return _mapper.Map<PointOfInterestDto>(foundPoi);

            var dbPoi = new PointOfInterest
            {
                Description = pointOfInterest.Description ?? "",
                Name = pointOfInterest.Name,
                Address = await _context.Addresses.FirstOrDefaultAsync(a => a.Id == pointOfInterest.Address.Id || a.StreetName == pointOfInterest.Address.StreetName)
                          ?? _mapper.Map<Address>(pointOfInterest.Address),
                Position = await _context.Positions.FirstOrDefaultAsync(p => p.Latitude == pointOfInterest.Position.Latitude || p.Longitude == pointOfInterest.Position.Longitude)
                           ?? _mapper.Map<Position>(pointOfInterest.Position)
            };

            if (pointOfInterest.TagIds.Count > 0)
            {
                foreach (var tagId in pointOfInterest.TagIds)
                {
                    dbPoi.PoiTags.Add(new PoiTag
                    {
                        PointOfInterest = dbPoi,
                        Tag = await _context.Tags.FirstOrDefaultAsync(t => t.Id == tagId)
                    });
                }
            }

            try
            {
                await _context.PointOfInterests.AddAsync(dbPoi);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                // TODO: Errorhandling
                return null;
            }

            return _mapper.Map<PointOfInterestDto>(dbPoi);
        }
        public async Task<PointOfInterestDto> AddGrade(int poiId, int userId, int gradeType)
        {
            var poi = await _context.PointOfInterests.Include(p => p.Gradings)
                                                       .FirstOrDefaultAsync(poi => poi.Id == poiId);
            var previousGrade = await _context.Gradings.FirstOrDefaultAsync(g => g.UserId == userId && g.PointOfInterestId == poiId);

            // User can either put one like or one dislike on a POI
            if (previousGrade != null && poi != null)
            {
                previousGrade.GradeType = gradeType;
                await _context.SaveChangesAsync();

                return _mapper.Map<PointOfInterestDto>(poi); ;
            }

            if (poi == null)
                return null;

            poi.Gradings.Add(new Grading
            {
                GradeType = gradeType,
                PointOfInterestId = poiId,
                UserId = userId
            });

            await _context.SaveChangesAsync();

            return _mapper.Map<PointOfInterestDto>(poi);
        }
        public async Task<PointOfInterestDto> AddCommentAsync(AddCommentDto comment)
        {
            var pointOfInterest = await _context.PointOfInterests.Include(p => p.Comments)
                                                                 .FirstOrDefaultAsync(p => p.Id == comment.PointOfInterestId);

            if (pointOfInterest == null)
                //TODO: Throw exception?
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

            try
            {
                var entity = await _context.Tags.AddAsync(dbTag);
                dbTag = entity.Entity;
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception("Could not add the tag to database");
            }
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

            if (objectToDelete != null)
                return null;

            try
            {
                _context.PointOfInterests.Remove(objectToDelete);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception("Could not remove POI.");
            }

            return _mapper.Map<PointOfInterestDto>(objectToDelete);
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
                                                          .FirstOrDefaultAsync(poi => poi.Id == id);
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
                                                                      .Include(poi => poi.PoiTags)
                                                                      .Include(poi => poi.Gradings)
                                                                      .FirstOrDefaultAsync(poi => poi.Name.ToLower() == name.ToLower() ||
                                                                                           poi.Position.Latitude == lat ||
                                                                                           poi.Position.Longitude == lon);
        }
        #endregion
    }
}