using ExamenAppDotNet.Models;
using ExamenAppDotNet.Services;
using ExamenAppDotNet.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExamenAppDotNet.Controllers
{
    // https://jasonwatmore.com/post/2018/08/14/aspnet-core-21-jwt-authentication-tutorial-with-example-api

    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUsersService userService;

        public UsersController(IUsersService userService)
        {
            this.userService = userService;
        }
        // returns the logged in user
        private User GetConectedUser()
        {
            return userService.GetCurrentUser(HttpContext);
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]LoginPostModel login)
        {
            var user = userService.Authenticate(login.Username, login.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }


        [AllowAnonymous]
        [HttpPost("register")]
        //[HttpPost]
        public IActionResult Register([FromBody]RegisterPostModel registerModel)
        {
            var user = userService.Register(registerModel);
            if (user == null)
            {
                return BadRequest(new { ErrorMessage = "Username already exists." });
            }
            return Ok(user);
        }


        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = userService.GetAll();
            return Ok(users);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var user = userService.DeleteUser(id);
            if (user == null)
            {
                return BadRequest();
            }
            
            return Ok(user);
        }
        
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Put(int id, [FromBody] RegisterPostModel registerPostModel)
        {
            var user = userService.UpsertUser(id, RegisterPostModel.ToUser(registerPostModel));

            if (user == null)
            {
                return BadRequest();
            }

            return Ok(user);
        }

  
        [HttpGet("GetLogedUser")]
        public IActionResult GetLogedUser()
        {
            var user = userService.GetCurrentUser(HttpContext);

            if (user == null)
                return BadRequest();

            return Ok(user);
        }
    }
}