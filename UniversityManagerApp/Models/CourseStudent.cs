namespace UniversityManagerApp.Models
{
    public class CourseStudent
    {
        public int CourseID { get; set; }
        public Course Course { get; set; }
        public string StudentID { get; set; }
        public Student Student { get; set; }
    }
}
