using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SchoolApp.Helpers;

namespace SchoolApp.Models;

public enum EnrollmentType
{
    [Display(Name = "Trial")] Trial,

    [Display(Name = "Beginner")] Beginner,

    [Display(Name = "Advanced")] Advanced
}

public class Enrollment
{
    public int EnrollmentId { get; init; }

    [Display(Name = "Type")]
    [JsonConverter(typeof(EnumConverter<EnrollmentType>))]
    public EnrollmentType EnrollmentType { get; init; }

    [Display(Name = "Start date")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
    public DateTime StartDate { get; init; }

    [Display(Name = "End date")]
    [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
    [DataType(DataType.Date)]
    public DateTime? EndDate { get; init; }

    public int StudentId { get; init; }

    public Student Student { get; init; }
}