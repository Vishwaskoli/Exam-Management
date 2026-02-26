using Exam_Mgmt.Models;

namespace Exam_Mgmt.Repositories
{
    public interface ILookupRepository
    {
        Task<List<Course>> GetCoursesAsync();
        Task<List<Semester>> GetSemestersByCourseAsync(int courseId);
        Task<List<Student>> GetStudentsByCourseAsync(int courseId);
        Task<List<Exam>> GetExamsAsync(int courseId, int semId);
    }
}