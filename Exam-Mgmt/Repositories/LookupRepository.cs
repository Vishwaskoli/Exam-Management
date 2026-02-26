using Exam_Mgmt.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Exam_Mgmt.Repositories
{
    public class LookupRepository : ILookupRepository
    {
        private readonly string _cs;

        public LookupRepository(IConfiguration config)
        {
            _cs = config.GetConnectionString("DefaultConnection");
        }

        public async Task<List<Course>> GetCoursesAsync()
        {
            var list = new List<Course>();

            using SqlConnection con = new SqlConnection(_cs);
            using SqlCommand cmd = new SqlCommand(
                "SELECT Course_Id, Course_Name FROM Course_Master WHERE Obsolete='N'",
                con);

            await con.OpenAsync();
            using SqlDataReader dr = await cmd.ExecuteReaderAsync();

            while (await dr.ReadAsync())
            {
                list.Add(new Course
                {
                    Course_Id = (int)dr["Course_Id"],
                    Course_Name = dr["Course_Name"].ToString()
                });
            }

            return list;
        }

        public async Task<List<Semester>> GetSemestersByCourseAsync(int courseId)
        {
            var list = new List<Semester>();

            using SqlConnection con = new SqlConnection(_cs);
            using SqlCommand cmd = new SqlCommand(@"
                SELECT s.Sem_Id, s.Sem_Name
                FROM Semester_Master s
                INNER JOIN Course_Sem_Mapping csm
                    ON csm.Sem_Id = s.Sem_Id
                WHERE csm.Course_Id = @CourseId
                  AND s.Obsolete='N'
                  AND csm.Obsolete='N'", con);

            cmd.Parameters.AddWithValue("@CourseId", courseId);

            await con.OpenAsync();
            using SqlDataReader dr = await cmd.ExecuteReaderAsync();

            while (await dr.ReadAsync())
            {
                list.Add(new Semester
                {
                    Sem_Id = (int)dr["Sem_Id"],
                    Sem_Name = dr["Sem_Name"].ToString()
                });
            }

            return list;
        }

        public async Task<List<Student>> GetStudentsByCourseAsync(int courseId)
        {
            var list = new List<Student>();

            using SqlConnection con = new SqlConnection(_cs);
            using SqlCommand cmd = new SqlCommand(@"
                SELECT Student_Id,
                       Stu_First_Name + ' ' + ISNULL(Stu_Last_Name,'') AS StudentName
                FROM Student_Master
                WHERE Course = @CourseId
                  AND Obsolete='N'", con);

            cmd.Parameters.AddWithValue("@CourseId", courseId);

            await con.OpenAsync();
            using SqlDataReader dr = await cmd.ExecuteReaderAsync();

            while (await dr.ReadAsync())
            {
                list.Add(new Student
                {
                    Student_Id = (int)dr["Student_Id"],
                    StudentName = dr["StudentName"].ToString()
                });
            }

            return list;
        }

        public async Task<List<Exam>> GetExamsAsync(int courseId, int semId)
        {
            var list = new List<Exam>();

            using SqlConnection con = new SqlConnection(_cs);
            using SqlCommand cmd = new SqlCommand(@"
                SELECT Exam_Id, Exam_Name
                FROM Exam_Master
                WHERE Course = @CourseId
                  AND Sem_Id = @SemId
                  AND Obsolete='N'", con);

            cmd.Parameters.AddWithValue("@CourseId", courseId);
            cmd.Parameters.AddWithValue("@SemId", semId);

            await con.OpenAsync();
            using SqlDataReader dr = await cmd.ExecuteReaderAsync();

            while (await dr.ReadAsync())
            {
                list.Add(new Exam
                {
                    Exam_Id = (int)dr["Exam_Id"],
                    Exam_Name = dr["Exam_Name"].ToString()
                });
            }

            return list;
        }
    }
}