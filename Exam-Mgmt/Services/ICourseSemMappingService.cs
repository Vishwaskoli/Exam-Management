using Exam_Mgmt.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Exam_Mgmt.Services
{
    public interface ICourseSemMappingService
    {
        Task<List<CourseSemMapping>> GetAll(int? courseId);
        Task<CourseSemMapping?> GetById(int id);
        Task<string> Save(CourseSemMapping model, string mode);
    }
}