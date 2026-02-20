using Exam_Mgmt.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Exam_Mgmt.Models;

namespace Exam_Mgmt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectSemMappingController : ControllerBase
    {
        private readonly SubjectSemMappingService _service;

        public SubjectSemMappingController(SubjectSemMappingService service)
        {
            _service = service;
        }

        // GET: api/SubjectSemMapping
        [HttpGet]
        public ActionResult<List<SubjectSemMapping>> GetAll()
        {
            var data = _service.GetAll();
            return Ok(data);
        }

        // POST: api/SubjectSemMapping/Create
        [HttpPost("Create")]
        public IActionResult Create([FromBody] SubjectSemMapping model)
        {
            if (model == null || model.Sub_Id <= 0 || model.Sem_Id <= 0 || model.Created_By <= 0)
                return BadRequest("Sub_Id, Sem_Id and Created_By are required.");

            _service.Create(model);

            return Ok(new
            {
                Message = "Subject-Semester mapping created successfully"
            });
        }

        // POST: api/SubjectSemMapping/Update
        [HttpPost("Update")]
        public IActionResult Update([FromBody] SubjectSemMapping model)
        {
            if (model == null || model.Sub_Sem_Map_Id <= 0 || model.Modified_By == null)
                return BadRequest("Valid Sub_Sem_Map_Id and Modified_By are required.");

            _service.Update(model);

            return Ok(new
            {
                Message = "Subject-Semester mapping updated successfully"
            });
        }

        // POST: api/SubjectSemMapping/Delete/{id}
        [HttpPost("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid Sub_Sem_Map_Id.");

            // For soft delete, you may want Modified_By also.
            // If needed, update your service to accept Modified_By.
            _service.Delete(id, 1); // Replace 1 with actual logged-in user id

            return Ok(new
            {
                Message = "Subject-Semester mapping deleted successfully"
            });
        }

        [HttpPost("DeleteBySemester/{semId}")]
        public IActionResult DeleteBySemester(int semId)
        {
            if (semId <= 0)
                return BadRequest("Invalid Semester Id");

            _service.DeleteBySemester(semId,1);

            return Ok(new { Message = "All mappings for this semester deleted successfully" });
        }
    }
}
