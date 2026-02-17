namespace Exam_Mgmt.Models
{
    public class CourseSemMapping
    {
        public int Course_Sem_Map_Id { get; set; }
        public int Course_Id { get; set; }
        public int Sem_Id { get; set; }

        public string? Created_By { get; set; }
        public DateTime? Created_Date { get; set; }

        public string? Modified_By { get; set; }
        public DateTime? Modified_Date { get; set; }
    }
}
