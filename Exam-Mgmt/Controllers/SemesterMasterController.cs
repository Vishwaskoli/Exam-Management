using Exam_Mgmt.Models;
using Exam_Mgmt.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Exam_Mgmt.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    //public class SemesterMasterController : ControllerBase
    //{
    //private readonly SemesterMasterService _semesterMasterService;
    //public SemesterMasterController(SemesterMasterService semesterMasterService)
    //{
    //    _semesterMasterService = semesterMasterService;
    //}
    //[HttpGet]
    //public async Task<IActionResult> GetSemesters()
    //{
    //    var semester = await _semesterMasterService.GetSemestersAsync();
    //    return Ok(semester);
    //}
    //private readonly ISemesterRepository _repo;
    //public SemesterMasterController(ISemesterRepository repo)
    //{
    //    _repo = repo;
    //}
    //[HttpGet]
    //public async Task<IActionResult> GetAll()
    //{
    //    var data = await _repo.GetAllAsync();
    //    return Ok(data);
    //}

    //[HttpGet("{id}")]
    //public async Task<IActionResult> GetById(int id)
    //{
    //    var data = await _repo.GetByIdAsync(id);
    //    if (data == null)
    //        return NotFound();

    //    return Ok(data);
    //}

    //[HttpPost]
    //public async Task<IActionResult> Create(Semester semester)
    //{
    //    var id = await _repo.CreateAsync(semester);
    //    return Ok(new { NewId = id });
    //}
    // ✅ GET ALL
    //[HttpGet]
    //public async Task<IActionResult> Get()
    //{
    //    var data = await _repo.GetAllAsync();
    //    return Ok(data);
    //}

    //// ✅ POST (Create / Update / Delete)
    //[HttpPost]
    //[HttpPost]
    //public async Task<IActionResult> Post([FromBody] Semester semester, [FromQuery] string mode)
    //{
    //    switch (mode)
    //    {
    //        case "Create":
    //            var newId = await _repo.CreateAsync(semester);
    //            return Ok(new
    //            {
    //                success = true,
    //                message = "Semester created successfully",
    //                id = newId
    //            });

    //        case "Read":
    //            var list = await _repo.GetAllAsync();
    //            return Ok(new
    //            {
    //                success = true,
    //                data = list
    //            });

    //        case "ReadById":
    //            var item = await _repo.GetByIdAsync(semester.Sem_Id);
    //            if (item == null)
    //                return NotFound(new { success = false, message = "Not found" });

    //            return Ok(new
    //            {
    //                success = true,
    //                data = item
    //            });

    //        case "Update":
    //            var updated = await _repo.UpdateAsync(semester);
    //            return Ok(new
    //            {
    //                success = updated > 0,
    //                message = updated > 0 ? "Updated successfully" : "Update failed"
    //            });

    //        case "Delete":
    //            var deleted = await _repo.DeleteAsync(semester.Sem_Id, semester.Modified_By ?? 0);
    //            return Ok(new
    //            {
    //                success = deleted > 0,
    //                message = deleted > 0 ? "Deleted successfully" : "Delete failed"
    //            });

    //        default:
    //            return BadRequest(new { success = false, message = "Invalid mode" });
    //    }
    //}
    [Route("api/semesters")]
    [ApiController]
    public class SemesterMasterController : ControllerBase
    {
<<<<<<< Updated upstream
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
=======
>>>>>>> Stashed changes
        private readonly ISemesterRepository _repo;

        public SemesterMasterController(ISemesterRepository repo)
        {
            _repo = repo;
        }
<<<<<<< Updated upstream
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
=======

        // GET - Read All
>>>>>>> Stashed changes
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var data = await _repo.GetAllAsync();
            return Ok(data);
        }

        // POST - Create / Update / Soft Delete / ReadById
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Semester semester, [FromQuery] string mode)
        {
            switch (mode)
            {
                case "Create":
                    var newId = await _repo.CreateAsync(semester);
                    return Ok(new { success = true, id = newId });

                case "Update":
                    var updated = await _repo.UpdateAsync(semester);
                    return Ok(new { success = updated > 0 });

                case "Delete":   // Soft delete
                    var deleted = await _repo.DeleteAsync(semester.Sem_Id, semester.Modified_By ?? 1);
                    return Ok(new { success = deleted > 0 });

                case "ReadById":
                    var item = await _repo.GetByIdAsync(semester.Sem_Id);
                    return Ok(item);

                default:
                    return BadRequest("Invalid mode");
            }
        }
    }

    //}
}
