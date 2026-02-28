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

        // Get all active exams (Obsolete = 'N')
        public async Task<IEnumerable<ExamMasterModel>> GetAllAsync()
        {
            var result = new List<ExamMasterModel>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_Exam_Master", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Mode", "View");

                await con.OpenAsync();

                using (var reader = await cmd.ExecuteReaderAsync())
                {
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
                }
            }

            return result;
        }

        // Add new exam
        public async Task<int> AddAsync(ExamMasterModel model)
        {
            return await ExecuteSpAsync(model, "Add");
        }

        // Update existing exam
        public async Task<int> UpdateAsync(ExamMasterModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_Exam_Master", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Set parameters
                    cmd.Parameters.AddWithValue("@Mode", "Update");
                    cmd.Parameters.AddWithValue("@Exam_Id", model.Exam_Id);
                    cmd.Parameters.AddWithValue("@Exam_Name", model.Exam_Name ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Course_Id", model.Course_Id ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Sem_Id", model.Sem_Id ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@SubjectIds", model.SubjectIds ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@ExamDates", model.ExamDates ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@TotalMarks", model.TotalMarks ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Modified_By", model.Modified_By ?? (object)DBNull.Value);

                    await conn.OpenAsync();

                    // Execute the SP
                    int rowsAffected = await cmd.ExecuteNonQueryAsync();

                    return rowsAffected;
                }
            }
        }

        // Soft delete exam
        public async Task<int> DeleteAsync(int examId, int modifiedBy)
        {
            var model = new ExamMasterModel
            {
                Mode = "Delete",
                Exam_Id = examId,
                Modified_By = modifiedBy
            };

            return await ExecuteSpAsync(model, "Delete");
        }

        // Common executor
        private async Task<int> ExecuteSpAsync(ExamMasterModel model, string mode)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_Exam_Master", con))
            {
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

                // ExecuteNonQuery returns affected rows
                return await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}