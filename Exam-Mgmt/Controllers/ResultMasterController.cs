using Exam_Mgmt.Models;
using Exam_Mgmt.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Exam_Mgmt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResultMasterController : ControllerBase
    {
        private readonly ResultRepository _repository;

        public ResultMasterController(ResultRepository repo)
        {
            _repository = repo;
        }

        // ✅ API 1 → Get All Present Records
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var result = await _repository.GetAllResultsAsync();
            return Ok(result);
        }

        // ✅ API 2 → Single Mode API
        [HttpPost("Execute")]
        public async Task<IActionResult> Execute([FromBody] Result result, [FromQuery] string mode)
        {
            if (string.IsNullOrEmpty(mode))
                return BadRequest("Mode is required");

            if (mode.ToLower() == "create")
                result.CreatedBy = 1;

            if (mode.ToLower() == "update" || mode.ToLower() == "delete")
                result.ModifiedBy = 1;

            int rows = await _repository.ExecuteAsync(result, mode);

            if (rows > 0)
                return Ok($"{mode} successful");

            return BadRequest($"{mode} failed");
        }
    }
}