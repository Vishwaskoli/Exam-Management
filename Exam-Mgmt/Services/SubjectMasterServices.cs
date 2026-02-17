namespace Exam_Mgmt.Services
{
    public class SubjectMasterServices
    {
        private readonly string cs;
        public SubjectMasterServices(IConfiguration config)
        {
            cs = config.GetConnectionString("DefaultConnection");
        }
    }
}
