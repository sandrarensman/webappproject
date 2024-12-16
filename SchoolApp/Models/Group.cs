using System.ComponentModel.DataAnnotations;
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

public class Group
{
    public Group() { }
    
    public Group(string day, TimeSpan startTime, TimeSpan endTime, Level level, List<int> studentIds)
    {
        Day = day;
        StartTime = startTime;
        EndTime = endTime;
        Level = level;
        StudentIds = studentIds;
    }
    
    public int GroupId { get; init; }

    [Display(Name = "Day")]
    [MaxLength(20)]
    public string Day { get; set; }

    [Display(Name = "Level")]
    [JsonConverter(typeof(EnumConverter<Level>))]
    public Level Level { get; set; }

    [Display(Name = "Start time")]
    [TimeSpanFormat]
    [DataType(DataType.Time)]
    public TimeSpan StartTime { get; set; }

    [Display(Name = "End time")]
    [TimeSpanFormat]
    [DataType(DataType.Time)]
    public TimeSpan EndTime { get; set; }

    [Display(Name = "Time")] 
    public string Time => StartTime.ToString(@"hh\:mm") + " - " + EndTime.ToString(@"hh\:mm");

    [Display(Name = "Group")]
    public string Name => Day + " " + Time;

    public ICollection<Student> Students { get; set; }
    
    public List<int> StudentIds { get; init; }
}