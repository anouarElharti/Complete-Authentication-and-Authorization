using AngularAuthApi.Context;
using AngularAuthApi.Helpers;
using AngularAuthApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.RegularExpressions;

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
            
            // CHECK USERNAME
            if(await CheckUsernameExistAsync(userObj.Username))
                return BadRequest(new {Message = "Username Already Exist!"});

            // CHECK EMAIL UNIQUE
            if (await CheckEmailExistAsync(userObj.Email))
                return BadRequest(new { Message = "Email Alreaddy Exist!" });

            // CHECK PASSWORD STRENGTH
            var pass = CheckPasswordStrength(userObj.Password);
            if (!string.IsNullOrEmpty(pass))
                return BadRequest(new { Message = pass });

            // HASHING THE PASSWORD
            userObj.Password = PasswordHasher.HashPassword(userObj.Password);
            userObj.Role = "User";
            userObj.Token = "";

            // SAVING THE USER TO DB
            await _authContext.Users.AddAsync(userObj);
            await _authContext.SaveChangesAsync();

            return Ok(new { Message = "User registered" });
        }

        private Task<bool> CheckUsernameExistAsync(string username)
        {
            return _authContext.Users.AnyAsync(x => x.Username == username);
        }
        private Task<bool> CheckEmailExistAsync(string email)
        {
            return _authContext.Users.AnyAsync(y => y.Email == email);
        }

        private string CheckPasswordStrength(string password)
        {
            StringBuilder sb = new StringBuilder();
            
            Regex validateGuidRegex = new Regex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$");

            if(!validateGuidRegex.IsMatch(password))
                sb.Append("The Passwor should contain at least one lower case, one upper case and one number." + Environment.NewLine);
            return sb.ToString();
        }
    }
}
