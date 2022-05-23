using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Data;
using ContosoUniversity.Models;
using AutoMapper;
using ContosoUniversity.ViewModels;

namespace ContosoUniversity.Pages.Departments
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

        public IList<DepartmentVM> Department { get;set; }

        public async Task OnGetAsync()
        {
            IList<Department> departmentEntities = await _context.Departments
                                                            .Include(d => d.Administrator).ToListAsync();

            this.Department = _mapper.Map<IList<DepartmentVM>>(departmentEntities);
        }
    }
}
