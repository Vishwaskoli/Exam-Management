using Microsoft.AspNetCore.Mvc;
using Exam_Mgmt.Models;
using Exam_Mgmt.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Exam_Mgmt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserRepository _userRepo;

        public UserController(UserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        [HttpPost("register")]
        public IActionResult Register(UserModel user)
        {
            var result = _userRepo.RegisterUser(user);
            return Ok(result);
        }
        [HttpGet("check")]
        public IActionResult CheckLogin()
        {
            //foreach (var cookie in Request.Cookies)
            //{
            //    Console.WriteLine($"{cookie.Key} : {cookie.Value}");
            //}
            var username = Request.Cookies["username"];

            if (string.IsNullOrEmpty(username))
                return Unauthorized("Not logged in");

            return Ok(new { username });
        }
        [HttpGet("profile")]
        public IActionResult GetProfile()
        {
            var username = Request.Cookies["username"];

            if (username == null)
                return Unauthorized("Not logged in");

            return Ok($"Welcome {username}");
        }
        // LOGIN API
        [HttpPost("login")]
        public IActionResult Login(LoginModel user)
        {
            var result = _userRepo.LoginUser(user);

            if (result != "Login Successful")
                return Unauthorized(result);

            var claims = new[]
            {
        new Claim(ClaimTypes.Name, user.Username)
    };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("THIS_IS_SECRET_KEY_FOR_JWT_TOKEN"));

            var creds = new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new
            {
                message = "Login Successful",
                token = jwt
            });
        }

        // LOGOUT API
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("username");
            return Ok("Logout Successful");
        }
    }
}