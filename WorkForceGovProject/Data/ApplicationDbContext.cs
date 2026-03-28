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
        public DbSet<Employer> Employers { get; set; }
        public DbSet<EmployerDocument> EmployerDocuments { get; set; }
        public DbSet<JobOpening> JobOpenings { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<Application> JobApplications => Applications; // Alias for compatibility
        public DbSet<Benefit> Benefits { get; set; }
        public DbSet<EmploymentProgram> EmploymentPrograms { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<SystemLog> SystemLogs { get; set; }
        public DbSet<Report> Reports { get; set; }
    }
}