using AngularAuthApi.Context;
using AngularAuthApi.Helpers;
using AngularAuthApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AngularAuthApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _authContext;

        public UserController(AppDbContext authContext)
        {
            _authContext = authContext;
        }

        [HttpPost("authenticate")]
        public async Task<ActionResult> Authenticate([FromBody] User userObj)
        {
            if(userObj == null)
                return BadRequest();

            var user = await _authContext.Users.FirstOrDefaultAsync(x => x.Username == userObj.Username && x.Password == userObj.Password);
            
            if (user == null)
                return NotFound(new { Message = "User Not Found!" });

            return Ok(new { Message = "Login Success!" });
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser([FromBody] User userObj)
        {
            if(userObj==null)
                return BadRequest();

            userObj.Password = PasswordHasher.HashPassword(userObj.Password);
            userObj.Role = "User";
            userObj.Token = "";
            await _authContext.Users.AddAsync(userObj);
            await _authContext.SaveChangesAsync();

            return Ok(new { Message = "User registered" });
        }
    }
}
