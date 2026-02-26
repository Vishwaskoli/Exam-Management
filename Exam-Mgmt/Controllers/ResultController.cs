using Exam_Mgmt.Models;
using Exam_Mgmt.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Exam_Mgmt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResultController : ControllerBase
    {
        private readonly IResultRepository _repo;

        public ResultController(IResultRepository repo)
        {
            _repo = repo;
        }

        // ================= GET ALL =================
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _repo.GetAllAsync();
            return Ok(data);
        }

        // ================= GET BY ID =================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _repo.GetByIdAsync(id);

            if (data == null)
                return NotFound("Result not found");

            return Ok(data);
        }

        // ================= POST (CREATE / UPDATE / DELETE) =================
        [HttpPost]
        public async Task<IActionResult> Execute(Result result)
        {
            if (string.IsNullOrWhiteSpace(result.Mode))
                return BadRequest("Mode is required (create/update/delete)");

            try
            {
                var response = await _repo.ExecuteAsync(result, result.Mode);

                return Ok(new
                {
                    message = $"Operation '{result.Mode}' executed successfully",
                    result = response
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}