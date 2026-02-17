namespace Exam_Mgmt.Models
{
    public class Course
    {
            public int Course_Id { get; set; }
            public string? Course_Name { get; set; }
            public string? Obsolete { get; set; }
            public DateTime Created_Date { get; set; }
            public string? Created_By { get; set; }
            public DateTime? Modified_Date { get; set; }
            public string? Modified_By { get; set; }
    }
}
