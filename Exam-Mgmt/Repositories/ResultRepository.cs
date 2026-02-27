using Exam_Mgmt.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Exam_Mgmt.Repositories
{
    public class ResultRepository
    {
        private readonly IConfiguration _config;
        private readonly string _connString;

        public ResultRepository(IConfiguration configuration)
        {
            _config = configuration;
            _connString = _config.GetConnectionString("DefaultConnection");
        }

        // ✅ Get All Present Records
        public async Task<List<Result>> GetAllResultsAsync()
        {
            List<Result> list = new List<Result>();

            using (SqlConnection con = new SqlConnection(_connString))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT * FROM Result_Master WHERE Obsolete='N'", con);

                await con.OpenAsync();
                SqlDataReader dr = await cmd.ExecuteReaderAsync();

                while (await dr.ReadAsync())
                {
                    list.Add(new Result
                    {
                        ResultId = Convert.ToInt32(dr["Res_Id"]),
                        CourseId = Convert.ToInt32(dr["Course_Id"]),
                        SemId = Convert.ToInt32(dr["Sem_Id"]),
                        StudentId = Convert.ToInt32(dr["Student_Id"]),
                        ExamId = Convert.ToInt32(dr["Exam_Id"]),
                        SubjectId = Convert.ToInt32(dr["Subject_Id"]),
                        ObtainedMarks = Convert.ToInt32(dr["Obtained_Marks"]),
                        TotalMarks = Convert.ToInt32(dr["Total_Marks"]),
                        CreatedBy = Convert.ToInt32(dr["Created_By"]),
                        CreatedDate = Convert.ToDateTime(dr["Created_Date"]),
                        Longitude = Convert.ToDecimal(dr["Longitude"]),
                        Latitude = Convert.ToDecimal(dr["Latitude"]),
                        Obsolete = Convert.ToChar(dr["Obsolete"])
                    });
                }
            }
            return list;
        }

        // ✅ Single Method for CRUD
        public async Task<int> ExecuteAsync(Result result, string mode)
        {
            using (SqlConnection con = new SqlConnection(_connString))
            {
                SqlCommand cmd = new SqlCommand("sp_ResultCRUD", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Mode", mode);
                cmd.Parameters.AddWithValue("@Res_Id", result.ResultId);
                cmd.Parameters.AddWithValue("@Course_Id", result.CourseId);
                cmd.Parameters.AddWithValue("@Sem_Id", result.SemId);
                cmd.Parameters.AddWithValue("@Student_Id", result.StudentId);
                cmd.Parameters.AddWithValue("@Exam_Id", result.ExamId);
                cmd.Parameters.AddWithValue("@Subject_Id", result.SubjectId);
                cmd.Parameters.AddWithValue("@Obtained_Marks", result.ObtainedMarks);
                cmd.Parameters.AddWithValue("@Total_Marks", result.TotalMarks);
                cmd.Parameters.AddWithValue("@Created_By", result.CreatedBy);
                cmd.Parameters.AddWithValue("@Modified_By", result.ModifiedBy);
                cmd.Parameters.AddWithValue("@Longitude", result.Longitude ?? 0);
                cmd.Parameters.AddWithValue("@Latitude", result.Latitude ?? 0);

                await con.OpenAsync();

                return await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}