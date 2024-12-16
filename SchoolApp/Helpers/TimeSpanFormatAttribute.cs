using System.ComponentModel.DataAnnotations;

namespace SchoolApp.Helpers;

public class TimeSpanFormatAttribute : DisplayFormatAttribute
{
    public TimeSpanFormatAttribute()
    {
        DataFormatString = @"{0:hh\:mm}";
        ApplyFormatInEditMode = true;
    }
}