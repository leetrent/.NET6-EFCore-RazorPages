using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ContosoUniversity.Data;
using ContosoUniversity.Models;

namespace ContosoUniversity.Pages.Courses
{
    public class CreateModel : DepartmentNamePageModel // Derives from DepartmentNamePageModel
    {
        private readonly ContosoUniversity.Data.SchoolContext _context;

        public CreateModel(ContosoUniversity.Data.SchoolContext context)
        {
            _context = context;
        }

        //public IActionResult OnGet()
        //{
        //ViewData["DepartmentID"] = new SelectList(_context.Departments, "DepartmentID", "DepartmentID");
        //    return Page();
        //}

        public IActionResult OnGet()
        {
            /////////////////////////////////////////////////////////////
            // Removes ViewData["DepartmentID"].
            // The DepartmentNameSL SelectList is a strongly typed model
            // and will be used by the Razor page.
            // Strongly typed models are preferred over weakly typed. 
            /////////////////////////////////////////////////////////////
            PopulateDepartmentsDropDownList(_context);
            return Page();
        }

        [BindProperty]
        public Course Course { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        //public async Task<IActionResult> OnPostAsync()
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return Page();
        //    }

        //    _context.Courses.Add(Course);
        //    await _context.SaveChangesAsync();

        //    return RedirectToPage("./Index");
        //}

        public async Task<IActionResult> OnPostAsync()
        {
            var emptyCourse = new Course();

            // Uses TryUpdateModelAsync to prevent overposting
            if (await TryUpdateModelAsync<Course>(
                 emptyCourse,
                 "course",   // Prefix for form value.
                 s => s.CourseID, s => s.DepartmentID, s => s.Title, s => s.Credits))
            {
                _context.Courses.Add(emptyCourse);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            /////////////////////////////////////////////////////////////
            // Select DepartmentID if TryUpdateModelAsync fails.
            // Removes ViewData["DepartmentID"].
            // The DepartmentNameSL SelectList is a strongly typed model.
            // and will be used by the Razor page.
            // Strongly typed models are preferred over weakly typed. 
            /////////////////////////////////////////////////////////////
            PopulateDepartmentsDropDownList(_context, emptyCourse.DepartmentID);
            return Page();
        }
    }
}
