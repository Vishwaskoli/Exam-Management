using Exam_Mgmt.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Exam_Mgmt.Models;

namespace Exam_Mgmt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectMasterController : ControllerBase
    {
        private readonly SubjectMasterService _service;

        public SubjectMasterController(SubjectMasterService service)
        {
            _service = service;
        }

        // ✅ GET: api/SubjectMaster
        [HttpGet]
        public ActionResult<List<Subject>> GetAll()
        {
            var subjects = _service.GetAllSubjects();
            return Ok(subjects);
        }

        // ✅ POST: api/SubjectMaster
        [HttpPost]
        public IActionResult Create([FromBody] Subject subject)
        {
            if (subject == null || string.IsNullOrWhiteSpace(subject.Subject_Name) || subject.Created_By <= 0)
            {
                return BadRequest("Subject_Name and Created_By are required.");
            }



            _service.CreateSubject(subject);

            return Ok(new
            {
                Message = "Subject created successfully"
            });
        }

        // ✅ PUT: api/SubjectMaster
        [HttpPut]
        public IActionResult Update([FromBody] Subject subject)
        {
            if (subject == null || subject.Subject_Id <= 0)
                return BadRequest("Valid Subject_Id is required.");

            _service.UpdateSubject(subject);

            return Ok(new
            {
                Message = "Subject updated successfully"
            });
        }

        // ✅ DELETE: api/SubjectMaster/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid Subject_Id.");

            _service.DeleteSubject(id);

            return Ok(new
            {
                Message = "Subject deleted successfully"
            });
        }
    }
}
