using AngularAuthApi.Context;
using AngularAuthApi.Helpers;
using AngularAuthApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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

            var user = await _authContext.Users.FirstOrDefaultAsync(x => x.Username == userObj.Username);
            
            if (user == null)
                return NotFound(new { Message = "User Not Found!" });

            // VERIFY THE PASSWORD
            if(!PasswordHasher.VerifyPassword(userObj.Password,user.Password))
                return BadRequest(new {Message="Password is incorrect!"});

            user.Token = CreateJwtToken(user);
            return Ok(new 
            {
                Token = user.Token,
                Message = "Login Success!" 
            });
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

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<User>> GetAllUsers()
        {
            return Ok(await _authContext.Users.ToListAsync());
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

        private string CreateJwtToken(User user)
        {
            var JwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("veryveryverysecrettoken......");
            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Name,$"{user.FirstName} {user.LastName}")
            });

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials
            };

            var token = JwtTokenHandler.CreateToken(tokenDescriptor);

            return JwtTokenHandler.WriteToken(token);
        }
    }
}
