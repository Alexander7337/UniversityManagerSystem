using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UniversityManagerApp.Data;
using UniversityManagerApp.Models;

namespace UniversityManagerApp.Services
{
    public class CourseService : ICourseService
    {
        private readonly SystemDbContext _context;
        private readonly UserManager<Student> _userManager;
        private readonly SignInManager<Student> _signInManager;

        public CourseService(SystemDbContext context, UserManager<Student> userManager, SignInManager<Student> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public CoursesViewModel GetStudentCourses()
        {
            var userCourses = new List<Course>();
            if (_signInManager.Context.User.Identity.IsAuthenticated)
            {
                var user = _userManager.FindByNameAsync(_signInManager.Context.User.Identity.Name).Result;
                userCourses = _context.Courses.Where(c => c.CourseStudents.Any(s => s.StudentID == user.Id)).ToList();
            }

            var allCourses = _context.Courses.ToList();
            var courses = new CoursesViewModel
            {
                AllCourses = allCourses,
                StudentCourses = userCourses
            };

            return courses;
        }

        public Course GetCourseByID(int? id) 
        {
            if (id == null || _context.Courses == null)
            {
                return new Course();
            }

            var course = _context.Courses
                .FirstOrDefault(m => m.CourseID == id);
            if (course == null)
            {
                return new Course();
            }

            return course;
        }

        public ICollection<Course> GetAll()
        {
            return _context.Courses.ToList();
        }

        public ICollection<Course> CreateCourse(Course course)
        {
            _context.Add(course);
            _context.SaveChanges();

            return _context.Courses.ToList();
        }
    }
}
