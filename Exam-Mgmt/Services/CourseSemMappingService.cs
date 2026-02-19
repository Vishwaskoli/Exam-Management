using Exam_Mgmt.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Exam_Mgmt.Services
{
    public class CourseSemMappingService : ICourseSemMappingService
    {
        private readonly IConfiguration _configuration;

        public CourseSemMappingService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection"));
        }

        // GET ALL
        public async Task<List<CourseSemMapping>> GetAll()
        {
            List<CourseSemMapping> list = new List<CourseSemMapping>();

            using (SqlConnection con = GetConnection())
            {
                SqlCommand cmd = new SqlCommand("SP_CourseSemMapping", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Mode", "GETALL");

                await con.OpenAsync();
                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    list.Add(new CourseSemMapping
                    {
                        Course_Sem_Map_Id = Convert.ToInt32(reader["Course_Sem_Map_Id"]),
                        Course_Id = Convert.ToInt32(reader["Course_Id"]),
                        Sem_Id = Convert.ToInt32(reader["Sem_Id"]),
                        Created_By = Convert.ToInt32(reader["Created_By"]),
                        Created_Date = Convert.ToDateTime(reader["Created_Date"]),
                        Modified_By = reader["Modified_By"] as int?,
                        Modified_Date = reader["Modified_Date"] as DateTime?,
                        Obsolete = reader["Obsolete"].ToString()
                    });
                }
            }

            return list;
        }

        // GET BY ID
        public async Task<CourseSemMapping?> GetById(int id)
        {
            CourseSemMapping? model = null;

            using (SqlConnection con = GetConnection())
            {
                SqlCommand cmd = new SqlCommand("SP_CourseSemMapping", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Mode", "GETBYID");
                cmd.Parameters.AddWithValue("@Course_Sem_Map_Id", id);

                await con.OpenAsync();
                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    model = new CourseSemMapping
                    {
                        Course_Sem_Map_Id = Convert.ToInt32(reader["Course_Sem_Map_Id"]),
                        Course_Id = Convert.ToInt32(reader["Course_Id"]),
                        Sem_Id = Convert.ToInt32(reader["Sem_Id"]),
                        Created_By = Convert.ToInt32(reader["Created_By"]),
                        Created_Date = Convert.ToDateTime(reader["Created_Date"]),
                        Modified_By = reader["Modified_By"] as int?,
                        Modified_Date = reader["Modified_Date"] as DateTime?,
                        Obsolete = reader["Obsolete"].ToString()
                    };
                }
            }

            return model;
        }

        // INSERT / UPDATE / DELETE
        public async Task<string> Save(CourseSemMapping model, string mode)
        {
            using (SqlConnection con = GetConnection())
            {
                SqlCommand cmd = new SqlCommand("SP_CourseSemMapping", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Mode", mode);
                cmd.Parameters.AddWithValue("@Course_Sem_Map_Id", model.Course_Sem_Map_Id);
                cmd.Parameters.AddWithValue("@Course_Id", model.Course_Id);
                cmd.Parameters.AddWithValue("@Sem_Id", model.Sem_Id);
                cmd.Parameters.AddWithValue("@Created_By", model.Created_By);
                cmd.Parameters.AddWithValue("@Modified_By", model.Modified_By ?? (object)DBNull.Value);

                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }

            return "Success";
        }
    }
}
