using Microsoft.AspNetCore.Mvc;
using Exam_Mgmt.DAL;

namespace Exam_Mgmt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Top3RankController : ControllerBase
    {
        private readonly Top3RankDAL _dal;

        public Top3RankController(Top3RankDAL dal)
        {
            _dal = dal;
        }

        [HttpGet]
        public IActionResult GetTop3(int? courseId, int? semId, int? subjectId)
        {
            var data = _dal.GetTop3(courseId, semId, subjectId);
            return Ok(data);
        }
    }
}