using StazorPages.Constants;

namespace StazorPages.Models
{
    public class NotificationResult
    {
        public NotificationResult()
        {
        }

        public NotificationStatus Status { get; set; }

        public string Message { get; set; }
    }
}
