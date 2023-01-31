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

        public ICollection<Course> UpdateCourse(Course course)
        {
            _context.Update(course);
            _context.SaveChanges();

            return _context.Courses.ToList();
        }

        public ICollection<Course> DeleteCourse(int id)
        {
            var course = _context.Courses.FirstOrDefault(c => c.CourseID == id);
            _context.Courses.Remove(course);
            _context.SaveChanges();

            return _context.Courses.ToList();
        }

        public CoursesViewModel EnrollCourse(Course course)
        {
            var user = _userManager.FindByNameAsync(_signInManager.Context.User.Identity.Name).Result;

            if (user != null)
            {
                var cs = new CourseStudent { Course = course, Student = user };
                if (!_context.CourseStudents.Contains(cs))
                {
                    user.CourseStudents.Add(new CourseStudent { Course = course, Student = user });
                    _context.Update(user);
                    _context.SaveChanges();
                }
            }

            var userCourses = _context.Courses.Where(c => c.CourseStudents.Any(s => s.StudentID == user.Id)).ToList();

            var allCourses = _context.Courses.ToList();
            var courses = new CoursesViewModel
            {
                AllCourses = allCourses,
                StudentCourses = userCourses
            };

            return courses;
        }

        public bool IsEnrolled(int id)
        {
            var user = _userManager.FindByNameAsync(_signInManager.Context.User.Identity.Name).Result;
            var userCourses = _context.Students.Where(s => s.Id == user.Id).Select(c => c.CourseStudents).FirstOrDefault();
            
            return userCourses.Any(c => c.CourseID == id);
        }
    }
}
