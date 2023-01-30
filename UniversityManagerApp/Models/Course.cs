using System.ComponentModel.DataAnnotations;

namespace UniversityManagerApp.Models
{
    public class Course
    {
        public Course()
        {
            this.Students = new HashSet<Student>();
        }

        [Key]
        public int CourseID { get; set; }

        [Display(Name = "Subject Name")]
        public string CourseName { get; set; }
        public virtual ICollection<Student> Students { get; set; }
    }
}
