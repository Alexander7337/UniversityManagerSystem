using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UniversityManagerApp.Data;
using UniversityManagerApp.Models;
using UniversityManagerApp.Services;

namespace UniversityManagerApp.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ICourseService _courseService;

        public CoursesController(ICourseService courseService)
        {
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

        public IActionResult Edit(int? id)
            => View(_courseService.GetCourseByID(id));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("CourseID,CourseName")] Course course)
            => ModelState.IsValid ? View("Index", _courseService.UpdateCourse(course)) : View(course);

        public IActionResult Delete(int? id)
            => View(_courseService.GetCourseByID(id));

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
            => View("Index", _courseService.DeleteCourse(id));

        public IActionResult Enroll(int? id)
            => !User.Identity.IsAuthenticated ? BadRequest("Student is not logged in! You must first sign into the system.") : View(_courseService.GetCourseByID(id));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Enroll(int id, [Bind("CourseID,CourseName")] Course course)
             => ModelState.IsValid ? View("List", _courseService.EnrollCourse(course)) : View(course);
    }
}
