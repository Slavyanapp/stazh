using Microsoft.AspNetCore.Identity;

namespace ScienceTeamsApp.Models
{
    public class ActivityLog
    {
        public int Id { get; set; }

        public string Action { get; set; } // CRUD operations
        public string Description { get; set; } // Ivan created task "Design Experiment" for team "Physics Team"
        public DateTime Timestamp { get; set; } = DateTime.Now;

        public string? UserId { get; set; }
        public IdentityUser? User { get; set; }
    }
}
