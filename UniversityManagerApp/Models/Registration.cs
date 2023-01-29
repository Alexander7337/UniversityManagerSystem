using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityManagerApp.Models
{
    public class Registration
    {
        public int StudentID { get; set; }

        public int CourseID { get; set; }
    }
}
