using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Exam_Mgmt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectMasterController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public SubjectMasterController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet]
    }
}
