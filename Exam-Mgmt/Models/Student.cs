using System.ComponentModel.DataAnnotations;

namespace Exam_Mgmt.Models
{
    public class Student
    {
        public int Student_Id { get; set; }
        public string Stu_FirstName { get; set; }
        public string? Stu_MiddleName { get; set; }
        public string? Stu_LastName { get; set; }
        public string Email { get; set; }
        public string Aadhaar_No { get; set; }
        public int CourseId { get; set; }
        public string? Phone_No { get; set; }
        public string Student_Code { get; set; }
        public int Created_By { get; set; }
        public DateTime? Created_Date { get; set; }
        public int? Modified_By { get; set; }
        public DateTime? Modified_Date { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public byte[]? Student_Img { get; set; }
        public char? Obsolete { get; set; }
        public DateTime? DOB { get; set; }
    }
}
