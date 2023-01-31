using Microsoft.AspNetCore.Mvc;
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
            => !User.Identity.IsAuthenticated ? 
                View("Error", new ErrorViewModel { ExceptionMessage = "You must be logged into the system in order to enroll in a course" })
                    : View(_courseService.GetCourseByID(id));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Enroll(int id, [Bind("CourseID,CourseName")] Course course)
             => ModelState.IsValid ?
                    _courseService.IsEnrolled(id) ? 
                        View("Error", new ErrorViewModel { ExceptionMessage = "You have already registered for this course" })
                            : View("List", _courseService.EnrollCourse(course))
                                : View(course);
    }
}
