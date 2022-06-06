using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Data;
using ContosoUniversity.Models;
using System.Text;

namespace ContosoUniversity.Pages.Departments
{
    public class EditModel : PageModel
    {
        private readonly ContosoUniversity.Data.SchoolContext _context;

        public EditModel(ContosoUniversity.Data.SchoolContext context)
        {
            Console.WriteLine("[Departments][Edit][Constructor] =>");
            _context = context;
        }

        [BindProperty]
        public Department Department { get; set; }

        public SelectList InstructorNameSL { get; set; }


        ///////////////////////////////////////////////////////////////////////////////////////
        // GET
        ///////////////////////////////////////////////////////////////////////////////////////
        public async Task<IActionResult> OnGetAsync(int id)
        {
            string logSnippet = "[Departments][Edit][GET] =>";

            Department = await _context.Departments
                .Include(d => d.Administrator)  // eager loading
                .AsNoTracking()                 // tracking not required
                .FirstOrDefaultAsync(m => m.DepartmentID == id);

            if (Department == null)
            {
                return NotFound();
            }

            string concurrencyTokenString = this.Department.ConcurrencyToken[7].ToString();

            Console.WriteLine();
            Console.WriteLine($"{logSnippet} (this.DepartmentID.DepartmentID).: '{this.Department.DepartmentID}'");
            Console.WriteLine($"{logSnippet} (concurrencyTokenString).........: '{concurrencyTokenString}'");
            Console.WriteLine();

            // Use strongly typed data rather than ViewData.
            InstructorNameSL = new SelectList(_context.Instructors, "ID", "FirstMidName");

            return Page();
        }


        ///////////////////////////////////////////////////////////////////////////////////////
        // POST
        ///////////////////////////////////////////////////////////////////////////////////////
        public async Task<IActionResult> OnPostAsync(int id)
        {
            string logSnippet = "[Departments][Edit][POST] =>";

            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Fetch current department from DB.
            // ConcurrencyToken may have changed.
            var departmentToUpdate = await _context.Departments
                .Include(i => i.Administrator)
                .FirstOrDefaultAsync(m => m.DepartmentID == id);

            if (departmentToUpdate == null)
            {
                return HandleDeletedDepartment();
            }

            string concurrencyTokenString = this.Department.ConcurrencyToken[7].ToString();

            Console.WriteLine();
            Console.WriteLine("-------------------------------------------------------------------------------------------------------");
            Console.WriteLine($"{logSnippet} (this.Department == null)..................: '{this.Department == null}'");
            Console.WriteLine($"{logSnippet} (this.Department.Name == null).............: '{this.Department.Name == null}'");
            Console.WriteLine($"{logSnippet} (this.Department.LastName).................: '{this.Department.Name}'");
            Console.WriteLine($"{logSnippet} (this.Department.ConcurrencyToken == null).: '{this.Department.ConcurrencyToken == null}'");
            Console.WriteLine($"{logSnippet} (this.Department.ConcurrencyToken).........: '{concurrencyTokenString}'");
            Console.WriteLine("-------------------------------------------------------------------------------------------------------");
            Console.WriteLine();


            // Set ConcurrencyToken to value read in OnGetAsync
            _context.Entry(departmentToUpdate).Property(
                 d => d.ConcurrencyToken).OriginalValue = Department.ConcurrencyToken;

            if (await TryUpdateModelAsync<Department>(
                departmentToUpdate,
                "Department",
                s => s.Name, s => s.StartDate, s => s.Budget, s => s.InstructorID))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToPage("./Index");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var exceptionEntry = ex.Entries.Single();
                    var clientValues = (Department)exceptionEntry.Entity;
                    var databaseEntry = exceptionEntry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError(string.Empty, "Unable to save. " +
                            "The department was deleted by another user.");
                        return Page();
                    }

                    var dbValues = (Department)databaseEntry.ToObject();
                    await setDbErrorMessage(dbValues, clientValues, _context);

                    // Save the current ConcurrencyToken so next postback
                    // matches unless an new concurrency issue happens.
                    Department.ConcurrencyToken = (byte[])dbValues.ConcurrencyToken;
                    // Clear the model error for the next postback.
                    ModelState.Remove($"{nameof(Department)}.{nameof(Department.ConcurrencyToken)}");
                }
            }

            InstructorNameSL = new SelectList(_context.Instructors,
                "ID", "FullName", departmentToUpdate.InstructorID);

            return Page();
        }


        ///////////////////////////////////////////////////////////////////////////////////////
        // HandleDeletedDepartment
        ///////////////////////////////////////////////////////////////////////////////////////
        private IActionResult HandleDeletedDepartment()
        {
            var deletedDepartment = new Department();
            // ModelState contains the posted data because of the deletion error
            // and overides the Department instance values when displaying Page().
            ModelState.AddModelError(string.Empty,
                "Unable to save. The department was deleted by another user.");
            InstructorNameSL = new SelectList(_context.Instructors, "ID", "FullName", Department.InstructorID);
            return Page();
        }


        ///////////////////////////////////////////////////////////////////////////////////////
        // setDbErrorMessage
        ///////////////////////////////////////////////////////////////////////////////////////
        private async Task setDbErrorMessage(Department dbValues,
                Department clientValues, SchoolContext context)
        {

            if (dbValues.Name != clientValues.Name)
            {
                ModelState.AddModelError("Department.Name",
                    $"Current value: {dbValues.Name}");
            }
            if (dbValues.Budget != clientValues.Budget)
            {
                ModelState.AddModelError("Department.Budget",
                    $"Current value: {dbValues.Budget:c}");
            }
            if (dbValues.StartDate != clientValues.StartDate)
            {
                ModelState.AddModelError("Department.StartDate",
                    $"Current value: {dbValues.StartDate:d}");
            }
            if (dbValues.InstructorID != clientValues.InstructorID)
            {
                Instructor dbInstructor = await _context.Instructors
                   .FindAsync(dbValues.InstructorID);
                ModelState.AddModelError("Department.InstructorID",
                    $"Current value: {dbInstructor?.FullName}");
            }

            ModelState.AddModelError(string.Empty,
                "The record you attempted to edit "
              + "was modified by another user after you. The "
              + "edit operation was canceled and the current values in the database "
              + "have been displayed. If you still want to edit this record, click "
              + "the Save button again.");
        }











        //public async Task<IActionResult> OnGetAsync(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    Department = await _context.Departments
        //        .Include(d => d.Administrator).FirstOrDefaultAsync(m => m.DepartmentID == id);

        //    if (Department == null)
        //    {
        //        return NotFound();
        //    }
        //   ViewData["InstructorID"] = new SelectList(_context.Instructors, "ID", "FirstMidName");
        //    return Page();
        //}


        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        //public async Task<IActionResult> OnPostAsync()
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return Page();
        //    }

        //    _context.Attach(Department).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!DepartmentExists(Department.DepartmentID))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return RedirectToPage("./Index");
        //}

        //private bool DepartmentExists(int id)
        //{
        //    return _context.Departments.Any(e => e.DepartmentID == id);
        //}
    }
}
