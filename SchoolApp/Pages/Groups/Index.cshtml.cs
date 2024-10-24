using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.Models;
using SchoolApp.Models.SchoolViewModels;

namespace SchoolApp.Pages.Groups
{
    public class IndexModel(SchoolContext context) : PageModel
    {
        private readonly SchoolContext _context = context;

        public GroupIndexData GroupData { get; set; }
        public int GroupID { get; set; }
        public int StudentID { get; set; }

        public IList<Group> Group { get; set; } = default!;

        public async Task OnGetAsync(int? id)
        {
            GroupData = new GroupIndexData
            {
                Groups = await _context.Groups
                    .Include(g => g.Students)
                    .OrderBy(g => g.Day)
                    .ToListAsync()
            };

            if (id != null)
            {
                GroupID = id.Value;
                Group group = GroupData.Groups
                    .Where(g => g.GroupID == GroupID).Single();
                GroupData.Students = group.Students.OrderBy(s => s.LastName);
            }
        }
    }
}
