using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.Models
{
    public enum Grade
    {
        A, B, C, D, F
    }

    public class Enrollment
    {
        public int EnrollmentID { get; set; }


        [DisplayFormat(NullDisplayText = "No grade")]
        public Grade? Grade { get; set; }

        /////////////////////////////////////////////////////////////////////////
        // An enrollment record is for one course,
        // so there's a CourseID FK property and a Course navigation property
        /////////////////////////////////////////////////////////////////////////
        public int CourseID { get; set; }
        public Course Course { get; set; }


        /////////////////////////////////////////////////////////////////////////
        // An enrollment record is for one student,
        // so there's a StudentID FK property and a Student navigation property
        /////////////////////////////////////////////////////////////////////////
        public int StudentID { get; set; }
        public Student Student { get; set; }
    }
}