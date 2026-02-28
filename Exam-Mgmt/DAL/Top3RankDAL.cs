using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Exam_Mgmt.Models;

namespace Exam_Mgmt.DAL
{
    public class Top3RankDAL
    {
        private readonly string _connectionString;

        public Top3RankDAL(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<Top3RankModel> GetTop3(int? courseId, int? semId, int? subjectId)
        {
            List<Top3RankModel> list = new List<Top3RankModel>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetTop3Ranks", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Course_Id", (object)courseId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Sem_Id", (object)semId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Subject_Id", (object)subjectId ?? DBNull.Value);

                    con.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Top3RankModel model = new Top3RankModel
                            {
                                Course_Name = dr["Course_Name"].ToString(),
                                Sem_Name = dr["Sem_Name"].ToString(),
                                Subject_Name = dr["Subject_Name"].ToString(),
                                Student_Id = Convert.ToInt32(dr["Student_Id"]),
                                Obtained_Marks = Convert.ToInt32(dr["Obtained_Marks"]),
                                Total_Marks = Convert.ToInt32(dr["Total_Marks"]),
                                Percentage = Convert.ToDecimal(dr["Percentage"]),
                                RankPosition = Convert.ToInt32(dr["RankPosition"])
                            };

                            list.Add(model);
                        }
                    }
                }
            }

            return list;
        }
    }
}