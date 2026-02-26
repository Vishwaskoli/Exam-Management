using Exam_Mgmt.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Exam_Mgmt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LookupController : ControllerBase
    {
        private readonly ILookupRepository _repo;

        public LookupController(ILookupRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("courses")]
        public async Task<IActionResult> GetCourses()
        {
            var data = await _repo.GetCoursesAsync();
            return Ok(data);
        }

        [HttpGet("semesters/{courseId}")]
        public async Task<IActionResult> GetSemesters(int courseId)
        {
            var data = await _repo.GetSemestersByCourseAsync(courseId);
            return Ok(data);
        }

        [HttpGet("students/{courseId}")]
        public async Task<IActionResult> GetStudents(int courseId)
        {
            var data = await _repo.GetStudentsByCourseAsync(courseId);
            return Ok(data);
        }

        [HttpGet("exams/{courseId}/{semId}")]
        public async Task<IActionResult> GetExams(int courseId, int semId)
        {
            var data = await _repo.GetExamsAsync(courseId, semId);
            return Ok(data);
        }
    }
}