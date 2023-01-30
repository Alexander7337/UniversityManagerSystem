using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace UniversityManagerApp.Models
{
    public class Student : IdentityUser
    {
        public Student()
        {
            this.CourseStudents = new HashSet<CourseStudent>();
        }

        public string? StudentNumber { get; set; }
        public string? StudentName { get; set; }
        public virtual ICollection<CourseStudent> CourseStudents { get; set; }
    }
}
