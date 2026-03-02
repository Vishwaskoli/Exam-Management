using Exam_Mgmt.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Exam_Mgmt.Repositories
{
    public class SemesterRepository : ISemesterRepository
    {
        private readonly string _cs;
        public SemesterRepository(IConfiguration config)
        {
            _cs = config.GetConnectionString("DefaultConnection");
        }
        public async Task<List<Semester>> GetAllAsync()
        {
            var list = new List<Semester>();
            using SqlConnection con = new SqlConnection(_cs);
            using SqlCommand cmd = new SqlCommand("sp_SemesterCRUD", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", "Read");
            await con.OpenAsync();
            using SqlDataReader dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync())
            {
                list.Add(new Semester
                {
                    Sem_Id = (int)dr["Sem_Id"],
                    Sem_Name = (string)dr["Sem_Name"],
                    Created_Date = Convert.ToDateTime(dr["Created_Date"]),
                    Obsolete = (string)dr["Obsolete"]
                });
            }
            return list;
        }
        public async Task<Semester?> GetByIdAsync(int id)
        {
            using SqlConnection con = new SqlConnection(_cs);
            using SqlCommand cmd = new SqlCommand("sp_SemesterCRUD", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", "ReadById");
            cmd.Parameters.AddWithValue("@Sem_Id", id);

            await con.OpenAsync();
            using SqlDataReader dr = await cmd.ExecuteReaderAsync();
            if (await dr.ReadAsync())
            {
                return new Semester
                {
                    Sem_Id = (int)dr["Sem_Id"],
                    Sem_Name = (string)dr["Sem_Name"]
                };
            }
            return null;
        }
        public async Task<int> CreateAsync(Semester semester)
        {
            using SqlConnection con = new SqlConnection(_cs);
            using SqlCommand cmd = new SqlCommand("sp_SemesterCRUD", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", "Create");
            cmd.Parameters.AddWithValue("@Sem_Name", semester.Sem_Name);
            cmd.Parameters.AddWithValue("@Created_By", semester.Created_By);
            await con.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();
            return (int)result;
        }
        public async Task<int> UpdateAsync(Semester semester)
        {
            using SqlConnection conn = new SqlConnection(_cs);
            using SqlCommand cmd = new SqlCommand("sp_SemesterCRUD", conn);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", "Update");
            cmd.Parameters.AddWithValue("@Sem_Id", semester.Sem_Id);
            cmd.Parameters.AddWithValue("@Sem_Name", semester.Sem_Name);
            cmd.Parameters.AddWithValue("@Modified_By", 1);

            await conn.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();

            return Convert.ToInt32(result);
        }

        public async Task<int> DeleteAsync(int id, int modifiedBy)
        {
            using SqlConnection conn = new SqlConnection(_cs);
            using SqlCommand cmd = new SqlCommand("sp_SemesterCRUD", conn);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", "Delete");
            cmd.Parameters.AddWithValue("@Sem_Id", id);
            cmd.Parameters.AddWithValue("@Modified_By", modifiedBy);

            await conn.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();

            return Convert.ToInt32(result);
        }
        public async Task<int> ExecuteAsync(Semester semester, string mode)
        {
            using SqlConnection conn = new SqlConnection(_cs);
            using SqlCommand cmd = new SqlCommand("sp_SemesterCRUD", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Mode", mode);
            cmd.Parameters.AddWithValue("@Sem_Id", semester.Sem_Id == 0 ? DBNull.Value : semester.Sem_Id);
            cmd.Parameters.AddWithValue("@Sem_Name", semester.Sem_Name ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Created_By", semester.Created_By);
            cmd.Parameters.AddWithValue("@Modified_By", semester.Modified_By);

            await conn.OpenAsync();

            var result = await cmd.ExecuteScalarAsync();

            return result == null ? 0 : Convert.ToInt32(result);
        }

    }
}
