using Microsoft.AspNetCore.Mvc;
using Exam_Mgmt.Models;
using Exam_Mgmt.Repositories;

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
            var username = Request.Cookies["username"];

            if (string.IsNullOrEmpty(username))
                return Unauthorized("Not logged in");

            return Ok(new { username });
        }
        // LOGIN API
        [HttpPost("login")]
        public IActionResult Login(UserModel user)
        {
            var result = _userRepo.LoginUser(user);

            if (result == "Login Successful")
            {
                Response.Cookies.Append("username", user.Username, new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTime.Now.AddHours(2)
                });
            }

            return Ok(result);
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