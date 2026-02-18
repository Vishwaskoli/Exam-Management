using Exam_Mgmt.Services;
using Microsoft.AspNetCore.Http;
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

            // GET: api/SubjectMaster
            [HttpGet]
            public ActionResult<List<Subject>> GetAll()
            {
                var subjects = _service.GetAllSubjects();
                return Ok(subjects);
            }

            // GET: api/SubjectMaster/5
            [HttpGet("{id}")]
            public ActionResult<Subject> GetById(int id)
            {
                var subject = _service.GetSubjectById(id);
                if (subject == null)
                    return NotFound();

                return Ok(subject);
            }

        // POST: api/SubjectMaster
        [HttpPost]
        public IActionResult Create([FromBody] Subject subject)
        {
            if (subject == null || string.IsNullOrWhiteSpace(subject.Subject_Name) || string.IsNullOrWhiteSpace(subject.Created_By))
                return BadRequest("Subject_Name and Created_By are required");

            int newId = _service.CreateSubject(subject);
            return Ok(new { Subject_Id = newId });
        }

    }
}
