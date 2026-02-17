using Exam_Mgmt.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Exam_Mgmt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseMasterController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public CourseMasterController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> GetCourses()
        {
            var courses = new List<Course>();

            using (SqlConnection conn = new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection")))
            {
                await conn.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Course_Master", conn))
                {
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            courses.Add(new Course
                            {
                                Course_Id = Convert.ToInt32(reader["Course_Id"]),
                                Course_Name = reader["Course_Name"]?.ToString(),
                                Obsolete = reader["Obsolete"]?.ToString(),
                                Created_Date = Convert.ToDateTime(reader["Created_Date"]),
                                Created_By = reader["Created_By"]?.ToString(),
                                Modified_Date = reader["Modified_Date"] == DBNull.Value
                                    ? null
                                    : Convert.ToDateTime(reader["Modified_Date"]),
                                Modified_By = reader["Modified_By"]?.ToString()
                            });
                        }
                    }
                }
            }

            return Ok(courses);
        }
    }
}
