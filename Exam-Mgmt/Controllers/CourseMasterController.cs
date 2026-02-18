//using Exam_Mgmt.Models;
//using Exam_Mgmt.Services;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Data.SqlClient;

namespace Exam_Mgmt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseMasterController : ControllerBase
    {
        private readonly CourseMasterService _courseMasterService;

        public CourseMasterController(CourseMasterService service)
        {
            _courseMasterService = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetCourses()
        {
            var courses = await _courseMasterService.GetAllCoursesAsync();
            return Ok(courses);
        }
    }
}
