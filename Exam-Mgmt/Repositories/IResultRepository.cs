using Exam_Mgmt.Models;

namespace Exam_Mgmt.Repositories
{
    public interface IResultRepository
    {
        public Task<List<Result>> GetAllAsync();
        public Task<Result?> GetByIdAsync(int id);
        public Task<int> CreateAsync(Result result);
        public Task<int> UpdateAsync(Result result);
        public Task<int> DeleteAsync(int id, int modifiedBy, decimal? latitude, decimal? longitude);
        public Task<int> ExecuteAsync(Result result, string mode);
    }
}