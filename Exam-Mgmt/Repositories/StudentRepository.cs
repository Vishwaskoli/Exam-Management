using Exam_Mgmt.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Exam_Mgmt.Repositories
{
    public class StudentRepository
    {
        private readonly string _cs;

        public StudentRepository(IConfiguration config)
        {
            _cs = config.GetConnectionString("DefaultConnection");
        }

        public async Task<List<Student>> GetAllStudentsAsync()
        {
            var students = new List<Student>();
            try
            {
                using (SqlConnection sc = new SqlConnection(_cs))
                {
                    await sc.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand("sp_StudentMaster", sc))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Mode", SqlDbType.VarChar, 50).Value = "View";
                        SqlDataReader rd = await cmd.ExecuteReaderAsync();
                        while (await rd.ReadAsync())
                        {
                            students.Add(new Student
                            {
                                Student_Id = Convert.ToInt32(rd["Student_Id"]),
                                Stu_FirstName = rd["Stu_First_Name"]?.ToString(),
                                Stu_MiddleName = rd["Stu_Middle_Name"] == DBNull.Value ? null : rd["Stu_Middle_Name"].ToString(),
                                Stu_LastName = rd["Stu_Last_Name"] == DBNull.Value ? null : rd["Stu_Last_Name"].ToString(),
                                Aadhaar_No = rd["Aadhar_Card"]?.ToString(),
                                CourseId = Convert.ToInt32(rd["Course"]),
                                Phone_No = rd["Phone_No"]?.ToString(),
                                Email = rd["Email"]?.ToString(),
                                Student_Code = rd["Student_Code"]?.ToString(),
                               // Student_Img = rd["Student_Image"] == DBNull.Value ? null : (byte[])rd["Student_Image"],
                                Created_By = Convert.ToInt32(rd["Created_By"]),
                                Created_Date = Convert.ToDateTime(rd["Created_Date"]),
                                Modified_By = rd["Modified_By"] == DBNull.Value ? null : Convert.ToInt32(rd["Modified_By"]),
                                Modified_Date = rd["Modified_Date"] == DBNull.Value ? null : Convert.ToDateTime(rd["Modified_Date"]),
                                Latitude = Convert.ToDecimal(rd["Latitude"]),
                                Longitude = Convert.ToDecimal(rd["Longitude"]),
                                Obsolete = Convert.ToChar(rd["Obsolete"]),
                                DOB = Convert.ToDateTime(rd["DOB"])
                            });
                        }
                        return students;
                    }
                }
            }catch(SqlException ex)
            {
                throw new Exception("Sql Exception");
                throw;
            }
        }

        public async Task<int> ExecuteAsync(Student student, string mode)
        {
            try
            {
                using (SqlConnection sc = new SqlConnection(_cs))
                {
                    await sc.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand("sp_StudentMaster", sc))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Mode", SqlDbType.VarChar, 50).Value = mode;
                        cmd.Parameters.Add("@Stu_Id", SqlDbType.Int).Value = (object)student.Student_Id ?? DBNull.Value;
                        cmd.Parameters.Add("@Stu_First_Name", SqlDbType.VarChar, 50).Value = (object)student.Stu_FirstName ?? DBNull.Value;
                        cmd.Parameters.Add("@Stu_Middle_Name", SqlDbType.VarChar, 50).Value = (object)student.Stu_MiddleName ?? DBNull.Value;
                        cmd.Parameters.Add("@Stu_Last_Name", SqlDbType.VarChar, 50).Value = (object)student.Stu_LastName ?? DBNull.Value;
                        cmd.Parameters.Add("@Aadhar_Card", SqlDbType.VarChar, 12).Value = (object)student.Aadhaar_No ?? DBNull.Value;
                        cmd.Parameters.Add("@Course", SqlDbType.Int).Value = (object)student.CourseId ?? DBNull.Value;
                        cmd.Parameters.Add("@Phone_No", SqlDbType.VarChar, 15).Value = (object)student.Phone_No ?? DBNull.Value;
                        cmd.Parameters.Add("@Email", SqlDbType.VarChar, 50).Value = (object)student.Email ?? DBNull.Value;
                        cmd.Parameters.Add("@Student_Code",SqlDbType.VarChar,15).Value = (object)student.Student_Code ?? DBNull.Value;
                        cmd.Parameters.Add("@Student_Image", SqlDbType.VarBinary,-1).Value = (object)student.Student_Img ?? DBNull.Value;
                        cmd.Parameters.Add("@Created_By", SqlDbType.Int).Value = (object)student.Created_By ?? DBNull.Value;
                        cmd.Parameters.Add("@Modified_By", SqlDbType.Int).Value = (object)student.Modified_By ?? DBNull.Value;
                        cmd.Parameters.Add("@Latitude", SqlDbType.Decimal, 18).Value = (object)student.Latitude ?? DBNull.Value;
                        cmd.Parameters.Add("@Longitude", SqlDbType.Decimal, 18).Value = (object)student.Longitude ?? DBNull.Value;
                        cmd.Parameters.Add("@DOB", SqlDbType.DateTime)
    .Value = student.DOB == default(DateTime)
        ? DBNull.Value
        : student.DOB;

                        if (mode == "Create")
                        {
                            var ans = await cmd.ExecuteScalarAsync();
                            return ans == null ? 0 : Convert.ToInt32(ans);
                        }
                        else
                        {
                            object result = await cmd.ExecuteScalarAsync();
                            return result != null ? Convert.ToInt32(result) : 0;
                        }

                    }
                    
                }
            }catch(SqlException ex)
            {
                if (ex.Number == 2627) 
                    { throw new Exception("Duplicate entry detected"); }
                throw;
            }
            
        }

        public async Task<byte[]?> GetStudentImageAsync(int id)
        {
            using (SqlConnection sc = new SqlConnection(_cs))
            {
                await sc.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("sp_StudentMaster", sc))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Mode", "Details");
                    cmd.Parameters.AddWithValue("@Stu_Id", id);

                    using (SqlDataReader rd = await cmd.ExecuteReaderAsync())
                    {
                        if (await rd.ReadAsync())
                        {
                            return rd["Student_Image"] == DBNull.Value
                                ? null
                                : (byte[])rd["Student_Image"];
                        }
                    }
                }
            }
            return null;
        }
    }
}
