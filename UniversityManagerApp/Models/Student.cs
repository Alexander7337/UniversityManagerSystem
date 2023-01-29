using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace UniversityManagerApp.Models
{
    public class Student : IdentityUser
    {
        public Student()
        {
            this.Courses = new HashSet<Course>();
        }

        [Key]
        public int StudentID { get; set; }
        public string? StudentNumber { get; set; }
        public string? StudentName { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
    }
}
