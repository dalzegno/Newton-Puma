using Logic.Models;
using Logic.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PointOfInterestDto>> Get(int id)
        {
            var poi = await _poiService.GetAsync(id);

            if (poi == null)
                return NotFound();

            return Ok(poi);
        }

        [HttpGet("GetTags")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ICollection<TagDto>>> GetTags()
        {
            var tags = await _poiService.GetTagsAsync();

            if (tags == null)
                return NotFound();

            return Ok(tags);
        }

        [HttpPost("AddTag")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PointOfInterestDto>> AddTag(string name)
        {
            TagDto createdTag = await _poiService.CreateTagAsync(name);

            return Ok(createdTag);
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
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PointOfInterestDto>> AddGrade([FromBody] AddGradeDto addGradeDto)
        {

            var poi = await _poiService.AddGrade(addGradeDto);

            if (poi == null)
                return NotFound();

            return Ok(poi);
        }

        [HttpDelete()]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PointOfInterestDto>> Delete([FromQuery] int id)
        {
            var deletedPoi = await _poiService.DeleteAsync(id);

            if (deletedPoi == null)
                return BadRequest();

            return Ok(deletedPoi);
        }
    }   
}
