using System;

namespace Exam_Mgmt.Models { 
    public class Subject
    {
        public int Subject_Id { get; set; }
        public required string ? Subject_Name { get; set; }
        public DateTime Created_Date { get; set; }
        public required string ? Created_By { get; set; }
        public  DateTime? Modified_Date { get; set; }
        public string ? Modified_By { get; set; }
        public string ? Obsolete { get; set; }
    }
}

