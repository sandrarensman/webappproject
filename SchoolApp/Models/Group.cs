using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using SchoolApp.Helpers;

namespace SchoolApp.Models;

public enum Level
{
    [Display(Name = "Beginners")] Beginners,
    [Display(Name = "Tai Chi form")] Form,
    [Display(Name = "Yin-yang form")] YinYang,
    [Display(Name = "Sword form")] SwordForm,
    [Display(Name = "Chi form")] ChiForm,
    [Display(Name = "Centre move")] CentreMove
}

public enum Day
{
    Monday,
    Tuesday,
    Wednesday,
    Thursday,
    Friday,
    Saturday,
    Sunday
}

public class Group
{
    public Group() { }
    
    public Group(Day day, TimeSpan startTime, TimeSpan endTime, Level level, List<int> studentIds)
    {
        Day = day;
        StartTime = startTime;
        EndTime = endTime;
        Level = level;
        StudentIds = studentIds;
    }
    
    public int GroupId { get; init; }

    [Display(Name = "Day")]
    [JsonConverter(typeof(EnumConverter<Day>))]
    [Required]
    public Day Day { get; init; }
    
    [Display(Name = "Level")]
    [JsonConverter(typeof(EnumConverter<Level>))]
    public Level Level { get; init; }

    [Display(Name = "Start time")]
    [TimeSpanFormat]
    [DataType(DataType.Time)]
    public TimeSpan StartTime { get; init; }

    [Display(Name = "End time")]
    [TimeSpanFormat]
    [DataType(DataType.Time)]
    public TimeSpan EndTime { get; init; }

    [Display(Name = "Time")] 
    public string Time => StartTime.ToString(@"hh\:mm") + " - " + EndTime.ToString(@"hh\:mm");

    [Display(Name = "Group")]
    public string Name => Day + " " + Time;

    [JsonIgnore]
    public ICollection<Student> Students { get; set; } = new List<Student>();
    
    [NotMapped]
    public List<int> StudentIds { get; init; } = [];
}