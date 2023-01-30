namespace UniversityManagerApp.Models
{
    public class CoursesViewModel
    {
        public ICollection<Course> AllCourses { get; set; }

        public ICollection<Course> StudentCourses { get; set; }
    }
}
