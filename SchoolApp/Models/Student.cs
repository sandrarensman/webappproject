using System.ComponentModel.DataAnnotations;

namespace SchoolApp.Models;

public class Student
{
    public int StudentId { get; init; }

    [Display(Name = "First name")]
    [MaxLength(100)]
    [Required]
    public string FirstName { get; init; }

    [Display(Name = "Prefix")]
    [MaxLength(10)]
    public string Prefix { get; init; }

    [Display(Name = "Last name")]
    [MaxLength(100)]
    [Required]
    public string LastName { get; init; }

    [Display(Name = "Phone number")]
    [MaxLength(20)]
    [RegularExpression(@"^[\d\s]+$", ErrorMessage = "Invalid phone number")]
    public string PhoneNumber { get; init; }

    [Display(Name = "Email")]
    [MaxLength(100)]
    [RegularExpression(@"^[^\s@]+@[^\s@]+\.[^\s@]+$", ErrorMessage = "Invalid email address")]
    public string EmailAddress { get; init; }

    [Display(Name = "Motivation")]
    [MaxLength(400)]
    public string Motivation { get; init; }

    [Display(Name = "Name")]
    public string FullName =>
        string.Join(" ", new[] { FirstName, Prefix, LastName }.Where(s => !string.IsNullOrEmpty(s)));

    [Display(Name = "Groups")] public ICollection<Group> Groups { get; set; }

    [Display(Name = "Enrollments")] public ICollection<Enrollment> Enrollments { get; init; }
}