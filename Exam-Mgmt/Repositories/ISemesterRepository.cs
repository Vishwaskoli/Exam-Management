using Exam_Mgmt.Models;

namespace Exam_Mgmt.Repositories
{
    public interface ISemesterRepository
    {
        public Task<List<Semester>> GetAllAsync();
        public Task<Semester?> GetByIdAsync(int id);
        public Task<int> CreateAsync(Semester semester);
        public Task<int> UpdateAsync(Semester semester);
        public Task<int> DeleteAsync(int id, int modifiedBy);
        public Task<int> ExecuteAsync(Semester semester, string mode);
    }
}
//s