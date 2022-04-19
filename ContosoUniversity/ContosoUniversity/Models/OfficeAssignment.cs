using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoUniversity.Models
{
    public class OfficeAssignment
    {
        //////////////////////////////////////////////////////////////////////////////
        // The [Key] attribute is used to identify a property as the primary key (PK)
        // when the property name is something other than classnameID or ID.
        //
        // EF Core can't automatically recognize InstructorID as the PK of OfficeAssignment
        // because InstructorID doesn't follow the ID or classnameID naming convention.
        // Therefore, the Key attribute is used to identify InstructorID as the PK
        //
        // The OfficeAssignment PK is also its foreign key (FK) to the Instructor entity.
        // A one-to-zero-or-one relationship occurs when a PK in one table is both a PK
        // and a FK in another table.
        //////////////////////////////////////////////////////////////////////////////
        [Key]
        public int InstructorID { get; set; }


        [StringLength(50)]
        [Display(Name = "Office Location")]
        public string Location { get; set; }


        /////////////////////////////////////////////////////////////////////////////////////////////
        // The OfficeAssignment.Instructor navigation property will always have an instructor entity
        // because the foreign key InstructorID type is int, a non-nullable value type.
        // An office assignment can't exist without an instructor.
        ////////////////////////////////////////////////////////////////////////////////////////////
        public Instructor Instructor { get; set; }
    }
}