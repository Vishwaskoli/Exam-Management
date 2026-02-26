using Exam_Mgmt.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Exam_Mgmt.Repositories
{
    public class ResultRepository : IResultRepository
    {
        private readonly string _cs;

        public ResultRepository(IConfiguration config)
        {
            _cs = config.GetConnectionString("DefaultConnection");
        }

        public async Task<List<Result>> GetAllAsync()
        {
            var list = new List<Result>();

            using SqlConnection con = new SqlConnection(_cs);
            using SqlCommand cmd = new SqlCommand("sp_ResultCRUD", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", "read");

            await con.OpenAsync();
            using SqlDataReader dr = await cmd.ExecuteReaderAsync();

            while (await dr.ReadAsync())
            {
                list.Add(new Result
                {
                    ResultId = (int)dr["Result_Id"],
                    CourseId = (int)dr["Course"],
                    SemId = (int)dr["Sem_Id"],
                    StudentId = (int)dr["Student_Id"],
                    ExamId = (int)dr["Exam_Id"],
                    SubjectId = (int)dr["Subject_Id"],
                    ObtainedMarks = (int)dr["Obtained_Marks"],
                    Latitude = Convert.ToDecimal(dr["Latitude"]),
                    Longitude = Convert.ToDecimal(dr["Longitude"])
                });
            }

            return list;
        }

        public async Task<Result?> GetByIdAsync(int id)
        {
            using SqlConnection con = new SqlConnection(_cs);
            using SqlCommand cmd = new SqlCommand(
                "SELECT * FROM Result_Master WHERE Result_Id=@id AND Obsolete='N'", con);

            cmd.Parameters.AddWithValue("@id", id);

            await con.OpenAsync();
            using SqlDataReader dr = await cmd.ExecuteReaderAsync();

            if (await dr.ReadAsync())
            {
                return new Result
                {
                    ResultId = (int)dr["Result_Id"],
                    CourseId = (int)dr["Course"],
                    SemId = (int)dr["Sem_Id"],
                    StudentId = (int)dr["Student_Id"],
                    ExamId = (int)dr["Exam_Id"],
                    SubjectId = (int)dr["Subject_Id"],
                    ObtainedMarks = (int)dr["Obtained_Marks"],
                    Latitude = Convert.ToDecimal(dr["Latitude"]),
                    Longitude = Convert.ToDecimal(dr["Longitude"])
                };
            }

            return null;
        }

        public async Task<int> CreateAsync(Result result)
        {
            return await ExecuteAsync(result, "create");
        }

        public async Task<int> UpdateAsync(Result result)
        {
            return await ExecuteAsync(result, "update");
        }

        public async Task<int> DeleteAsync(int id, int modifiedBy, decimal? latitude, decimal? longitude)
        {
            using SqlConnection con = new SqlConnection(_cs);
            using SqlCommand cmd = new SqlCommand("sp_ResultCRUD", con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Mode", "delete");
            cmd.Parameters.AddWithValue("@Result_Id", id);
            cmd.Parameters.AddWithValue("@Modified_By", modifiedBy);
            cmd.Parameters.AddWithValue("@Latitude", latitude ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Longitude", longitude ?? (object)DBNull.Value);

            await con.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();

            return result == null ? 0 : Convert.ToInt32(result);
        }

        public async Task<int> ExecuteAsync(Result result, string mode)
        {
            using SqlConnection con = new SqlConnection(_cs);
            using SqlCommand cmd = new SqlCommand("sp_ResultCRUD", con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Mode", mode);

            cmd.Parameters.AddWithValue("@Result_Id",
                result.ResultId == 0 ? DBNull.Value : result.ResultId);

            cmd.Parameters.AddWithValue("@Course",
                result.CourseId == 0 ? DBNull.Value : result.CourseId);

            cmd.Parameters.AddWithValue("@Sem_Id",
                result.SemId == 0 ? DBNull.Value : result.SemId);

            cmd.Parameters.AddWithValue("@Student_Id",
                result.StudentId == 0 ? DBNull.Value : result.StudentId);

            cmd.Parameters.AddWithValue("@Exam_Id",
                result.ExamId == 0 ? DBNull.Value : result.ExamId);

            cmd.Parameters.AddWithValue("@Subject_Id",
                result.SubjectId == 0 ? DBNull.Value : result.SubjectId);

            cmd.Parameters.AddWithValue("@Obtained_Marks",
                result.ObtainedMarks == 0 ? DBNull.Value : result.ObtainedMarks);

            cmd.Parameters.AddWithValue("@Created_By",
                result.CreatedBy == 0 ? DBNull.Value : result.CreatedBy);

            cmd.Parameters.AddWithValue("@Modified_By",
                result.ModifiedBy == null ? DBNull.Value : result.ModifiedBy);

            cmd.Parameters.AddWithValue("@Latitude",
                result.Latitude ?? (object)DBNull.Value);

            cmd.Parameters.AddWithValue("@Longitude",
                result.Longitude ?? (object)DBNull.Value);

            await con.OpenAsync();

            var dbResult = await cmd.ExecuteScalarAsync();

            return dbResult == null ? 0 : Convert.ToInt32(dbResult);
        }
    }
}