using Exam_Mgmt.Models;
using Exam_Mgmt.Repositories;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class ResultMasterController : ControllerBase
{
    private readonly ResultRepository _repository;

    public ResultMasterController(ResultRepository repo)
    {
        _repository = repo;
    }

    [HttpGet("All")]
    public async Task<IActionResult> GetAll()
    {
        var data = await _repository.GetAllResultsAsync();
        return Ok(data);
    }

    [HttpPost("Save")]
    public async Task<IActionResult> Save([FromBody] Result result)
    {
        string mode = result.ResultId == 0 ? "create" : "update";

        try
        {
            int rows = await _repository.ExecuteAsync(result, mode);
            return Ok(new { message = "Saved successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("Delete")]
    public async Task<IActionResult> Delete([FromBody] Result result)
    {
        int rows = await _repository.ExecuteAsync(result, "delete");
        return Ok(new { message = "Deleted successfully" });
    }

    [HttpGet("Report")]
    public async Task<IActionResult> GetReport(int courseId, int semId)
    {
        var data = await _repository.GetReport(courseId, semId);
        return Ok(data);
    }
}