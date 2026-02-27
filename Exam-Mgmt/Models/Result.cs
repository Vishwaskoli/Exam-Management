namespace Exam_Mgmt.Models
{
    public class Result
    {
        public int ResultId { get; set; }
        public int CourseId { get; set; }
        public int SemId { get; set; }
        public int StudentId { get; set; }
        public int ExamId { get; set; }
        public int SubjectId { get; set; }
        public int ObtainedMarks { get; set; }
        public int TotalMarks { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public char? Obsolete { get; set; }
    }
}