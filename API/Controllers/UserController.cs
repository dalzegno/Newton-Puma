using Logic.Models;
using Logic.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet("LogIn")]
        public async Task<ActionResult<UserDto>> Login([FromQuery] string email, [FromQuery] string password)
        {
            var user = await _userService.LogIn(email, password);

            if (user == null)
                NotFound("Username or password was wrong.");

            return Ok(user);
        }


        /// <summary>
        /// Gets all users
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Returns all users</response>
        /// <response code="404">If users could not be found</response>
        [HttpGet("GetAllUsers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ICollection<UserDto>>> Get()
        {
            ICollection<UserDto> users = await _userService.GetAllAsync();

            if (users.Count == 0)
                return NotFound();

            return Ok(users);
        }

        /// <summary>
        /// Get a user by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The requested user</returns>
        /// <response code="200">Returns the requested user</response>
        /// <response code="404">If user couldn't be found</response>
        /// <response code="400">If request was faulty</response>
        [HttpGet("GetUserById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> GetUser([FromQuery] int id)
        {
            if (id == 0)
                return BadRequest("Request parameter was 0");

            UserDto user = await _userService.GetUserAsync(id);

            if (user == null)
                return NotFound($"Could not find a user with id: {id}");

            return Ok(user);
        }

        /// <summary>
        /// Get a user by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns>The requested user</returns>
        /// <response code="200">Returns the requested user</response>
        /// <response code="404">If user couldn't be found</response>
        /// <response code="400">If user couldn't be found</response>
        [HttpGet("GetUserByEmail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserDto>> GetUser([FromQuery] string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return BadRequest("Request contained null or faulty values");

            UserDto user = await _userService.GetUserAsync(email.ToLower());

            if (user == null)
                return NotFound($"Could not find a user with email: {email}");

            return Ok(user);
        }

        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <param name="userToPost"></param>
        /// <returns></returns>
        /// <response code="201">Returns the created user</response>
        /// <response code="400">If the user was null</response>
        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserDto>> Post([FromBody] UserDto userToPost)
        {
            UserDto createdUser = await _userService.PostUserAsync(userToPost);

            if (createdUser == null)
                return BadRequest("The request body was null, or contained faulty values.");

            return Ok(createdUser);
        }
        /// <summary>
        /// Edit a user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPut("EditUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> EditUser([FromBody] UserDto user)
        {
            UserDto updatedUser = await _userService.EditUser(user);

            if (updatedUser == null)
                return NotFound();

            return Ok(updatedUser);
        }

        ///// <summary>
        ///// Set if user is admin or not (true/false)
        ///// </summary>
        ///// <param name="id"></param>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //[HttpPut("SetAdmin")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<ActionResult<UserDto>> SetAdmin([FromQuery] int id, [FromQuery] bool value)
        //{
        //    UserDto updatedUser = await _userService.SetAdmin(id, value);
        //    if (updatedUser == null)
        //        return NotFound();

        //    return Ok(updatedUser);
        //}
    }
}
