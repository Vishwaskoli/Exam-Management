using System.Data;
using System.Data.SqlClient;
using Exam_Mgmt.Models;

namespace Exam_Mgmt.Repositories
{
    public class UserRepository
    {
        public string RegisterUser(UserModel user)
        {
            string message = "";

            try
            {
                using (SqlConnection con = new SqlConnection(/* your existing connection string */))
                using (SqlCommand cmd = new SqlCommand("RegisterUser", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Add parameters with explicit SqlDbType (optional but safer)
                    cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 100).Value = user.Name;
                    cmd.Parameters.Add("@Username", SqlDbType.NVarChar, 50).Value = user.Username;
                    cmd.Parameters.Add("@Password", SqlDbType.NVarChar, 50).Value = user.Password;
                    cmd.Parameters.Add("@ConfirmPassword", SqlDbType.NVarChar, 50).Value = user.ConfirmPassword;

                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            message = reader["Message"].ToString();
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                // Log exception if needed
                message = "Database error: " + ex.Message;
            }
            catch (Exception ex)
            {
                message = "Error: " + ex.Message;
            }

            return message;
        }
    }
}