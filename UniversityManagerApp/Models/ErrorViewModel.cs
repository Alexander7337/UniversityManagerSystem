namespace UniversityManagerApp.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public string? ExceptionMessage { get; set; } = "An error occured. Please contact the Support.";
    }
}