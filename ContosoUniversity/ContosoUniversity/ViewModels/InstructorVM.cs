
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoUniversity.ViewModels
{
    public class InstructorVM
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [Column("FirstName")]
        [Display(Name = "First Name")]
        [StringLength(50)]
        public string FirstMidName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Hire Date")]

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // ALL ON ONE LINE
        // [DataType(DataType.Date),Display(Name = "Hire Date"),DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public DateTime HireDate { get; set; }

        [Display(Name = "Full Name")]
        public string FullName
        {
            get { return LastName + ", " + FirstMidName; }
        }

        public ICollection<CourseVM> Courses { get; set; }


        /////////////////////////////////////////////////////////////////////////////
        // The Instructor.OfficeAssignment navigation property can be null
        // because there might not be an OfficeAssignment row for a given instructor.
        // An instructor might not have an office assignment.
        /////////////////////////////////////////////////////////////////////////////
        public OfficeAssignmentVM OfficeAssignment { get; set; }
    }
}