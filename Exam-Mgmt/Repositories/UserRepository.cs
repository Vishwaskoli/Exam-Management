using System.Data;
using System.Data.SqlClient;
using Exam_Mgmt.Models;

namespace Exam_Mgmt.Repositories
{
    public class UserRepository
    {
        private readonly string connectionString;

        public UserRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public string RegisterUser(UserModel user)
        {
            string message = "";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("RegisterUser", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Name", user.Name);
                cmd.Parameters.AddWithValue("@Username", user.Username);
                cmd.Parameters.AddWithValue("@Password", user.Password);
                cmd.Parameters.AddWithValue("@ConfirmPassword", user.ConfirmPassword);

                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    message = reader["Message"].ToString();
                }
            }

            return message;
        }
    }
}