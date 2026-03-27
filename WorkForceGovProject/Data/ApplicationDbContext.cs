using Microsoft.EntityFrameworkCore;
using WorkForceGovProject.Models;

namespace WorkForceGovProject.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Citizen> Citizens { get; set; }
        public DbSet<CitizenDocument> CitizenDocuments { get; set; }
        public DbSet<JobOpening> JobOpenings { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<Benefit> Benefits { get; set; }
        public DbSet<EmploymentProgram> EmploymentPrograms { get; set; }
        public DbSet<Notification> Notifications { get; set; }
    }
}