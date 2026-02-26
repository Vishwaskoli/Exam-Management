using Exam_Mgmt.Models;
using Exam_Mgmt.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Exam_Mgmt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly StudentRepository _repository;
        public StudentController(StudentRepository repo) 
        {
            _repository = repo;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var result = await _repository.GetAllStudentsAsync();
            return Ok(result);
        }

        [HttpGet("image/{id}")]
        public async Task<IActionResult> GetStudentImage(int id)
        {
            var imageBytes = await _repository.GetStudentImageAsync(id);

            if (imageBytes == null)
                return NotFound();

            return File(imageBytes, "image/jpeg");
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromForm] Student stu,IFormFile? image)
        {
            if (image != null)
            {
                using var ms = new MemoryStream();
                await image.CopyToAsync(ms);
                stu.Student_Img = ms.ToArray();
            }
            stu.Created_By = 1;
            int newId = await _repository.ExecuteAsync(stu,"Create");

            if (newId > 0)
                return Ok(new { Message = "Student Created", Student_Id = newId });

            return BadRequest("Insert failed");
        }

        [HttpPost("Update")]
        public async Task<ActionResult> Update([FromForm] Student stu,IFormFile? image)
        {
            if (stu.Student_Id <= 0)
                return BadRequest("Invalid Student Id");

            if (image != null)
            {
                using var ms = new MemoryStream();
                await image.CopyToAsync(ms);
                stu.Student_Img = ms.ToArray();
            }
            else
            {
                // IMPORTANT: keep existing image
                stu.Student_Img = null;
                // You must modify SP to handle this properly (explained below)
            }

            stu.Modified_By = 1;
            int res = await _repository.ExecuteAsync(stu, "Update");
            if (res > 0)
            {
                return Ok(new { Message="Student Updated succesfully" });
            }
            return NotFound("Student not found");
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody]Student student)
        {
            if (student.Student_Id <= 0)
                return BadRequest("Invalid Student Id");

            student.Modified_By = 1;

            int rows = await _repository.ExecuteAsync(student, "Delete");

            if (rows > 0)
                return Ok("Student Deleted");

            return NotFound("Student not found");
        }
    }
}

