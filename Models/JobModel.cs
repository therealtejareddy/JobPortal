using System.ComponentModel.DataAnnotations;

namespace JobPortal.Models
{
    public class JobModel
    {
        [Key]
        [Required]
        public int JobId { get; set; }
        public string JobTitle { get; set; }
        [Required]
        public string JobDescription { get; set; }
        [Required]
        public string JobCompany { get; set; }
        [Required]
        public int JobSalary { get; set; }
        [Required]
        public string JobMajorSkill { get; set; }

        [Required]
        public CategoryModel Category { get; set; }

        [Required]
        public UserModel User { get; set; }
    }
}
