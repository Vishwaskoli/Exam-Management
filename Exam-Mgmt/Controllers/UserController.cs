using Exam_Mgmt.Models;
using Exam_Mgmt.Repositories;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    UserRepository repo = new UserRepository();

    [HttpPost("register")]
    public IActionResult Register(UserModel user)
    {
        var result = repo.RegisterUser(user);

        return Ok(result);
    }
}