namespace Exam_Mgmt.Models
{
    public class Result
    {
        public string? Mode { get; set; }   // create | update | delete

        public int ResultId { get; set; }

        public int CourseId { get; set; }
        public int SemId { get; set; }
        public int StudentId { get; set; }
        public int ExamId { get; set; }
        public int SubjectId { get; set; }

        public int ObtainedMarks { get; set; }

        public int CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }

        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
    }
}