using Exam_Mgmt.Models;

namespace Exam_Mgmt.Services
{
    public interface IExamMasterService
    {
        Task<List<ExamMasterModel>> GetAllAsync();
        Task<List<ExamMasterModel>> GetByCourseSemAsync(int courseId, int semId);
        Task<int> GetTotalMarksAsync(int examId);

        Task<int> AddAsync(ExamMasterModel model);
        Task<int> UpdateAsync(ExamMasterModel model);
        Task<int> DeleteAsync(int examId, int modifiedBy);
    }
}