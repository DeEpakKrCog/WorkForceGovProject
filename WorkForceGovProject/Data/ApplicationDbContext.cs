using Microsoft.EntityFrameworkCore;
using WorkForceGovProject.Models;

namespace WorkForceGovProject.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Citizen> Citizens { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<SystemLog> SystemLogs { get; set; }
        public DbSet<Report> Reports { get; set; }
    }
}