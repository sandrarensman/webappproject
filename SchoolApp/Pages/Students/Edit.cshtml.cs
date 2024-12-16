using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.Helpers;
using SchoolApp.Models;

namespace SchoolApp.Pages.Students;

public class EditModel(DefaultContext context) : StudentGroupsPageModel
{
    private readonly DefaultContext _context = context;

    [BindProperty] public Student Student { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null || _context.Students == null) return NotFound();

        Student = await _context.Students
            .Include(s => s.Groups)
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.StudentId == id);

        if (Student == null) return NotFound();

        PopulateGroupParticipationData(_context, Student);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id, string[] selectedGroups)
    {
        if (id == null) return NotFound();

        var studentToUpdate = await _context.Students
            .Include(s => s.Groups)
            .FirstOrDefaultAsync(s => s.StudentId == id);

        if (studentToUpdate == null) return NotFound();

        if (await TryUpdateModelAsync(
                studentToUpdate,
                "student",
                s => s.FirstName, s => s.Prefix,
                s => s.LastName, s => s.PhoneNumber,
                s => s.EmailAddress, s => s.Motivation))
        {
            UpdateStudentGroups(selectedGroups, studentToUpdate);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }

        UpdateStudentGroups(selectedGroups, studentToUpdate);
        PopulateGroupParticipationData(_context, studentToUpdate);
        return Page();
    }

    public void UpdateStudentGroups(string[] selectedGroups,
        Student studentToUpdate)
    {
        if (selectedGroups == null)
        {
            studentToUpdate.Groups = [];
            return;
        }

        var selectedGroupsHs = new HashSet<string>(selectedGroups);
        var studentGroups = new HashSet<int>
            (studentToUpdate.Groups.Select(g => g.GroupId));
        foreach (var group in _context.Groups)
            if (selectedGroupsHs.Contains(group.GroupId.ToString()))
            {
                if (!studentGroups.Contains(group.GroupId)) studentToUpdate.Groups.Add(group);
            }
            else
            {
                if (studentGroups.Contains(group.GroupId))
                {
                    var groupToRemove = studentToUpdate.Groups.Single(
                        g => g.GroupId == group.GroupId);
                    studentToUpdate.Groups.Remove(groupToRemove);
                }
            }
    }
}