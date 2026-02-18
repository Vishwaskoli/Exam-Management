//using Exam_Mgmt.Models;
//using Microsoft.Data.SqlClient;
//using Microsoft.Extensions.Configuration;
//using System.Reflection.Metadata.Ecma335;

//namespace Exam_Mgmt.Services
//{
//    public class CourseMasterService
//    {
//        private readonly string cs;
//        public CourseMasterService(IConfiguration config) 
//        { 
//            cs = config.GetConnectionString("DefaultConnection");
//        }

//        public async Task<List<Course>> GetAllCoursesAsync()
//        {
//            var courses = new List<Course>();
//            using (SqlConnection conn = new SqlConnection(cs))
//            {
//                await conn.OpenAsync();

//                using (SqlCommand cmd = new SqlCommand(
//                    "SELECT Course_Id, Course_Name, Obsolete, Created_Date, Created_By, Modified_Date, Modified_By FROM dbo.Course_Master",
//                    conn))
//                {
//                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
//                    {
//                        while (await reader.ReadAsync())
//                        {
//                            courses.Add(new Course
//                            {
//                                Course_Id = Convert.ToInt32(reader["Course_Id"]),
//                                Course_Name = reader["Course_Name"]?.ToString(),
//                                Obsolete = reader["Obsolete"].ToString(),
//                                Created_Date = Convert.ToDateTime(reader["Created_Date"]),
//                                Created_By = reader["Created_By"]?.ToString(),
//                                Modified_Date = reader["Modified_Date"] == DBNull.Value
//                                    ? null
//                                    : Convert.ToDateTime(reader["Modified_Date"]),
//                                Modified_By = reader["Modified_By"]?.ToString()
//                            });
//                        }
//                    }
//                }
//                return courses;

//            }
//        }
//    }
//}
