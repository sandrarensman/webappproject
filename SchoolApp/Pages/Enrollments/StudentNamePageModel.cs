using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolApp.Data;
using SchoolApp.Models;
using Microsoft.EntityFrameworkCore;

namespace SchoolApp.Pages.Enrollments
{
    public class StudentNamePageModel : PageModel
    {
        public SelectList StudentNameSL { get; set; }

        public void PopulateStudentsDropDownList(SchoolContext _context,
            object selectedStudent = null)
        {
            var studentsQuery = from s in _context.Students
                                   orderby s.FirstName // Sort by name.
                                   select s;

            StudentNameSL = new SelectList(studentsQuery.AsNoTracking(),
                nameof(Student.StudentID),
                nameof(Student.FullName),
                selectedStudent);
        }
    }
}

