using ContosoUniversity.Data;
using ContosoUniversity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContosoUniversity.Pages.Students
{
    public class EditModel : PageModel
    {
        private readonly ContosoUniversity.Data.SchoolContext _context;

        public EditModel(ContosoUniversity.Data.SchoolContext context)
        {
            _context = context;
        }

        ///////////////////////////////////////////////////////////////////////////////////////
        // BIND PROPERTY
        ///////////////////////////////////////////////////////////////////////////////////////
        [BindProperty]
        public Student Student { get; set; }
        // Replace ViewData["InstructorID"] 
 
        ///////////////////////////////////////////////////////////////////////////////////////
        // GET
        ///////////////////////////////////////////////////////////////////////////////////////
        public async Task<IActionResult> OnGetAsync(int id)
        {
            string logSnippet = "[Student][Edit][GET] =>";

            Student = await _context.Students
                .AsNoTracking()                 // tracking not required
                .FirstOrDefaultAsync(m => m.ID == id);

            if (Student == null)
            {
                return NotFound();
            }

            // Encoding.Default.GetString(bytes);

            string anyNameString = this.Student.AnyName[7].ToString();

            Console.WriteLine();
            Console.WriteLine($"{logSnippet} (this.Student.ID).....: '{this.Student.ID}'");
            Console.WriteLine($"{logSnippet} (anyNameString).......: '{anyNameString}'");
            Console.WriteLine();

            return Page();
        }

        ///////////////////////////////////////////////////////////////////////////////////////
        // POST
        ///////////////////////////////////////////////////////////////////////////////////////
        public async Task<IActionResult> OnPostAsync(int id)
        {
            string logSnippet = "[Student][Edit][POST] =>";

            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Fetch current student from DB. ConcurrencyToken may have changed.
            var studentToUpdate = await _context.Students.FirstOrDefaultAsync(m => m.ID == id);
            
            if (studentToUpdate == null)
            {
                ModelState.AddModelError(string.Empty, "Unable to save. The student was deleted by another user.");
                return Page();
            }

            Console.WriteLine();
            Console.WriteLine("--------------------------------------------------------------------------------------------");
            Console.WriteLine($"{logSnippet} (this.Student == null).........: '{this.Student == null}'");
            Console.WriteLine($"{logSnippet} (this.Student.LastName == null): '{this.Student.LastName == null}'");
            Console.WriteLine($"{logSnippet} (this.Student.LastName)........: '{this.Student.LastName}'");
            Console.WriteLine($"{logSnippet} (this.Student.AnyName == null).: '{this.Student.AnyName == null}'");
            Console.WriteLine("--------------------------------------------------------------------------------------------");
            Console.WriteLine();

            string thisDotStudentToUpdateAnyNameString = null;

            if (this.Student != null) { thisDotStudentToUpdateAnyNameString = this.Student.AnyName[7].ToString(); }
            string studentToUpdateAnyNameString        = studentToUpdate.AnyName[7].ToString();

            Console.WriteLine();
            Console.WriteLine("--------------------------------------------------------------------------------------------");
            Console.WriteLine($"{logSnippet} (this.Student == null)...............: '{this.Student == null}'");
            Console.WriteLine("--------------------------------------------------------------------------------------------");
            Console.WriteLine($"{logSnippet} (this.Student.ID)....................: '{this.Student.ID}'");
            Console.WriteLine($"{logSnippet} (thisDotStudentToUpdateAnyNameString): '{thisDotStudentToUpdateAnyNameString}'");
            Console.WriteLine("--------------------------------------------------------------------------------------------");
            Console.WriteLine($"{logSnippet} (studentToUpdate.ID).................: '{studentToUpdate.ID}'");
            Console.WriteLine($"{logSnippet} (studentToUpdateAnyNameString).......: '{studentToUpdateAnyNameString}'");
            Console.WriteLine();

            // Set ConcurrencyToken to value read in OnGetAsync
            _context.Entry(studentToUpdate).Property(d => d.AnyName).OriginalValue = Student.AnyName;

            if (await TryUpdateModelAsync<Student>(
                studentToUpdate,
                "Student",
                s => s.FirstMidName, s => s.LastName, s => s.EnrollmentDate))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToPage("./Index");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var exceptionEntry = ex.Entries.Single();
                    var clientValues = (Student)exceptionEntry.Entity;
                    var databaseEntry = exceptionEntry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError(string.Empty, "Unable to save. The department was deleted by another user.");
                        return Page();
                    }

                    var dbValues = (Student)databaseEntry.ToObject();
                    setDbErrorMessage(dbValues, clientValues, _context);

                    // Save the current ConcurrencyToken (AnyName) so next postback matches unless an new concurrency issue happens.
                    Student.AnyName = (byte[])dbValues.AnyName;
                    // Clear the model error for the next postback.
                    ModelState.Remove($"{nameof(Student)}.{nameof(Student.AnyName)}");
                }
            }

            return Page();
        }

        private void setDbErrorMessage(Student dbValues, Student clientValues, SchoolContext context)
        {
            string logSnippet = "[Student][Edit][setDbErrorMessage] =>";
            Console.WriteLine();
            Console.WriteLine($"{logSnippet} (dbValues.FirstMidName)......: '{dbValues.FirstMidName}'");
            Console.WriteLine($"{logSnippet} (clientValues.FirstMidName)..: '{clientValues.FirstMidName}'");
            Console.WriteLine("--------------------------------------------------------------------------------");
            Console.WriteLine($"{logSnippet} (dbValues.LastName)..........: '{dbValues.LastName}'");
            Console.WriteLine($"{logSnippet} (clientValues.LastName)......: '{clientValues.LastName}'");
            Console.WriteLine("--------------------------------------------------------------------------------");
            Console.WriteLine($"{logSnippet} (dbValues.EnrollmentDate)....: '{dbValues.EnrollmentDate}'");
            Console.WriteLine($"{logSnippet} (clientValues.EnrollmentDate): '{clientValues.EnrollmentDate}'");
            Console.WriteLine();


            if (dbValues.FirstMidName != clientValues.FirstMidName)
            {
                ModelState.AddModelError("Student.FirstMidName", $"Current value: {dbValues.FirstMidName}");
            }
            if (dbValues.LastName != clientValues.LastName)
            {
                ModelState.AddModelError("Student.LastName", $"Current value: {dbValues.LastName}");
            }
            if (dbValues.EnrollmentDate != clientValues.EnrollmentDate)
            {
                ModelState.AddModelError("Student.EnrollmentDate", $"Current value: {dbValues.EnrollmentDate:d}");
            }

            ModelState.AddModelError(string.Empty,
                "The record you attempted to edit "
              + "was modified by another user after you. The "
              + "edit operation was canceled and the current values in the database "
              + "have been displayed. If you still want to edit this record, click "
              + "the Save button again.");
        }
    }
}