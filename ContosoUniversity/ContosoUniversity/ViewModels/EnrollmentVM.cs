using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.ViewModels
{
    public enum Grade
    {
        A, B, C, D, F
    }

    public class EnrollmentVM
    {
        public int EnrollmentID { get; set; }


        [DisplayFormat(NullDisplayText = "No grade")]
        public Grade? Grade { get; set; }

        /////////////////////////////////////////////////////////////////////////
        // An enrollment record is for one course,
        // so there's a CourseID FK property and a Course navigation property
        /////////////////////////////////////////////////////////////////////////
        public int CourseID { get; set; }
        public CourseVM Course { get; set; }


        /////////////////////////////////////////////////////////////////////////
        // An enrollment record is for one student,
        // so there's a StudentID FK property and a Student navigation property
        /////////////////////////////////////////////////////////////////////////
        public int StudentID { get; set; }
        public StudentVM Student { get; set; }
    }
}
