using Exam_Mgmt.Models;

namespace Exam_Mgmt.Services
{
    public interface ICourseMasterService
    {
        Task<int> CreateCourseAsync(Course c1);
        Task<int> DeleteCourseAsync(int id);
        Task<List<Course>> GetActiveCourseAsync();
        //Task<List<Course>> GetAllCoursesAsync();
        Task<int> UpdateCourseAsync(int id, Course c);
        Task<Course> GetCourseByIdAsync(int id);
    }
}