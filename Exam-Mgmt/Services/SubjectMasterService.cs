using Exam_Mgmt.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;

namespace Exam_Mgmt.Services
{
    public class SubjectMasterService
        {
            private readonly string _connectionString;

            public SubjectMasterService(IConfiguration configuration)
            {
                _connectionString = configuration.GetConnectionString("DefaultConnection");
            }

        // GET ALL
        public List<Subject> GetAllSubjects()
        {
            var subjects = new List<Subject>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_Subject_Master", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Mode", "View");

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    subjects.Add(new Subject
                    {
                        Subject_Id = Convert.ToInt32(reader["Subject_Id"]),
                        Subject_Name = reader["Subject_Name"].ToString(),
                        Created_Date = Convert.ToDateTime(reader["Created_Date"]),
                        Created_By = Convert.ToInt32(reader["Created_By"]),
                        Modified_Date = reader["Modified_Date"] == DBNull.Value ? null : (DateTime?)reader["Modified_Date"],
                        Modified_By = reader["Modified_By"] == DBNull.Value ? null : Convert.ToInt32(reader["Modified_By"]),

                        Obsolete = reader["Obsolete"].ToString()
                    });
                }
            }

            return subjects;
        }

        // CREATE
        public void CreateSubject(Subject subject)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_Subject_Master", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Mode", "Add");
                cmd.Parameters.AddWithValue("@Subject_Name", subject.Subject_Name);
                cmd.Parameters.AddWithValue("@Created_By", subject.Created_By);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        //UPDATE
        public void UpdateSubject(Subject subject)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_Subject_Master", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Mode", "Update");
                cmd.Parameters.AddWithValue("@Subject_Id", subject.Subject_Id);
                cmd.Parameters.AddWithValue("@Subject_Name", subject.Subject_Name);
                cmd.Parameters.AddWithValue("@Modified_By", subject.Modified_By);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }


        //DELETE
        public void DeleteSubject(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_Subject_Master", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Mode", "Delete");
                cmd.Parameters.AddWithValue("@Subject_Id", id);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }


    }
}
