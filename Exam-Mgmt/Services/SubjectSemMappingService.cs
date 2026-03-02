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
                    Course_Id = Convert.ToInt32(reader["Course_Id"]),
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
            cmd.Parameters.AddWithValue("@Course_Id", model.Course_Id);

            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }

    // UPDATE
    public void Update(SubjectSemMapping model)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            conn.Open();

            // 1️⃣ Get existing subjects
            var existingSubjects = new List<int>();

            using (SqlCommand getCmd = new SqlCommand(
                "SELECT Sub_Id FROM Subject_Sem_Mapping WHERE Course_Id=@Course_Id AND Sem_Id=@Sem_Id", conn))
            {
                getCmd.Parameters.AddWithValue("@Course_Id", model.Course_Id);
                getCmd.Parameters.AddWithValue("@Sem_Id", model.Sem_Id);

                using (SqlDataReader reader = getCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        existingSubjects.Add(Convert.ToInt32(reader["Sub_Id"]));
                    }
                }
            }

            // 2️⃣ Delete deselected subjects
            var subjectsToDelete = existingSubjects.Except(model.SubjectIds).ToList();

            foreach (var subId in subjectsToDelete)
            {
                using (SqlCommand delCmd = new SqlCommand(
                    "DELETE FROM Subject_Sem_Mapping WHERE Course_Id=@Course_Id AND Sem_Id=@Sem_Id AND Sub_Id=@Sub_Id", conn))
                {
                    delCmd.Parameters.AddWithValue("@Course_Id", model.Course_Id);
                    delCmd.Parameters.AddWithValue("@Sem_Id", model.Sem_Id);
                    delCmd.Parameters.AddWithValue("@Sub_Id", subId);

                    delCmd.ExecuteNonQuery();
                }
            }

            // 3️⃣ Insert newly added subjects
            var subjectsToInsert = model.SubjectIds.Except(existingSubjects).ToList();

            foreach (var subId in subjectsToInsert)
            {
                using (SqlCommand insCmd = new SqlCommand(
                    @"INSERT INTO Subject_Sem_Mapping
                  (Course_Id, Sub_Id, Sem_Id, Created_Date, Created_By)
                  VALUES (@Course_Id, @Sub_Id, @Sem_Id, GETDATE(), @Modified_By)", conn))
                {
                    insCmd.Parameters.AddWithValue("@Course_Id", model.Course_Id);
                    insCmd.Parameters.AddWithValue("@Sem_Id", model.Sem_Id);
                    insCmd.Parameters.AddWithValue("@Sub_Id", subId);
                    insCmd.Parameters.AddWithValue("@Modified_By", model.Modified_By);

                    insCmd.ExecuteNonQuery();
                }
            }
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

    public void DeleteBySemesterAndCourse(int semId, int courseId, int modifiedBy)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            SqlCommand cmd = new SqlCommand("sp_Subject_Sem_Mapping", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Mode", "DeleteBySemesterAndCourse");
            cmd.Parameters.AddWithValue("@Sem_Id", semId);
            cmd.Parameters.AddWithValue("@Course_Id", courseId);
            cmd.Parameters.AddWithValue("@Modified_By", modifiedBy);

            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }
}