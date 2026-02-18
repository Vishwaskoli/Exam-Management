using Exam_Mgmt.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

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
                SqlCommand cmd = new SqlCommand("sp_GetAllSubjects", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    subjects.Add(new Subject
                    {
                        Subject_Id = Convert.ToInt32(reader["Subject_Id"]),
                        Subject_Name = reader["Subject_Name"].ToString(),
                        Created_Date = Convert.ToDateTime(reader["Created_Date"]),
                        Created_By = reader["Created_By"].ToString(),
                        Modified_Date = reader["Modified_Date"] == DBNull.Value ? null : (DateTime?)reader["Modified_Date"],
                        Modified_By = reader["Modified_By"] == DBNull.Value ? null : reader["Modified_By"].ToString(),
                        Obsolete = reader["Obsolete"].ToString()
                    });
                }
            }

            return subjects;
        }


        // GET BY ID
        public Subject GetSubjectById(int id)
            {
                Subject subject = null;

                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    string query = "SELECT * FROM Subject_Master WHERE Subject_Id = @Subject_Id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Subject_Id", id);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        subject = new Subject
                        {
                            Subject_Id = Convert.ToInt32(reader["Subject_Id"]),
                            Subject_Name = reader["Subject_Name"].ToString(),
                            Created_Date = Convert.ToDateTime(reader["Created_Date"]),
                            Created_By = reader["Created_By"].ToString(),
                            Modified_Date = reader["Modified_Date"] == DBNull.Value ? null : (DateTime?)reader["Modified_Date"],
                            Modified_By = reader["Modified_By"] == DBNull.Value ? null : reader["Modified_By"].ToString(),
                            Obsolete = reader["Obsolete"].ToString()
                        };
                    }
                }

                return subject;
            }

        // CREATE
        public int CreateSubject(Subject subject)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_InsertSubject", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Subject_Name", subject.Subject_Name);
                cmd.Parameters.AddWithValue("@Created_By", subject.Created_By);
                cmd.Parameters.AddWithValue("@Obsolete", subject.Obsolete);

                // Output parameter
                SqlParameter outputIdParam = new SqlParameter("@NewSubjectId", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outputIdParam);

                conn.Open();
                cmd.ExecuteNonQuery();

                return (int)outputIdParam.Value; // return new ID
            }
        }

    }
}
