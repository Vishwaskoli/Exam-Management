using Exam_Mgmt.DAL;
using Exam_Mgmt.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Exam_Mgmt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AuthFilter]
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
            if (courseId == null || courseId <= 0)
                return BadRequest("Course is required");

            try
            {
                var data = _dal.GetTop3(courseId, semId, subjectId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}