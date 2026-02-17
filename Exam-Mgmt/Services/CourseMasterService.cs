namespace Exam_Mgmt.Services
{
    public class CourseMasterService
    {
        private readonly string cs;
        public CourseMasterService(IConfiguration config) 
        { 
            cs = config.GetConnectionString("DefaultConnection");
        }


    }
}
