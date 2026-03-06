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
    }
}