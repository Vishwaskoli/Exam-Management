using Exam_Mgmt.Models;
using Exam_Mgmt.Services;
using Microsoft.AspNetCore.Mvc;

namespace Exam_Mgmt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseSemMappingController : ControllerBase
    {
        private readonly ICourseSemMappingService _service;

        public CourseSemMappingController(ICourseSemMappingService service)
        {
            _service = service;
        }

        // GET
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_service.GetAll());
        }

        // POST (Insert + Update)
        [HttpPost]
        public IActionResult Post(CourseSemMapping model)
        {
            return Ok(_service.Save(model));
        }
    }
}
