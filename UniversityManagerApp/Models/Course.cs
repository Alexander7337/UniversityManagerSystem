using System.ComponentModel.DataAnnotations;

namespace UniversityManagerApp.Models
{
    public class Course
    {
        public Course()
        {
            this.CourseStudents = new HashSet<CourseStudent>();
        }

        [Key]
        public int CourseID { get; set; }

        [Display(Name = "Subject Name")]
        public string CourseName { get; set; }

        public virtual ICollection<CourseStudent> CourseStudents { get; set; }
    }
}
