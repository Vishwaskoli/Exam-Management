using Exam_Mgmt.Models;
using Exam_Mgmt.Services;
using Microsoft.AspNetCore.Mvc;

namespace Exam_Mgmt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseMasterController : ControllerBase
    {
        private readonly ICourseMasterService _courseMasterService;

        // ✅ Only ONE constructor
        public CourseMasterController(ICourseMasterService service)
        {
            _courseMasterService = service;
        }

        // ================= GET ALL =================
        [HttpGet]
        public async Task<IActionResult> GetCourses()
        {
            var courses = await _courseMasterService.GetAllCoursesAsync();
            return Ok(courses);
        }

        // ================= CREATE =================
        [HttpPost]
        public async Task<IActionResult> CreateCourse([FromBody] Course c1)
        {
            if (c1 == null || string.IsNullOrWhiteSpace(c1.Course_Name))
                return BadRequest("Course Name is required.");

            int a = await _courseMasterService.CreateCourseAsync(c1);

            if (a > 0)
                return Ok("Course created successfully");
            else
                return BadRequest("Course not created");
        }

        // ================= GET ACTIVE =================
        [HttpGet("ActiveCourses")]
        public async Task<IActionResult> GetActiveCourse()
        {
            var courses = await _courseMasterService.GetActiveCourseAsync();
            return Ok(courses);
        }

        // ================= UPDATE =================
        [HttpPost("UpdateCourse/{id}")]
        public async Task<ActionResult> UpdateCourseName([FromBody] Course c, [FromRoute] int id)
        {
            if (c == null || string.IsNullOrWhiteSpace(c.Course_Name))
                return BadRequest("Valid Course Name is required.");

            int a = await _courseMasterService.UpdateCourseAsync(id, c);

            if (a > 0)
                return Ok($"Course Updated Successfully to {c.Course_Name}");
            else
                return BadRequest("Course not updated");
        }

        // ================= DELETE =================
        [HttpPost("DeleteCourse/{id}")]
        public async Task<ActionResult> DeleteCourse([FromRoute] int id)
        {
            int a = await _courseMasterService.DeleteCourseAsync(id);

            if (a > 0)
                return Ok("Course Deleted Successfully");
            else
                return BadRequest("Course not Deleted");
        }

        // ================= GET BY ID =================
        [HttpGet("{id}")]
        public async Task<ActionResult> CourseById(int id)
        {
            Course c = await _courseMasterService.GetCourseByIdAsync(id);

            if (c != null)
                return Ok(c);   // ✅ you forgot to return c
            else
                return NotFound();
        }
    }
}