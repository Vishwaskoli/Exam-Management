using System.ComponentModel.DataAnnotations;

namespace Exam_Mgmt.Models
{
    public class Course
    {
            public int Course_Id { get; set; }
            [Required]
            public string Course_Name { get; set; }
            public char Obsolete { get; set; }
            public DateTime Created_Date { get; set; }
            public int Created_By { get; set; }
            public DateTime? Modified_Date { get; set; }
            public int? Modified_By { get; set; }
    }
}
