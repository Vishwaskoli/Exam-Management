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

        // ============================
        // ✅ GET ALL (WITH FILTER)
        // ============================
        public async Task<List<CourseSemMapping>> GetAll(int? courseId)
        {
            var list = new List<CourseSemMapping>();

            try
            {
                using (SqlConnection con = GetConnection())
                using (SqlCommand cmd = new SqlCommand("SP_CourseSemMapping", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Mode", "GETALL");

                    // 🔥 FILTER PARAMETER
                    cmd.Parameters.AddWithValue("@Course_Id",
                        (object)courseId ?? DBNull.Value);

                    await con.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            list.Add(new CourseSemMapping
                            {
                                Course_Sem_Map_Id = Convert.ToInt32(reader["Course_Sem_Map_Id"]),
                                Course_Id = Convert.ToInt32(reader["Course_Id"]),
                                Sem_Id = Convert.ToInt32(reader["Sem_Id"]),
                                Sem_Name = reader["Sem_Name"].ToString(), // 🔥 important

                                Created_By = Convert.ToInt32(reader["Created_By"]),

                                Created_Date = reader["Created_Date"] != DBNull.Value
                                    ? Convert.ToDateTime(reader["Created_Date"])
                                    : (DateTime?)null,

                                Modified_By = reader["Modified_By"] != DBNull.Value
                                    ? Convert.ToInt32(reader["Modified_By"])
                                    : (int?)null,

                                Modified_Date = reader["Modified_Date"] != DBNull.Value
                                    ? Convert.ToDateTime(reader["Modified_Date"])
                                    : (DateTime?)null,

                                Obsolete = reader["Obsolete"] != DBNull.Value
                                    ? reader["Obsolete"].ToString()
                                    : null,

                                Latitude = reader["Latitude"] != DBNull.Value
                                    ? Convert.ToDecimal(reader["Latitude"])
                                    : (decimal?)null,

                                Longitude = reader["Longitude"] != DBNull.Value
                                    ? Convert.ToDecimal(reader["Longitude"])
                                    : (decimal?)null
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAll: {ex.Message}");
            }

            return list;
        }

        // ============================
        // ✅ GET BY ID
        // ============================
        public async Task<CourseSemMapping?> GetById(int id)
        {
            CourseSemMapping? model = null;

            try
            {
                using (SqlConnection con = GetConnection())
                using (SqlCommand cmd = new SqlCommand("SP_CourseSemMapping", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Mode", "GETBYID");
                    cmd.Parameters.AddWithValue("@Course_Sem_Map_Id", id);

                    await con.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            model = new CourseSemMapping
                            {
                                Course_Sem_Map_Id = Convert.ToInt32(reader["Course_Sem_Map_Id"]),
                                Course_Id = Convert.ToInt32(reader["Course_Id"]),
                                Sem_Id = Convert.ToInt32(reader["Sem_Id"]),
                                Sem_Name = reader["Sem_Name"]?.ToString(),

                                Created_By = Convert.ToInt32(reader["Created_By"])
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetById: {ex.Message}");
            }

            return model;
        }

        // ============================
        // ✅ INSERT / UPDATE / DELETE
        // ============================
        public async Task<string> Save(CourseSemMapping model, string mode)
        {
            try
            {
                using (SqlConnection con = GetConnection())
                using (SqlCommand cmd = new SqlCommand("SP_CourseSemMapping", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Mode", mode);
                    cmd.Parameters.AddWithValue("@Course_Sem_Map_Id", model.Course_Sem_Map_Id);
                    cmd.Parameters.AddWithValue("@Course_Id", model.Course_Id);
                    cmd.Parameters.AddWithValue("@Sem_Id", model.Sem_Id);
                    cmd.Parameters.AddWithValue("@Latitude", model.Latitude);
                    cmd.Parameters.AddWithValue("@Longitude", model.Longitude);
                    cmd.Parameters.AddWithValue("@Created_By", model.Created_By);

                    cmd.Parameters.AddWithValue("@Modified_By",
                        model.Modified_By.HasValue
                            ? model.Modified_By.Value
                            : (object)DBNull.Value);

                    await con.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }

                return "Success";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Save ({mode}): {ex.Message}");
                return $"Error: {ex.Message}";
            }
        }
    }
}