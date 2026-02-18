using Exam_Mgmt.Models;
using System.Collections.Generic;

namespace Exam_Mgmt.Services
{
    public interface ICourseSemMappingService
    {
        List<object> GetAll();
        string Save(CourseSemMapping model);
    }
}
