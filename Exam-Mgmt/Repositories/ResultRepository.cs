using Exam_Mgmt.Models;
using Exam_Mgmt.Models.DTOs;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Exam_Mgmt.Repositories
{
    public class ResultRepository
    {
        private readonly string _connString;

        public ResultRepository(IConfiguration configuration)
        {
            _connString = configuration.GetConnectionString("DefaultConnection");
        }

        private string CalculateGrade(decimal percentage)
        {
            if (percentage >= 75) return "A";
            if (percentage >= 60) return "B";
            if (percentage >= 50) return "C";
            if (percentage >= 40) return "D";
            return "F";
        }

        // ✅ Get All Results
        public async Task<List<ResultListDto>> GetAllResultsAsync()
        {
            List<ResultListDto> list = new();

            using SqlConnection con = new(_connString);
            using SqlCommand cmd = new("sp_ResultCRUD", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", "readall");

            await con.OpenAsync();
            using SqlDataReader dr = await cmd.ExecuteReaderAsync();

            while (await dr.ReadAsync())
            {
                int obtained = Convert.ToInt32(dr["Obtained_Marks"]);
                int total = Convert.ToInt32(dr["Total_Marks"]);
                decimal percentage = total == 0 ? 0 : (obtained * 100m / total);

                list.Add(new ResultListDto
                {
                    ResultId = Convert.ToInt32(dr["Res_Id"]),
                    StudentName = dr["StudentName"].ToString(),
                    ExamName = dr["Exam_Name"].ToString(),
                    SubjectName = dr["Subject_Name"].ToString(),
                    ObtainedMarks = obtained,
                    TotalMarks = total,
                    Percentage = percentage,
                    Grade = CalculateGrade(percentage)
                });
            }

            return list;
        }

        // ✅ Transaction Safe Execute
        public async Task<int> ExecuteAsync(Result result, string mode)
        {
            using SqlConnection con = new(_connString);
            await con.OpenAsync();

            using SqlTransaction transaction = con.BeginTransaction();

            try
            {
                using SqlCommand cmd = new("sp_ResultCRUD", con, transaction);
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

                int rows = await cmd.ExecuteNonQueryAsync();

                transaction.Commit();
                return rows;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        // ✅ Report
        public async Task<List<dynamic>> GetReport(int courseId, int semId)
        {
            List<dynamic> list = new();

            using SqlConnection con = new(_connString);
            using SqlCommand cmd = new(@"
                SELECT 
                    s.Stu_FirstName + ' ' + ISNULL(s.Stu_LastName,'') AS StudentName,
                    SUM(r.Obtained_Marks) AS TotalObtained,
                    SUM(r.Total_Marks) AS TotalMarks,
                    (SUM(r.Obtained_Marks)*100.0/SUM(r.Total_Marks)) AS Percentage
                FROM Result_Master r
                JOIN Student_Master s ON r.Student_Id=s.Student_Id
                WHERE r.Course_Id=@CourseId
                AND r.Sem_Id=@SemId
                AND r.Obsolete='N'
                GROUP BY s.Stu_FirstName, s.Stu_LastName
                ORDER BY Percentage DESC", con);

            cmd.Parameters.AddWithValue("@CourseId", courseId);
            cmd.Parameters.AddWithValue("@SemId", semId);

            await con.OpenAsync();
            SqlDataReader dr = await cmd.ExecuteReaderAsync();

            while (await dr.ReadAsync())
            {
                list.Add(new
                {
                    StudentName = dr["StudentName"].ToString(),
                    TotalObtained = Convert.ToInt32(dr["TotalObtained"]),
                    TotalMarks = Convert.ToInt32(dr["TotalMarks"]),
                    Percentage = Math.Round(Convert.ToDecimal(dr["Percentage"]), 1)
                });
            }

            return list;
        }
    }
}