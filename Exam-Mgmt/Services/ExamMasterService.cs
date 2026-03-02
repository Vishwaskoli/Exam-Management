using Exam_Mgmt.Models;
using System.Data;
using Microsoft.Data.SqlClient;

namespace Exam_Mgmt.Services
{
    public class ExamMasterService : IExamMasterService
    {
        private readonly string _connectionString;

        public ExamMasterService(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        // ================= GET ALL =================
        public async Task<List<ExamMasterModel>> GetAllAsync()
        {
            var result = new List<ExamMasterModel>();

            using SqlConnection con = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("sp_Exam_Master", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", "View");

            await con.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                result.Add(new ExamMasterModel
                {
                    Exam_Id = reader.GetInt32(reader.GetOrdinal("Exam_Id")),
                    Exam_Name = reader.GetString(reader.GetOrdinal("Exam_Name")),
                    Course_Id = reader.GetInt32(reader.GetOrdinal("Course_Id")),
                    Sem_Id = reader.GetInt32(reader.GetOrdinal("Sem_Id")),
                    SubjectIds = reader["Subject_Id"].ToString(),
                    ExamDates = reader["Exam_Date"].ToString(),
                    TotalMarks = reader["Total_Marks"].ToString(),
                    Created_By = reader["Created_By"] as int?,
                    Modified_By = reader["Modified_By"] as int?
                });
            }

            return result;
        }

        // ================= GET BY COURSE + SEM =================
        public async Task<List<ExamMasterModel>> GetByCourseSemAsync(int courseId, int semId)
        {
            var allExams = await GetAllAsync();

            return allExams
                .Where(e => e.Course_Id == courseId && e.Sem_Id == semId)
                .ToList();
        }

        // ================= GET TOTAL MARKS =================
        public async Task<int> GetTotalMarksAsync(int examId)
        {
            var allExams = await GetAllAsync();

            var exam = allExams.FirstOrDefault(e => e.Exam_Id == examId);

            if (exam == null || string.IsNullOrEmpty(exam.TotalMarks))
                return 0;

            // Since View mode aggregates as comma string
            var marks = exam.TotalMarks.Split(',').FirstOrDefault();

            return int.TryParse(marks, out int total) ? total : 0;
        }

        // ================= ADD =================
        public async Task<int> AddAsync(ExamMasterModel model)
        {
            return await ExecuteSpAsync(model, "Add");
        }

        // ================= UPDATE =================
        public async Task<int> UpdateAsync(ExamMasterModel model)
        {
            return await ExecuteSpAsync(model, "Update");
        }

        // ================= DELETE =================
        public async Task<int> DeleteAsync(int examId, int modifiedBy)
        {
            var model = new ExamMasterModel
            {
                Exam_Id = examId,
                Modified_By = modifiedBy
            };

            return await ExecuteSpAsync(model, "Delete");
        }

        // ================= COMMON EXECUTOR =================
        private async Task<int> ExecuteSpAsync(ExamMasterModel model, string mode)
        {
            using SqlConnection con = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("sp_Exam_Master", con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Mode", mode);
            cmd.Parameters.AddWithValue("@Exam_Id", (object)model.Exam_Id ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Exam_Name", (object)model.Exam_Name ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Course_Id", (object)model.Course_Id ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Sem_Id", (object)model.Sem_Id ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@SubjectIds", (object)model.SubjectIds ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ExamDates", (object)model.ExamDates ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@TotalMarks", (object)model.TotalMarks ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Created_By", (object)model.Created_By ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Modified_By", (object)model.Modified_By ?? DBNull.Value);

            await con.OpenAsync();

            return await cmd.ExecuteNonQueryAsync();
        }
    }
}