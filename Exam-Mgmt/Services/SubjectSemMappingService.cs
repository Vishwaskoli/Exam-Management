using Exam_Mgmt.Models;
using Microsoft.Data.SqlClient;
using System.Data;

public class SubjectSemMappingService
{
    private readonly string _connectionString;

    public SubjectSemMappingService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    // VIEW
    public List<SubjectSemMapping> GetAll()
    {
        var list = new List<SubjectSemMapping>();

        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            SqlCommand cmd = new SqlCommand("sp_Subject_Sem_Mapping", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", "View");

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new SubjectSemMapping
                {
                    Sub_Sem_Map_Id = Convert.ToInt32(reader["Sub_Sem_Map_Id"]),
                    Sub_Id = Convert.ToInt32(reader["Sub_Id"]),
                    Sem_Id = Convert.ToInt32(reader["Sem_Id"]),
                    Created_Date = Convert.ToDateTime(reader["Created_Date"]),
                    Created_By = Convert.ToInt32(reader["Created_By"]),
                    Modified_Date = reader["Modified_Date"] == DBNull.Value ? null : (DateTime?)reader["Modified_Date"],
                    Modified_By = reader["Modified_By"] == DBNull.Value ? null : Convert.ToInt32(reader["Modified_By"]),
                    Obsolete = reader["Obsolete"].ToString()
                });
            }
        }

        return list;
    }

    // ADD
    public void Create(SubjectSemMapping model)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            SqlCommand cmd = new SqlCommand("sp_Subject_Sem_Mapping", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Mode", "Add");
            cmd.Parameters.AddWithValue("@Sub_Id", model.Sub_Id);
            cmd.Parameters.AddWithValue("@Sem_Id", model.Sem_Id);
            cmd.Parameters.AddWithValue("@Created_By", model.Created_By);

            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }

    // UPDATE
    public void Update(SubjectSemMapping model)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            SqlCommand cmd = new SqlCommand("sp_Subject_Sem_Mapping", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Mode", "Update");
            cmd.Parameters.AddWithValue("@Sub_Sem_Map_Id", model.Sub_Sem_Map_Id);
            cmd.Parameters.AddWithValue("@Sub_Id", model.Sub_Id);
            cmd.Parameters.AddWithValue("@Sem_Id", model.Sem_Id);
            cmd.Parameters.AddWithValue("@Modified_By", model.Modified_By);

            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }

    // DELETE (Soft Delete)
    public void Delete(int id, int modifiedBy)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            SqlCommand cmd = new SqlCommand("sp_Subject_Sem_Mapping", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Mode", "Delete");
            cmd.Parameters.AddWithValue("@Sub_Sem_Map_Id", id);
            cmd.Parameters.AddWithValue("@Modified_By", modifiedBy);

            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }

    // DELETE ALL MAPPINGS FOR A SEMESTER
    public void DeleteBySemester(int semId, int modifiedBy)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            SqlCommand cmd = new SqlCommand("sp_Subject_Sem_Mapping", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Mode", "DeleteBySemester"); // You need to handle this in your SP
            cmd.Parameters.AddWithValue("@Sem_Id", semId);
            cmd.Parameters.AddWithValue("@Modified_By", modifiedBy);

            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }
}

