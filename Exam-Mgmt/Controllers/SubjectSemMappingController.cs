using Exam_Mgmt.Filters;
using Exam_Mgmt.Models;
using Exam_Mgmt.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Exam_Mgmt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[AuthFilter]
    public class SubjectSemMappingController : ControllerBase
    {
        private readonly SubjectSemMappingService _service;
        private readonly IConfiguration _configuration;

        //public SubjectSemMappingController(SubjectSemMappingService service)
        //{
        //    _service = service;
        //}
        public SubjectSemMappingController(
    SubjectSemMappingService service,
    IConfiguration configuration)
        {
            _service = service;
            _configuration = configuration;
        }
        // GET: api/SubjectSemMapping
        [HttpGet]
        public ActionResult<List<SubjectSemMapping>> GetAll()
        {
            var data = _service.GetAll();
            return Ok(data);
        }

        // GET: api/SubjectSemMapping/GetSubjects?courseId=1&semId=1
        [HttpGet("GetSubjects")]
        public IActionResult GetSubjects(int courseId, int semId)
        {
            if (courseId <= 0 || semId <= 0)
                return BadRequest("Invalid Course or Semester Id");

            var mappings = _service.GetAll()
                .Where(m => m.Course_Id == courseId
                         && m.Sem_Id == semId
                         && m.Obsolete == "N")
                .ToList();

            var subjects = new List<object>();

            using (SqlConnection con = new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection")))
            {
                con.Open();

                foreach (var map in mappings)
                {
                    using (SqlCommand cmd = new SqlCommand(
                        "SELECT Subject_Id, Subject_Name FROM Subject_Master WHERE Subject_Id=@id AND Obsolete='N'", con))
                    {
                        cmd.Parameters.AddWithValue("@id", map.Sub_Id);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                subjects.Add(new
                                {
                                    subject_Id = reader["Subject_Id"],
                                    subject_Name = reader["Subject_Name"]
                                });
                            }
                        }
                    }
                }
            }

            return Ok(subjects);
        }

        // POST: api/SubjectSemMapping/Create
        [HttpPost("Create")]
        public IActionResult Create([FromBody] SubjectSemMapping model)
        {
            // Basic validation
            if (model == null)
                return BadRequest("Request body is null.");

            if (model.Sub_Id <= 0)
                return BadRequest("Sub_Id must be greater than 0.");

            if (model.Sem_Id <= 0)
                return BadRequest("Sem_Id must be greater than 0.");

            if (model.Course_Id <= 0)
                return BadRequest("Course_Id must be greater than 0.");

            if (model.Created_By <= 0)
                return BadRequest("Created_By must be greater than 0.");

            try
            {
                _service.Create(model); // call your SP
                return Ok(new { Message = "Subject-Semester mapping created successfully" });
            }
            catch (SqlException ex)
            {
                // Handle SQL errors
                return StatusCode(500, new
                {
                    Message = "Database error while creating mapping.",
                    Error = ex.Message,
                    ErrorCode = ex.Number
                });
            }
            catch (Exception ex)
            {
                // Handle general errors
                return StatusCode(500, new
                {
                    Message = "Unexpected error while creating mapping.",
                    Error = ex.Message
                });
            }
        }

        // POST: api/SubjectSemMapping/Update
        [HttpPost("Update")]
        public IActionResult Update([FromBody] SubjectSemMapping model)
        {
            if (model == null ||
                model.Course_Id <= 0 ||
                model.Sem_Id <= 0 ||
                model.Modified_By == null ||
                model.SubjectIds == null)
            {
                return BadRequest("Valid data required.");
            }

            _service.Update(model);

            return Ok(new
            {
                Message = "Subject-Semester mapping updated successfully"
            });
        }

        // POST: api/SubjectSemMapping/Delete/{id}
        [HttpPost("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid Sub_Sem_Map_Id.");

            // For soft delete, you may want Modified_By also.
            // If needed, update your service to accept Modified_By.
            _service.Delete(id, 1); // Replace 1 with actual logged-in user id

            return Ok(new
            {
                Message = "Subject-Semester mapping deleted successfully"
            });
        }

        [HttpPost("DeleteBySemester/{semId}")]
        public IActionResult DeleteBySemester(int semId)
        {
            if (semId <= 0)
                return BadRequest("Invalid Semester Id");

            _service.DeleteBySemester(semId,1);

            return Ok(new { Message = "All mappings for this semester deleted successfully" });
        }

        [HttpPost("DeleteBySemesterAndCourse")]
        public IActionResult DeleteBySemesterAndCourse([FromBody] SubjectSemMapping model)
        {
            _service.DeleteBySemesterAndCourse(
                model.Sem_Id,
                model.Course_Id,
                model.Modified_By ?? 0
            );

            return Ok(new { Message = "Deleted successfully" });
        }

        [HttpGet("TestDelete")]
        public IActionResult TestDelete()
        {
            return Ok("Working");
        }

        [HttpGet("CheckCourseSemester")]
        public IActionResult CheckCourseSemester(int courseId, int semId)
        {
            if (courseId <= 0 || semId <= 0)
                return BadRequest("Invalid Course or Semester ID");

            var exists = _service.GetAll().Any(m => m.Course_Id == courseId && m.Sem_Id == semId);

            return Ok(new { exists });
        }

        // GET: api/SubjectSemMapping/SemesterByCourse/1
        [HttpGet("SemesterByCourse/{courseId}")]
        public IActionResult GetSemesterByCourse(int courseId)
        {
            if (courseId <= 0)
                return BadRequest("Invalid Course Id");

            var semesters = _service.GetAll()
                .Where(x => x.Course_Id == courseId)
                .Select(x => x.Sem_Id)
                .Distinct()
                .ToList();

            return Ok(semesters);
        }


        // GET: api/SubjectSemMapping/SubjectByCourseAndSemester?courseId=1&semId=1
        [HttpGet("SubjectByCourseAndSemester")]
        public IActionResult GetSubjectByCourseAndSemester(int courseId, int semId)
        {
            if (courseId <= 0 || semId <= 0)
                return BadRequest("Invalid Course or Semester Id");

            var subjects = _service.GetAll()
                .Where(x => x.Course_Id == courseId && x.Sem_Id == semId)
                .Select(x => x.Sub_Id)
                .Distinct()
                .ToList();

            return Ok(subjects);
        }
    }
}
