using Exam_Mgmt.Models;

namespace Exam_Mgmt.Services
{
    public interface ICourseSemMappingService
    {
        Task<List<CourseSemMapping>> GetAll();
        Task<CourseSemMapping?> GetById(int id);
        Task<string> Save(CourseSemMapping model, string mode);
    }
}