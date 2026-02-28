using System;

namespace Exam_Mgmt.Models
{
    public class CourseSemMapping
    {
        public int Course_Sem_Map_Id { get; set; }
        public int Course_Id { get; set; }
        public int Sem_Id { get; set; }
        public string? Sem_Name { get; set; }
        public int Created_By { get; set; }
        public DateTime? Created_Date { get; set; }
        public int? Modified_By { get; set; }
        public DateTime? Modified_Date { get; set; }
        public string? Obsolete { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
    }
}