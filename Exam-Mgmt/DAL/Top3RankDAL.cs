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
                                Course_Name = dr["Course_Name"]?.ToString(),
                                Sem_Name = dr["Sem_Name"]?.ToString(),
                                Subject_Name = dr["Subject_Name"]?.ToString(),

                                Student_Id = dr["Student_Id"] != DBNull.Value
                    ? Convert.ToInt32(dr["Student_Id"])
                    : 0,

                                 

                                Obtained_Marks = dr["Obtained_Marks"] != DBNull.Value
                    ? Convert.ToInt32(dr["Obtained_Marks"])
                    : 0,

                                Total_Marks = dr["Total_Marks"] != DBNull.Value
                    ? Convert.ToInt32(dr["Total_Marks"])
                    : 0,

                                Percentage = dr["Percentage"] != DBNull.Value
                    ? Convert.ToDecimal(dr["Percentage"])
                    : 0,

                                RankPosition = dr["RankPosition"] != DBNull.Value
                    ? Convert.ToInt32(dr["RankPosition"])
                    : 0
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