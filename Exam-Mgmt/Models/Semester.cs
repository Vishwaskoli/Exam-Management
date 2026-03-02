namespace Exam_Mgmt.Models
{
    public class Semester
    {
        public int Sem_Id { get; set; }
        public string? Sem_Name { get; set; }
        public DateTime Created_Date { get; set; }
        public int Created_By { get; set; }
        public DateTime? Modified_Date { get; set; }
        public int? Modified_By { get; set; }
        public string? Obsolete { get; set; }
    }

}
