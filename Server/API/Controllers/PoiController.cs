using Logic.Helpers;
using Logic.Models;
using Logic.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class PoiController : Controller
    {
        IPoiService _poiService;
        IUserService _userService;
        public PoiController(IPoiService poiService, IUserService userService)
        {
            _poiService = poiService;
            _userService = userService;
        }

        #region Create
        /// <summary>
        /// Creates a new point of interest
        /// </summary>
        /// <param name="poi"></param>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        /// <response code="200">Returns the created point of interest or the the found poi</response>
        /// <response code="400">If request contained faulty values</response>
        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PointOfInterestDto>> Post([FromBody] AddPoiDto poi, [FromHeader] string apiKey)
        {
            if (!await _userService.IsUserAuthorizedAsync(apiKey))
                return Unauthorized("Unauthorized");

            PointOfInterestDto createdPoi = await _poiService.CreateAsync(poi);

            if (createdPoi == null)
                return BadRequest("The request body was null, or contained faulty values.");

            return Ok(createdPoi);
        }

        /// <summary>
        /// Adds a tag to use for POIS
        /// </summary>
        /// <param name="name"></param>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        [HttpPost("AddTag")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PointOfInterestDto>> AddTag([FromQuery] string name, [FromHeader] string apiKey)
        {
            if (!await _userService.IsUserAuthorizedAsync(apiKey))
                return Unauthorized("Unauthorized");

            TagDto createdTag = await _poiService.CreateTagAsync(name);

            return Ok(createdTag);
        }

        /// <summary>
        /// Add a comment to a POI
        /// </summary>
        /// <param name="comment"></param>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        [HttpPost("AddComment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PointOfInterestDto>> AddComment([FromBody] AddCommentDto comment, [FromHeader] string apiKey)
        {
            if (!await _userService.IsUserAuthorizedAsync(apiKey))
                return Unauthorized("Unauthorized");

            var poiCommentAdded = await _poiService.AddCommentAsync(comment);

            if (poiCommentAdded == null)
                return NotFound($"Could not find a Point of interest with id: {comment.PointOfInterestId}");

            return Ok(poiCommentAdded);
        }

        [HttpPost("AddGrade")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PointOfInterestDto>> AddGrade([FromBody] AddGradeDto addGradeDto, [FromHeader] string apiKey)
        {
            if (!await _userService.IsUserAuthorizedAsync(apiKey))
                return Unauthorized("Unauthorized");

            var poi = await _poiService.AddGrade(addGradeDto);

            if (poi == null)
                return NotFound();

            return Ok(poi);
        }
        #endregion

        #region Read
        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PointOfInterestDto>> Get([FromQuery] int id)
        {
            var poi = await _poiService.GetAsync(id);

            if (poi == null)
                return NotFound();

            return Ok(poi);
        }

        [HttpGet("GetFromLatAndLon")]
        public async Task<ActionResult<IEnumerable<PointOfInterestDto>>> Get([FromQuery] double lat, [FromQuery] double lon)
        {
            if (!lat.TryParseToInvariantCulture(out double latDouble) || !lon.TryParseToInvariantCulture(out double lonDouble))
                return BadRequest("The provided latitude and/or longitude values were faulty.");

            var pois = await _poiService.GetAsync(latDouble, lonDouble);

            if (pois == null || !pois.Any())
                return NotFound("Could not find any pois in the area of the provided latitude and longitude values");

            return Ok(pois);
        }

        /// <summary>
        /// Gets all POI
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Returns all users</response>
        /// <response code="404">If users could not be found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<PointOfInterestDto>>> Get()
        {
            IEnumerable<PointOfInterestDto> pois = await _poiService.GetAllAsync();

            if (!pois.Any())
                return NotFound("Could not find any points of interests.");

            return Ok(pois);
        }

        [HttpGet("GetTags")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ICollection<TagDto>>> GetTags()
        {
            var tags = await _poiService.GetTagsAsync();

            if (tags == null)
                return NotFound("Could not find any tags");

            return Ok(tags);
        }
        #endregion

        #region Update

        #endregion

        #region Delete
        [HttpDelete()]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PointOfInterestDto>> Delete([FromQuery] int id, [FromHeader] string apiKey)
        {
            if (!await _userService.IsUserAuthorizedAsync(apiKey))
                return Unauthorized("Unauthorized");

            var deletedPoi = await _poiService.DeleteAsync(id);

            if (deletedPoi == null)
                return BadRequest("Could not find a poi to delete.");

            return Ok(deletedPoi);
        }
        #endregion
    }
}