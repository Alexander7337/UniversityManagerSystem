using UniversityManagerApp.Models;

namespace UniversityManagerApp.Services
{
    public interface ICourseService
    {
        CoursesViewModel GetStudentCourses();

        Course GetCourseByID(int? id);

        ICollection<Course> GetAll();

        ICollection<Course> CreateCourse(Course course);

        ICollection<Course> UpdateCourse(Course course);

        ICollection<Course> DeleteCourse(int id);

        CoursesViewModel EnrollCourse(Course course);
    }
}
