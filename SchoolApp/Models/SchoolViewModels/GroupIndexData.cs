using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolApp.Models.SchoolViewModels
{
    public class GroupIndexData
    {
        public IEnumerable<Group> Groups { get; set; }
        public IEnumerable<Student> Students { get; set; }
    }
}