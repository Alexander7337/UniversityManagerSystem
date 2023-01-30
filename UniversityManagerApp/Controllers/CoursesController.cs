using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using UniversityManagerApp.Data;
using UniversityManagerApp.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UniversityManagerApp.Controllers
{
    public class CoursesController : Controller
    {
        private readonly SystemDbContext _context;
        private readonly UserManager<Student> _userManager;
        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<Student> _signInManager;

        public CoursesController(SystemDbContext context, ILogger<HomeController> logger, UserManager<Student> userManager, SignInManager<Student> signInManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index()
        {
              return View(await _context.Courses.ToListAsync());
        }

        public async Task<IActionResult> List()
        {
            var userCourses = new List<Course>();
            if (User.Identity.IsAuthenticated)
            {
                var user = _userManager.FindByNameAsync(_signInManager.Context.User.Identity.Name).Result;
                userCourses = _context.Courses.Where(c => c.CourseStudents.Any(s => s.StudentID == user.Id)).ToList();
            }

            var allCourses = await _context.Courses.ToListAsync();
            var courses = new CoursesViewModel
            {
                AllCourses = allCourses,
                StudentCourses = userCourses
            };

            return View(courses);
        }

        public async Task<IActionResult> Details(int? id)
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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseID,CourseName")] Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

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
                AddErrors("Student is not loggerd in! You must sign into the system first.");
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
