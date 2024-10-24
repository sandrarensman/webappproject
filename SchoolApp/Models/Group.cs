using System.ComponentModel.DataAnnotations;

namespace SchoolApp.Models
{
    public enum Level
    {
        BC,
        VL,
        YY,
        SF,
        CV,
        CM
    }

    public class Group
    {
        public int GroupID { get; set; }

        [Display(Name = "Day")]
        public string Day { get; set; }

        [Display(Name = "Level")]
        public Level Level { get; set; }

        [Display(Name = "Start time")]
        [DataType(DataType.Time)]
        public DateTime StartTime { get; set; }

        [Display(Name = "End time")]
        [DataType(DataType.Time)]
        public DateTime EndTime { get; set; }

        [Display(Name = "Time")]
        public string Time
        {
            get
            {
                return StartTime.ToString("HH:mm") + " - " + EndTime.ToString("HH:mm");
            }
        }

        [Display(Name = "Class")]
        public string Name
        {
            get
            { return Day + " " + Time; }
        }

        public ICollection<Student> Students { get; set; }
    }
}