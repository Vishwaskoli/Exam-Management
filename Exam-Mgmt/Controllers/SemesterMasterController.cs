using Exam_Mgmt.Filters;
using Exam_Mgmt.Models;
using Exam_Mgmt.Repositories;
using Exam_Mgmt.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Exam_Mgmt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[AuthFilter]
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
        [HttpGet("ByCourse/{courseId}")]
        public async Task<IActionResult> GetByCourse(int courseId)
        {
            var data = await _repo.GetByCourse(courseId);
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
