using System.ComponentModel.DataAnnotations;

namespace ScienceTeamsApp.Models
{
    public class Team
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Team Name")]
        public string Name { get; set; }

        [Display(Name = "Team Description")]
        public string Description { get; set; }

        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // One team can have many tasks
        public List<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}
