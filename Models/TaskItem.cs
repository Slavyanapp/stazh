using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ScienceTeamsApp.Models
{
    public class TaskItem
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Task Title")]
        public string Title { get; set; }

        [Display(Name = "Task Description")]
        public string Description { get; set; }

        [Display(Name = "Deadline")]
        public DateTime Deadline { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; } = "To Do"; // To Do, In Progress, Done

        // Foreign key to Team
        public int TeamId { get; set; } 
        public Team? Team { get; set; } 

        // Connection to IdentityUser for task assignment
        public string? AssignedUserId { get; set; }
        public IdentityUser? AssignedUser {  get; set; } 
    }
}
