using Exam_Mgmt.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Reflection.Metadata.Ecma335;

namespace Exam_Mgmt.Services
{
    public class CourseMasterService : ICourseMasterService
    {
        private readonly string? cs;
        public CourseMasterService(IConfiguration config)
        {
            cs = config.GetConnectionString("DefaultConnection");
        }

        //public async Task<List<Course>> GetAllCoursesAsync()
        //{
        //    var courses = new List<Course>();
        //    using (SqlConnection conn = new SqlConnection(cs))
        //    {
        //        await conn.OpenAsync();

        //        using (SqlCommand cmd = new SqlCommand(
        //            "sp_GetAllCourse",
        //            conn))
        //        {
        //            using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
        //            {
        //                while (await reader.ReadAsync())
        //                {
        //                    courses.Add(new Course
        //                    {
        //                        Course_Id = Convert.ToInt32(reader["Course_Id"]),
        //                        Course_Name = reader["Course_Name"].ToString(),
        //                        Obsolete = Convert.ToChar(reader["Obsolete"]),
        //                        Created_Date = Convert.ToDateTime(reader["Created_Date"]),
        //                        Created_By = Convert.ToInt32(reader["Created_By"]),
        //                        Modified_Date = reader["Modified_Date"] == DBNull.Value
        //                            ? null
        //                            : Convert.ToDateTime(reader["Modified_Date"]),
        //                        Modified_By = reader["Modified_By"] == DBNull.Value
        //                                      ? null
        //                                      : Convert.ToInt32(reader["Modified_By"]),
        //                        Latitude = reader["Latitude"] == DBNull.Value 
        //                                   ? null
        //                                   : Convert.ToDecimal(reader["Latitude"]),
        //                        Longitude = reader["Longitude"]==DBNull.Value
        //                                   ? null
        //                                   : Convert.ToDecimal(reader["Longitude"])
        //                    });
        //                }
        //            }
        //        }
        //        return courses;
        //    }
        //}

        public async Task<int> CreateCourseAsync(Course c1)
        {
            using (SqlConnection cn = new SqlConnection(cs))
            {
                await cn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("sp_Course_Master", cn))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@mode", SqlDbType.VarChar, 50).Value = "create";
                    cmd.Parameters.Add("@CourseName", SqlDbType.VarChar, 50).Value = c1.Course_Name;
                    cmd.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = c1.Created_By;
                    cmd.Parameters.Add("@Latitude", SqlDbType.Decimal,18).Value = c1.Latitude;
                    cmd.Parameters.Add("@Longitude", SqlDbType.Decimal, 18).Value = c1.Longitude;
                    //cmd.Parameters.Add("@ModifiedBy",

                    int i = await cmd.ExecuteNonQueryAsync();
                    return i;
                }
            }

        }

        public async Task<int> DeleteCourseAsync(int id)
        {
            using (SqlConnection sc = new SqlConnection(cs))
            {
                await sc.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("sp_Course_Master", sc))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@mode", SqlDbType.VarChar, 50).Value = "deletebyid";
                    cmd.Parameters.Add("@CourseId", SqlDbType.Int).Value = id;
                    cmd.Parameters.Add("@ModifyBy", SqlDbType.Int).Value = 1;
                    //cmd.Parameters.Add("@Latitude", SqlDbType.Decimal, 18).Value = lat;
                    //cmd.Parameters.Add("@Longitude", SqlDbType.Decimal, 18).Value = lon;

                    int a = await cmd.ExecuteNonQueryAsync();
                    return a;
                }
            }
        }

        public async Task<List<Course>> GetActiveCourseAsync()
        {
            var Courses = new List<Course>();
            using (SqlConnection sc = new SqlConnection(cs))
            {
                await sc.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("sp_Course_Master", sc))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@mode", SqlDbType.VarChar, 50).Value = "view";

                    using (SqlDataReader rd = cmd.ExecuteReader())
                    {
                        while (await rd.ReadAsync())
                        {
                            Courses.Add(new Course
                            {
                                Course_Id = Convert.ToInt32(rd["Course_Id"]),
                                Course_Name = rd[1].ToString(),
                                Obsolete = Convert.ToChar(rd["Obsolete"]),
                                Modified_Date = rd["Modified_Date"] == DBNull.Value
                                    ? null
                                    : Convert.ToDateTime(rd["Modified_Date"]),
                                Created_By = Convert.ToInt32(rd[4]),
                                Modified_By = rd["Modified_By"] == DBNull.Value
                                              ? null
                                              : Convert.ToInt32(rd["Modified_By"]),
                                Latitude = rd["Latitude"] == DBNull.Value
                                           ? null
                                           : Convert.ToDecimal(rd["Latitude"]),
                                Longitude = rd["Longitude"] == DBNull.Value
                                           ? null
                                           : Convert.ToDecimal(rd["Longitude"])
                            });
                        }
                    }

                }
                return Courses;
            }
        }

        public async Task<int> UpdateCourseAsync(int id, Course c)
        {
            using (SqlConnection sc = new SqlConnection(cs))
            {
                await sc.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("sp_Course_Master", sc))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@mode", SqlDbType.VarChar, 50).Value = "update";
                    cmd.Parameters.Add("@CourseName", SqlDbType.VarChar, 50).Value = c.Course_Name;
                    cmd.Parameters.Add("@ModifyBy", SqlDbType.Int).Value = c.Modified_By;
                    cmd.Parameters.Add("@CourseId", SqlDbType.Int).Value = id;
                    cmd.Parameters.Add("@Latitude", SqlDbType.Decimal,18).Value = c.Latitude;
                    cmd.Parameters.Add("@Longitude", SqlDbType.Decimal, 18).Value = c.Longitude;

                    int a = await cmd.ExecuteNonQueryAsync();
                    return a;
                }
            }
        }
    }
}
