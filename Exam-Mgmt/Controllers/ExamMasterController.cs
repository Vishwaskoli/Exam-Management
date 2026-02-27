using Microsoft.AspNetCore.Mvc;
using Exam_Mgmt.Models;
using Exam_Mgmt.Services;
using System.Threading.Tasks;

namespace Exam_Mgmt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamMasterController : ControllerBase
    {
        private readonly IExamMasterService _service;

        public ExamMasterController(IExamMasterService service)
        {
            _service = service;
        }

        // GET: api/ExamMaster
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAllAsync();
            return Ok(data);
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody] ExamMasterDeleteModel model)
        {
            if (model == null)
                return BadRequest("Invalid request.");

            int result = await _service.DeleteAsync(model.Exam_Id, model.Modified_By);
            return Ok(new { message = "Exam deleted successfully (soft delete).", affectedRows = result });
        }

        // POST: api/ExamMaster
        [HttpPost]
        public async Task<IActionResult> Save([FromBody] ExamMasterModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.Mode))
                return BadRequest("Mode is required: Add, Update, or Delete.");

            // Skip automatic validation for Delete mode
            if (model.Mode.ToLower() == "delete")
            {
                ModelState.Clear(); // bypass validation for Delete
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            int result = 0;
            string message = "";

            switch (model.Mode.ToLower())
            {
                //case "delete":
                //    if (model.Exam_Id == null || model.Modified_By == null)
                //        return BadRequest("Exam_Id and Modified_By are required for Delete.");

                //    // Call soft delete
                //    result = await _service.DeleteAsync(model.Exam_Id.Value, model.Modified_By.Value);
                //    message = "Exam deleted successfully (soft delete).";
                //    break;

                case "add":
                    if (string.IsNullOrEmpty(model.Exam_Name) ||
                        string.IsNullOrEmpty(model.SubjectIds) ||
                        string.IsNullOrEmpty(model.ExamDates) ||
                        string.IsNullOrEmpty(model.TotalMarks) ||
                        model.Course_Id == null ||
                        model.Sem_Id == null ||
                        model.Created_By == null)
                    {
                        return BadRequest("Missing required fields for Add.");
                    }
                    result = await _service.AddAsync(model);
                    message = "Exam added successfully.";
                    break;

                case "update":
                    if (model.Exam_Id == null ||
                        string.IsNullOrEmpty(model.Exam_Name) ||
                        string.IsNullOrEmpty(model.SubjectIds) ||
                        string.IsNullOrEmpty(model.ExamDates) ||
                        string.IsNullOrEmpty(model.TotalMarks) ||
                        model.Course_Id == null ||
                        model.Sem_Id == null ||
                        model.Modified_By == null)
                    {
                        return BadRequest("Missing required fields for Update.");
                    }
                    result = await _service.UpdateAsync(model);
                    message = "Exam updated successfully.";
                    break;

                default:
                    return BadRequest("Invalid mode. Allowed values: Add, Update, Delete.");
            }

            return Ok(new { message, affectedRows = result });
        }
    }
}