public class ResultDto
{
    public int ResultId { get; set; }
    public int CourseId { get; set; }
    public int SemId { get; set; }
    public int StudentId { get; set; }
    public int ExamId { get; set; }
    public int SubjectId { get; set; }
    public int ObtainedMarks { get; set; }
    public int TotalMarks { get; set; }
    public decimal Percentage { get; set; }
    public string Grade { get; set; }
}