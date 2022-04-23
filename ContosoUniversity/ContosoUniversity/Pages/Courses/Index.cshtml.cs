using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Data;
using ContosoUniversity.Models;
using ContosoUniversity.ViewModels;

namespace ContosoUniversity.Pages.Courses
{
    public class IndexModel : PageModel
    {
        private readonly ContosoUniversity.Data.SchoolContext _context;

        public IndexModel(ContosoUniversity.Data.SchoolContext context)
        {
            _context = context;
        }

        ////////////////////////////////////////////////////
        // OPTION #1:
        ////////////////////////////////////////////////////
        public IList<Course> Courses { get; set; }

        public async Task OnGetAsync()
        {
            this.Courses = await _context.Courses
                            .Include(c => c.Department)
                            .AsNoTracking()
                            .ToListAsync();
        }

        ////////////////////////////////////////////////////
        // OPTION #2:
        ////////////////////////////////////////////////////
        //public IList<CourseVM> Courses { get; set; }
        //public async Task OnGetAsync()
        //{
        //    this.Courses = await _context.Courses
        //    .Select(p => new CourseVM
        //    {
        //        CourseID = p.CourseID,
        //        Title = p.Title,
        //        Credits = p.Credits,
        //        DepartmentName = p.Department.Name
        //    }).ToListAsync();
        //}
    }

}
