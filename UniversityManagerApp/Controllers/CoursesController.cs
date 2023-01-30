using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityManagerApp.Data;
using UniversityManagerApp.Models;
using UniversityManagerApp.Services;

namespace UniversityManagerApp.Controllers
{
    public class CoursesController : Controller
    {
        private readonly SystemDbContext _context;
        private readonly UserManager<Student> _userManager;
        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<Student> _signInManager;
        private readonly ICourseService _courseService;

        public CoursesController(SystemDbContext context, ILogger<HomeController> logger, UserManager<Student> userManager, SignInManager<Student> signInManager, ICourseService courseService)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _courseService = courseService;
        }

        public IActionResult Index()
            => View(_courseService.GetAll());

        public IActionResult List()
            => View(_courseService.GetStudentCourses());

        public IActionResult Details(int? id)
            => View(_courseService.GetCourseByID(id));

        public IActionResult Create()
            => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("CourseID,CourseName")] Course course)
            => ModelState.IsValid ? View("Index", _courseService.CreateCourse(course)) : View(course);

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Courses == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CourseID,CourseName")] Course course)
        {
            if (id != course.CourseID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.CourseID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Courses == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .FirstOrDefaultAsync(m => m.CourseID == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Courses == null)
            {
                return Problem("Entity set 'Courses' is null.");
            }
            var course = await _context.Courses.FindAsync(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Enroll(int id, [Bind("CourseID,CourseName")] Course course)
        {
            if (id != course.CourseID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = _userManager.FindByNameAsync(_signInManager.Context.User.Identity.Name).Result;

                    if (user != null)
                    {
                        var cs = new CourseStudent { Course = course, Student = user };
                        if (!_context.CourseStudents.Contains(cs))
                        {
                            user.CourseStudents.Add(new CourseStudent { Course = course, Student = user });
                            _context.Update(user);
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            AddErrors("Student is already enrolled in this course!");
                        }
                    }
                    else
                    {
                        AddErrors("Student is not loggerd in! You must sign into the system first.");
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(List));
        }

        public async Task<IActionResult> Enroll(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (!User.Identity.IsAuthenticated)
            {
                AddErrors("Student is not logged in! You must first sign into the system.");
            }

            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        private bool CourseExists(int id)
        {
          return _context.Courses.Any(e => e.CourseID == id);
        }

        private void AddErrors(string errorMessage)
        {
            ModelState.AddModelError("CourseError", errorMessage);
        }
    }
}
