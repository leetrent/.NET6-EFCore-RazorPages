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
using AutoMapper;

namespace ContosoUniversity.Pages.Courses
{
    public class IndexModel : PageModel
    {
        private readonly ContosoUniversity.Data.SchoolContext _context;
        public readonly IMapper _mapper;

        public IndexModel(ContosoUniversity.Data.SchoolContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        ////////////////////////////////////////////////////
        // OPTION #1:
        ////////////////////////////////////////////////////
        public IList<CourseVM> Courses { get; set; }

        public async Task OnGetAsync()
        {

            IList<Course> courseEntities = await _context.Courses
                                                    .Include(c => c.Department)
                                                    .AsNoTracking()
                                                    .ToListAsync();

            this.Courses = _mapper.Map<List<CourseVM>>(courseEntities);
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
