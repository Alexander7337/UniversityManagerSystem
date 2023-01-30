using UniversityManagerApp.Models;

namespace UniversityManagerApp.Services
{
    public interface ICourseService
    {
        CoursesViewModel GetStudentCourses();

        Course GetCourseByID(int? id);

        ICollection<Course> GetAll();

        ICollection<Course> CreateCourse(Course course);
    }
}
