using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.Models;

namespace SchoolApp.Helpers;

public class StudentNamePageModel : PageModel
{
    public SelectList StudentNameSelectList { get; set; }

    public void PopulateStudentsDropDownList(DefaultContext context,
        object selectedStudent = null)
    {
        var studentsQuery = from s in context.Students
            orderby s.FirstName
            select s;

        StudentNameSelectList = new SelectList(studentsQuery.AsNoTracking(),
            nameof(Student.StudentId),
            nameof(Student.FullName),
            selectedStudent);
    }
}