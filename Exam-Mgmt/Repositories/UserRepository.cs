using System.Data;
using System.Data.SqlClient;
using Exam_Mgmt.Models;

namespace Exam_Mgmt.Repositories
{
    public class UserRepository
    {
        private readonly string connectionString =
            "Server=WINDOWS-H9I7U7H\\SQLEXPRESS01;Database=Student_Management_System;User Id=sa;Password=bethlehem;TrustServerCertificate=True;";

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