using System.ComponentModel.DataAnnotations;

namespace SchoolApp.Models
{
    public enum EnrollmentType
    {
        [Display(Name = "Trial")]
        trial,

        [Display(Name = "Beginner")]
        beginner,

        [Display(Name = "Advanced")]
        advanced
    }

    public class Enrollment
    {
        public int EnrollmentID { get; set; }

        [Display(Name = "Type")]
        public EnrollmentType EnrollmentType { get; set; }

        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime StartDate { get; set; }

        [Display(Name = "End date")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        public int StudentID { get; set; }

        public Student Student { get; set; }
    }
}