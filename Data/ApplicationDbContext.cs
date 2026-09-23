using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ScienceTeamsApp.Models;

namespace ScienceTeamsApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets for our entities
        public DbSet<Team> Teams { get; set; }
        public DbSet<TaskItem> TaskItems { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }
    }
}
