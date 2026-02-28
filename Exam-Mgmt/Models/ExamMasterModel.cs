namespace Exam_Mgmt.Models
{
    public class ExamMasterModel
    {
        public string Mode { get; set; }                 // Add, Update, Delete, View
        public int? Exam_Id { get; set; }                // Required for Update/Delete
        public string Exam_Name { get; set; }            // Required for Add/Update
        public int? Course_Id { get; set; }              // Required for Add/Update
        public int? Sem_Id { get; set; }                 // Required for Add/Update
        public string SubjectIds { get; set; }           // CSV of subject IDs (Add/Update)
        public string ExamDates { get; set; }            // CSV of exam dates (Add/Update)
        public string TotalMarks { get; set; }           // CSV of marks (Add/Update)
        public int? Created_By { get; set; }             // Required for Add
        public int? Modified_By { get; set; }            // Required for Update/Delete
    }
    public class ExamMasterDeleteModel
    {
        public int Exam_Id { get; set; }
        public int Modified_By { get; set; }
    }
}