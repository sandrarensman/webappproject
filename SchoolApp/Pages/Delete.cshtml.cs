using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.Models;
using System.Data;

namespace SchoolApp.Pages.Students
{
    public class DeleteModel(SchoolContext context,
                            ILogger<DeleteModel> logger) : PageModel
    {
        private readonly SchoolContext _context = context;
        private readonly ILogger<DeleteModel> _logger = logger;

        [BindProperty]
        public Student Student { get; set; }
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id, bool? saveChangesError = false)
        {
            if (id == null || _context.Students == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.StudentID == id);

            if (student == null)
            {
                return NotFound();
            }
            else 
            {
                Student = student;
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ErrorMessage = string.Format("Delete {0} failed. Try again", id);
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Students == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.Groups)
                .SingleAsync(s => s.StudentID == id);

            if (student == null)
            {
                return RedirectToPage("./Index");
            }

            var enrollments = await _context.Enrollments
                .Where(e => e.StudentID == id)
                .ToListAsync();

            try
            {
                foreach (Enrollment enrollment in enrollments)
                {
                    _context.Enrollments.Remove(enrollment);
                }
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, ErrorMessage);

                return RedirectToAction("./Delete",
                        new { id, saveChangesError = true });
            }
        }
    }
}
