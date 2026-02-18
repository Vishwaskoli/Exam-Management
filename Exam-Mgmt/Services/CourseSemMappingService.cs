using Exam_Mgmt.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
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

        // ==========================
        // GET
        // ==========================
        public List<object> GetAll()
        {
            List<object> list = new List<object>();

            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                SqlCommand cmd = new SqlCommand("SP_CourseSemMapping", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Mode", "GET");

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    list.Add(new
                    {
                        Course_Sem_Map_Id = Convert.ToInt32(dr["Course_Sem_Map_Id"]),
                        Course_Name = dr["Course_Name"].ToString(),
                        Sem_Name = dr["Sem_Name"].ToString(),
                        Created_By = Convert.ToInt32(dr["Created_By"]),
                        Created_Date = Convert.ToDateTime(dr["Created_Date"])
                    });
                }
            }

            return list;
        }

        // ==========================
        // INSERT + UPDATE
        // ==========================
        public string Save(CourseSemMapping model)
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                SqlCommand cmd = new SqlCommand("SP_CourseSemMapping", con);
                cmd.CommandType = CommandType.StoredProcedure;

                if (model.Course_Sem_Map_Id == 0)
                {
                    cmd.Parameters.AddWithValue("@Mode", "INSERT");
                    cmd.Parameters.AddWithValue("@Course_Id", model.Course_Id);
                    cmd.Parameters.AddWithValue("@Sem_Id", model.Sem_Id);
                    cmd.Parameters.AddWithValue("@Created_By", model.Created_By);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Mode", "UPDATE");
                    cmd.Parameters.AddWithValue("@Course_Sem_Map_Id", model.Course_Sem_Map_Id);
                    cmd.Parameters.AddWithValue("@Course_Id", model.Course_Id);
                    cmd.Parameters.AddWithValue("@Sem_Id", model.Sem_Id);
                    cmd.Parameters.AddWithValue("@Modified_By", model.Modified_By);
                }

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return model.Course_Sem_Map_Id == 0
                ? "Inserted Successfully"
                : "Updated Successfully";
        }
    }
}
