using API.Models;
using Logic.Models;
using Logic.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        [HttpGet("GetAllUsers")]
        public async Task<PumaResponse> Get([FromServices] UserService userService)
        {
            ICollection<UserDto> users = await userService.GetAsync();
            return PumaResponse.CreateSuccessResponse(users);
        }

        [HttpGet("GetUser")]
        public async Task<PumaResponse> GetUser([FromServices] UserService userService, [FromQuery]int userId)
        {
            UserDto user = await userService.GetUserAsync(userId);
            return PumaResponse.CreateSuccessResponse(user);
        }

        [HttpPost("AddUser")]
        public async Task<PumaResponse> Post([FromServices] UserService userService, [FromBody] UserDto userToPost)
        {
            var result = await userService.PostUserAsync(userToPost);
            return PumaResponse.CreateSuccessResponse(result);
        }
    }
}
