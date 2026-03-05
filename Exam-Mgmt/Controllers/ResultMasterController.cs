using Exam_Mgmt.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Exam_Mgmt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResultMasterController : ControllerBase
    {
        private readonly string _connectionString;

        public ResultMasterController(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        // ================= GET ALL RESULTS =================

        [HttpGet("All")]
        public async Task<IActionResult> GetAll()
        {
            var list = new List<Result>();

            using SqlConnection con = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("sp_ResultCRUD", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", "readall");

            await con.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new Result
                {
                    ResultId = Convert.ToInt32(reader["Res_Id"]),
                    CourseId = Convert.ToInt32(reader["Course_Id"]),
                    SemId = Convert.ToInt32(reader["Sem_Id"]),
                    StudentId = Convert.ToInt32(reader["Student_Id"]),
                    ExamId = Convert.ToInt32(reader["Exam_Id"]),
                    SubjectId = Convert.ToInt32(reader["Subject_Id"]),
                    StudentName = reader["StudentName"].ToString(),
                    ExamName = reader["Exam_Name"].ToString(),
                    SubjectName = reader["Subject_Name"].ToString(),
                    ObtainedMarks = Convert.ToInt32(reader["Obtained_Marks"]),
                    TotalMarks = Convert.ToInt32(reader["Total_Marks"])
                });
            }

            return Ok(list);
        }

        // ================= SAVE RESULT =================

        [HttpPost("Save")]
        public async Task<IActionResult> Save([FromBody] Result result)
        {
            using SqlConnection con = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("sp_ResultCRUD", con);

            cmd.CommandType = CommandType.StoredProcedure;

            string mode = result.ResultId == 0 ? "create" : "update";

            cmd.Parameters.AddWithValue("@Mode", mode);
            cmd.Parameters.AddWithValue("@Res_Id", result.ResultId);
            cmd.Parameters.AddWithValue("@Course_Id", result.CourseId);
            cmd.Parameters.AddWithValue("@Sem_Id", result.SemId);
            cmd.Parameters.AddWithValue("@Student_Id", result.StudentId);
            cmd.Parameters.AddWithValue("@Exam_Id", result.ExamId);
            cmd.Parameters.AddWithValue("@Subject_Id", result.SubjectId);
            cmd.Parameters.AddWithValue("@Obtained_Marks", result.ObtainedMarks);
            cmd.Parameters.AddWithValue("@Total_Marks", result.TotalMarks);
            cmd.Parameters.AddWithValue("@Created_By", result.CreatedBy ?? 1);
            cmd.Parameters.AddWithValue("@Modified_By", result.ModifiedBy ?? 1);
            cmd.Parameters.AddWithValue("@Longitude", result.Longitude ?? 0);
            cmd.Parameters.AddWithValue("@Latitude", result.Latitude ?? 0);

            await con.OpenAsync();
            await cmd.ExecuteNonQueryAsync();

            return Ok(new { message = "Saved successfully" });
        }

        // ================= DELETE RESULT =================

        [HttpPost("Delete")]
        public async Task<IActionResult> Delete([FromBody] Result result)
        {
            using SqlConnection con = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("sp_ResultCRUD", con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Mode", "delete");
            cmd.Parameters.AddWithValue("@Res_Id", result.ResultId);
            cmd.Parameters.AddWithValue("@Modified_By", result.ModifiedBy ?? 1);

            await con.OpenAsync();
            await cmd.ExecuteNonQueryAsync();

            return Ok(new { message = "Deleted successfully" });
        }

        // ================= GET EXAMS BY COURSE & SEM =================

        [HttpGet("GetByCourseSem")]
        public async Task<IActionResult> GetByCourseSem(int courseId, int semId)
        {
            var list = new List<object>();

            using SqlConnection con = new SqlConnection(_connectionString);

            using SqlCommand cmd = new SqlCommand(@"
                SELECT Exam_Id, Exam_Name
                FROM Exam_Master
                WHERE Course_Id=@CourseId
                AND Sem_Id=@SemId
                AND ISNULL(Obsolete,'N')='N'
            ", con);

            cmd.Parameters.AddWithValue("@CourseId", courseId);
            cmd.Parameters.AddWithValue("@SemId", semId);

            await con.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new
                {
                    exam_Id = Convert.ToInt32(reader["Exam_Id"]),
                    exam_Name = reader["Exam_Name"].ToString()
                });
            }

            return Ok(list);
        }

        // ================= GET SUBJECTS FOR EXAM =================


        [HttpGet("GetSubjectsForExam")]
        public async Task<IActionResult> GetSubjectsForExam(int examId)
        {
            try
            {
                var list = new List<object>();

                using SqlConnection con = new SqlConnection(_connectionString);

                using SqlCommand cmd = new SqlCommand(@"
            SELECT 
                Subject_Id,
                Subject_Name
            FROM Subject_Master
            WHERE ISNULL(Obsolete,'N')='N'
        ", con);

                await con.OpenAsync();

                using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    list.Add(new
                    {
                        subjectId = Convert.ToInt32(reader["Subject_Id"]),
                        subjectName = reader["Subject_Name"].ToString(),
                        totalMarks = 100
                    });
                }

                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ================= GET TOTAL MARKS =================

        [HttpGet("GetTotalMarks/{examId}")]
        public async Task<IActionResult> GetTotalMarks(int examId)
        {
            using SqlConnection con = new SqlConnection(_connectionString);

            using SqlCommand cmd = new SqlCommand(@"
                SELECT SUM(Total_Marks) AS Total
                FROM Subject_Master
                WHERE Exam_Id=@ExamId
            ", con);

            cmd.Parameters.AddWithValue("@ExamId", examId);

            await con.OpenAsync();

            var total = await cmd.ExecuteScalarAsync();

            return Ok(total ?? 0);
        }
    }
}