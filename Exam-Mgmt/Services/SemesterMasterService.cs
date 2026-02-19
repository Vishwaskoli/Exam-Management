using Exam_Mgmt.Models;
using Microsoft.Data.SqlClient;

namespace Exam_Mgmt.Services
{
    public class SemesterMasterService
    {
        private readonly string cs;

        public SemesterMasterService(IConfiguration config)
        {
            cs = config.GetConnectionString("DefaultConnection");
        }

        public async Task<List<Semester>> GetSemestersAsync()
        {
            var semesters = new List<Semester>();

            using (SqlConnection conn = new SqlConnection(cs))
            {
                await conn.OpenAsync();

                using (SqlCommand cmd = new SqlCommand(
                    "sp_SemesterCRUD", conn))  // ✅ Bug 1 fixed — double ))
                {
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        //<<<<<<< Updated upstream
                        //=======
                        //<<<<<<< Updated upstream
                        //Sem_Id = Convert.ToInt32(reader["Sem_Id"]);
                        //Sem_Name = reader["Sem_Name"].ToString();
                        //Created_Date = Convert.ToDateTime(reader["Created_Date"]);
                        //Created_By = Convert.ToInt32(reader["Created_By"]);
                        //Modified_Date = reader["Modified_Date"] == DBNull.Value ? null : (DateTime?)reader["Modified_Date"];
                        //Modified_By = reader["Modified_By"] == DBNull.Value ? null : Convert.ToInt32(reader["Modified_By"]);
//=======
//>>>>>>> Stashed changes
                        while (await reader.ReadAsync())
                        {
                            semesters.Add(new Semester
                            {
                                Sem_Id = Convert.ToInt32(reader["Sem_Id"]),  // ✅ Bug 2 fixed
                                Sem_Name = reader["Sem_Name"].ToString(),
                                Created_By = reader["Created_By"].ToString(),
                                Created_Date = Convert.ToDateTime(reader["Created_Date"]),
                                Modified_By = reader["Modified_By"] == DBNull.Value   // ✅ null check for safety
                                               ? null
                                               : reader["Modified_By"].ToString(),
                                Modified_Date = reader["Modified_Date"] == DBNull.Value
                                               ? null
                                               : Convert.ToDateTime(reader["Modified_Date"]),
                                Obsolete = reader["Obsolete"].ToString()
                            });
                        }
                    }
                }
            }  // ✅ Bug 3 fixed — SqlConnection ka closing brace add kiya
//<<<<<<< Updated upstream
//=======
//>>>>>>> Stashed changes
//>>>>>>> Stashed changes

            return semesters;
        }
    }
}