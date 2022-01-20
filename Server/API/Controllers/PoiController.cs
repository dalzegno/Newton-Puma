﻿using Logic.Models;
using Logic.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class PoiController : Controller
    {
        IPoiService _poiService;
        public PoiController(IPoiService poiService)
        {
            _poiService = poiService;
        }

        [HttpGet()]
        public async Task<ActionResult<PointOfInterestDto>> Get(int id)
        {
            var poi = await _poiService.GetAsync(id);

            return Ok(poi);
        }

        /// <summary>
        /// Creates a new point of interest
        /// </summary>
        /// <param name="poi"></param>
        /// <returns></returns>
        /// <response code="200">Returns the created point of interest or the the found poi</response>
        /// <response code="400">If request contained faulty values</response>
        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PointOfInterestDto>> Post([FromBody] AddPoiDto poi)
        {
            PointOfInterestDto createdPoi = await _poiService.CreateAsync(poi);

            if (createdPoi == null)
                return BadRequest("The request body was null, or contained faulty values.");

            return Ok(createdPoi);
        }

        [HttpPost("AddComment")]
        public async Task<ActionResult<PointOfInterestDto>> AddComment([FromBody] AddCommentDto comment)
        {
            var poiCommentAdded = await _poiService.AddCommentAsync(comment);

            if (poiCommentAdded == null)
                return BadRequest();

            return Ok(poiCommentAdded);
        }

        [HttpPost("AddGrade")]
        public async Task<ActionResult<PointOfInterestDto>> AddGrade([FromQuery]int poiId, [FromQuery] int userId, [FromQuery] int gradeType)
        {
            var poi = await _poiService.AddGrade(poiId, userId, gradeType);

            if (poi == null)
                return NotFound();

            return Ok(poi);
        }

        [HttpDelete()]
        public async Task<ActionResult<PointOfInterestDto>> Delete([FromQuery] int id)
        {
            var deletedPoi = await _poiService.DeleteAsync(id);

            if (deletedPoi == null)
                return BadRequest();

            return Ok(deletedPoi);
        }
    }   
}
