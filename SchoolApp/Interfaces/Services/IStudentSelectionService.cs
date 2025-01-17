using Microsoft.AspNetCore.Mvc.Rendering;

namespace SchoolApp.Interfaces.Services;

public interface IStudentSelectionService
{
    Task<SelectList> GetStudentDropdownListAsync(object selectedStudent = null);
}