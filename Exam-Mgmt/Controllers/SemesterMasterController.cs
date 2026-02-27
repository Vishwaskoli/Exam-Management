using Exam_Mgmt.Models;
using Exam_Mgmt.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Exam_Mgmt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SemesterMasterController : ControllerBase
    {
        private readonly ISemesterRepository _repo;

        public SemesterMasterController(ISemesterRepository repo)
        {
            _repo = repo;
        }

        // ✅ GET ALL
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var data = await _repo.GetAllAsync();
            return Ok(data);
        }

        // ✅ POST (Create / Update / Delete)
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Semester semester, [FromQuery] string mode)
        {
            var result = await _repo.ExecuteAsync(semester, mode);

            if (result == 0)
                return BadRequest("Operation Failed");

            return Ok(new
            {
                id = result,
                message = "Success"
            });
        }
    }
}