using Exam_Mgmt.Models;
using Exam_Mgmt.Repositories;
using Exam_Mgmt.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Exam_Mgmt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SemesterMasterController : ControllerBase
    {
        private readonly SemesterMasterService _semesterMasterService;
        public SemesterMasterController(SemesterMasterService semesterMasterService)
        {
            _semesterMasterService = semesterMasterService;
        }
        [HttpGet]
        public async Task<IActionResult> GetSemesters()
        {
            var semester = await _semesterMasterService.GetSemestersAsync();
            return Ok(semester);
        }
        private readonly ISemesterRepository _repo;
        public SemesterMasterController(ISemesterRepository repo)
        {
            _repo = repo;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _repo.GetAllAsync();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _repo.GetByIdAsync(id);
            if (data == null)
                return NotFound();

            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Semester semester)
        {
            var id = await _repo.CreateAsync(semester);
            return Ok(new { NewId = id });
        }
         //✅ GET ALL
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var data = await _repo.GetAllAsync();
            return Ok(data);
        }

        // ✅ POST (Create / Update / Delete)
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Semester semester, [FromQuery] string mode)
        {
            var result = await _repo.ExecuteAsync(semester, mode);

            if (result == 0)
                return BadRequest("Operation Failed");

            return Ok("Success");
        }
    }
}
