namespace Exam_Mgmt.Models
{
    public class Top3RankModel
    {
        public string Course_Name { get; set; }
        public string Sem_Name { get; set; }
        public string Subject_Name { get; set; }
        public int Student_Id { get; set; }
        public int Obtained_Marks { get; set; }
        public int Total_Marks { get; set; }
        public decimal Percentage { get; set; }
        public int RankPosition { get; set; }
    }
}