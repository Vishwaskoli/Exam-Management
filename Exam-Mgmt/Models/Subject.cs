using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SubjectApi.Models
{
    [Table("Subject_Master")]
    public class Subject
    {
        [Key]
        public int Subject_Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string ? Subject_Name  { get; set; }

        [Required]
        public DateTime Created_Date { get; set; }

        [MaxLength(100)]
        public string ? Created_By { get; set; }

        public DateTime? Modified_Date { get; set; }

        [MaxLength(100)]
        public string ? Modified_By { get; set; }

        [Required]
        [Column(TypeName = "char(1)")]
        public string Obsolete { get; set; } = "N";
    }
}
