namespace Exam_Mgmt.Models
{
    public class SubjectSemMapping
    {
        public int Sub_Sem_Map_Id { get; set; }
        public int Course_Id { get; set; }
        public int Sub_Id { get; set; }
        public int Sem_Id { get; set; }
        public DateTime Created_Date { get; set; }
        public int Created_By { get; set; }
        public DateTime? Modified_Date { get; set; }
        public int? Modified_By { get; set; }
        public string? Obsolete { get; set; }
        public List<int>? SubjectIds { get; set; }
    }
}
