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
    public class UserController : Controller
    {
        IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        #region Create
        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <param name="userToPost"></param>
        /// <returns></returns>
        /// <response code="200">Returns the created user</response>
        /// <response code="400">If the user was null</response>
        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserDto>> Post([FromBody] AddUserDto userToPost)
        {
            UserDto createdUser = await _userService.CreateAsync(userToPost);

            if (createdUser == null)
                return BadRequest("The request body was null, or contained faulty values.");

            return Ok(createdUser);
        }
        #endregion

        #region Read
        /// <summary>
        /// Log in user
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <response code="200">Returns logged in user</response>
        /// <response code="404">Could not find a user with the provided values</response>
        /// <response code="500">Server error</response>
        [HttpGet("LogIn")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserDto>> Login([FromQuery] string email, [FromQuery] string password)
        {
            var user = await _userService.LogInAsync(email, password);

            if (user == null)
                return NotFound("Username or password was wrong.");

            return Ok(user);
        }


        /// <summary>
        /// Gets all users
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Returns all users</response>
        /// <response code="401">If user is unauthorized to make the call.</response>
        /// <response code="404">If users could not be found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<UserDto>>> Get([FromHeader] string apiKey)
        {
            //if (!await _userService.IsUserAuthorizedAsync(apiKey))
            //    return Unauthorized();

            IEnumerable<UserDto> users = await _userService.GetAllAsync();

            return Ok(users);
        }

        /// <summary>
        /// Get a user by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="apiKey"></param>
        /// <returns>The requested user</returns>
        /// <response code="200">Returns the requested user</response>
        /// <response code="404">If user couldn't be found</response>
        /// <response code="400">If request was faulty</response>
        [HttpGet("GetById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> GetUser([FromQuery] int id, [FromHeader]string apiKey)
        {
            if (!await _userService.IsUserAuthorizedAsync(apiKey))
                return Unauthorized();

            if (id == 0)
                return BadRequest("Request parameter was 0");

            UserDto user = await _userService.GetAsync(id);

            if (user == null)
                return NotFound($"Could not find a user with id: {id}");

            return Ok(user);
        }

        /// <summary>
        /// Get a user by email
        /// </summary>
        /// <param name="email"></param>
        /// <param name="apiKey"></param>
        /// <returns>The requested user</returns>
        /// <response code="200">Returns the requested user</response>
        /// <response code="404">If user couldn't be found</response>
        /// <response code="400">If user couldn't be found</response>
        [HttpGet("GetByEmail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserDto>> GetUser([FromQuery] string email, [FromHeader] string apiKey)
        {
            if (!await _userService.IsUserAuthorizedAsync(apiKey))
                return Unauthorized();

            UserDto user = await _userService.GetAsync(email.ToLower());

            if (user == null)
                return NotFound($"Could not find a user with email: \"{email}\"");

            return Ok(user);
        }
        #endregion

        #region Update
        /// <summary>
        /// Edit a user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        [HttpPut()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> Edit([FromBody] UpdateUserDto user, [FromHeader] string apiKey)
        {

            if (!await _userService.IsUserAuthorizedAsync(apiKey))
                return Unauthorized();

            UserDto updatedUser = await _userService.UpdateAsync(user);

            if (updatedUser == null)
                return NotFound($"Could not find a user with ID: {user.Id}");

            return Ok(updatedUser);
        }
        #endregion

        #region Delete
        /// <summary>
        /// Delete user by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="apiKey"></param>
        /// <response code="200">Returns the deleted user</response>
        /// <response code="404">Not found</response>
        /// <response code="500">Server error</response>
        [HttpDelete()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserDto>> Delete([FromQuery] int id, [FromHeader] string apiKey)
        {
            //if (!await _userService.IsUserAuthorizedAsync(apiKey))
            //    return Unauthorized();

            UserDto deletedUser = await _userService.DeleteAsync(id);

            if (deletedUser == null)
                return NotFound();

            return Ok(deletedUser);
        }
        #endregion
    }
}
