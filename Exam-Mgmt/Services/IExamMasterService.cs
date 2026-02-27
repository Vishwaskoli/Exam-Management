using Exam_Mgmt.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Exam_Mgmt.Services
{
    public interface IExamMasterService
    {
        Task<IEnumerable<ExamMasterModel>> GetAllAsync();                             // Calls SP with Mode = 'View'
        Task<int> AddAsync(ExamMasterModel model);                                   // Calls SP with Mode = 'Add'
        Task<int> UpdateAsync(ExamMasterModel model);                                // Calls SP with Mode = 'Update'
        Task<int> DeleteAsync(int examId, int modifiedBy);                           // Calls SP with Mode = 'Delete'
    }
}