using Exam_Mgmt.Models;
using Exam_Mgmt.Services;
using Microsoft.AspNetCore.Mvc;

namespace Exam_Mgmt.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseSemMappingController : ControllerBase
    {
        private readonly ICourseSemMappingService _service;

        public CourseSemMappingController(ICourseSemMappingService service)
        {
            _service = service;
        }

        // GET ALL
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAll();
            return Ok(data);
        }

        // GET BY ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _service.GetById(id);
            return Ok(data);
        }

        // INSERT / UPDATE / DELETE
        [HttpPost("{mode}")]
        public async Task<IActionResult> Save(string mode, [FromBody] CourseSemMapping model)
        {
            var result = await _service.Save(model, mode);
            return Ok(result);
        }
    }
}