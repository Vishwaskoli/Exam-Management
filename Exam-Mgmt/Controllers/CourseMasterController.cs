using Exam_Mgmt.Models;
using Exam_Mgmt.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

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

        [HttpPost]
        public async Task<IActionResult> CreateCourse([FromBody] Course c1)
        {
            int a = await _courseMasterService.CreateCourseAsync(c1);
            if (a > 0)
                return Ok("Course created succesfully");
            else
                return BadRequest("Course not created, check before submiting...");
        }

        [HttpGet("ActiveCourses")]
        public async Task<IActionResult> GetActiveCourse()
        {
            var courses = await _courseMasterService.GetActiveCourseAsync();
            return Ok(courses);
        }

        [HttpPost("UpdateCourse/{id}")]
        public async Task<ActionResult> UpdateCourseName([FromBody]Course c, [FromRoute]int id)
        {
            int a = await _courseMasterService.UpdateCourseAsync(id,c);
            if (a > 0)
                return Ok($"Course Updated Succesfullly to {c.Course_Name}");
            else
                return BadRequest("Course not updated");
        }

        [HttpPost("DeleteCourse/{id}")]
        public async Task<ActionResult> DeleteCourse([FromRoute] int id)
        {
            int a = await _courseMasterService.DeleteCourseAsync(id);
            if (a > 0)
                return Ok("Course Deleted Successfully");
            else
                return BadRequest("Course not Deleted");
        }
    }
}
