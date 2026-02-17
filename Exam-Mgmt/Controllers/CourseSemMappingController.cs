using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;


namespace Exam_Mgmt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseSemMappingController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public CourseSemMappingController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        // ===============================
        // GET ALL
        // ===============================
        [HttpGet]
        public IActionResult GetAll()
        {
            List<object> list = new List<object>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_GetAllCourseSemMapping", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new
                    {
                        Courase_Sem_Map_Id = Convert.ToInt32(reader["Courase_Sem_Map_Id"]),
                        Course_Id = Convert.ToInt32(reader["Course_Id"]),
                        Sem_Id = Convert.ToInt32(reader["Sem_Id"]),
                        Created_By = reader["Created_By"].ToString(),
                        Created_Date = Convert.ToDateTime(reader["Created_Date"]),
                        Modified_By = reader["Modified_By"]?.ToString(),
                        Modified_Date = reader["Modified_Date"] as DateTime?
                    });
                }
            }

            return Ok(list);
        }

        // ===============================
        // GET BY ID
        // ===============================
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            object data = null;

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_GetCourseSemMappingById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Courase_Sem_Map_Id", id);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    data = new
                    {
                        Courase_Sem_Map_Id = Convert.ToInt32(reader["Courase_Sem_Map_Id"]),
                        Course_Id = Convert.ToInt32(reader["Course_Id"]),
                        Sem_Id = Convert.ToInt32(reader["Sem_Id"]),
                        Created_By = reader["Created_By"].ToString(),
                        Created_Date = Convert.ToDateTime(reader["Created_Date"]),
                        Modified_By = reader["Modified_By"]?.ToString(),
                        Modified_Date = reader["Modified_Date"] as DateTime?
                    };
                }
            }

            if (data == null)
                return NotFound("Record Not Found");

            return Ok(data);
        }

        // ===============================
        // INSERT (POST)
        // ===============================
        [HttpPost]
        public IActionResult Insert([FromBody] dynamic obj)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_InsertCourseSemMapping", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Course_Id", (int)obj.course_Id);
                cmd.Parameters.AddWithValue("@Sem_Id", (int)obj.sem_Id);
                cmd.Parameters.AddWithValue("@Created_By", (string)obj.created_By);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return Ok("Inserted Successfully");
        }

       
    }
}
