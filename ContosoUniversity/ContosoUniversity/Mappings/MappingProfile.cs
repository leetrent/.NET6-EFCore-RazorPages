using AutoMapper;
using ContosoUniversity.Models;
using ContosoUniversity.ViewModels;

namespace ContosoUniversity.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMapProfiles();
        }

        private void CreateMapProfiles()
        {
            CreateMap<Course, CourseVM>().ReverseMap();
            CreateMap<Department, DepartmentVM>().ReverseMap();
            CreateMap<Enrollment, EnrollmentVM>().ReverseMap();
            CreateMap<Instructor, InstructorVM>().ReverseMap();
            CreateMap<OfficeAssignment, OfficeAssignmentVM>().ReverseMap();
            CreateMap<Student, StudentVM>().ReverseMap();
        }
    }
}
