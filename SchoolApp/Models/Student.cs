using System.ComponentModel.DataAnnotations;

namespace SchoolApp.Models
{
    public class Student
    {
        public int StudentID { get; set; }

        [Display(Name = "First name")]
        [Required]
        public string FirstName { get; set; }

        [Display(Name = "Prefix")]
        public string Prefix { get; set; }

        [Display(Name = "Last name")]
        [Required]
        public string LastName { get; set; }

        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Email")]
        public string EmailAddress { get; set; }

        [Display(Name = "Motivation")]
        public string Motivation { get; set; }

        [Display(Name = "Name")]
        public string FullName
        {
            get
            {
                return string.Join(" ", new string[] { FirstName, Prefix, LastName }.Where(s => !string.IsNullOrEmpty(s)));
            }
        }

        [Display(Name = "Classes")]
        public ICollection<Group> Groups { get; set; }

        [Display(Name = "Enrollments")]
        public ICollection<Enrollment> Enrollments { get; set; }
    }
}