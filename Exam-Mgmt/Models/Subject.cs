using System;

namespace Exam_Mgmt.Models { 
    public class Subject
    {
        public int Subject_Id { get; set; }
        public required string ? Subject_Name { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public DateTime Created_Date { get; set; }
        public int  Created_By { get; set; }
        public  DateTime? Modified_Date { get; set; }
        public int ? Modified_By { get; set; }
        public string ? Obsolete { get; set; }
    }
}

