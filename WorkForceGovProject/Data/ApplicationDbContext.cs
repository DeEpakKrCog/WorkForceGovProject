using Microsoft.EntityFrameworkCore;
using WorkForceGovProject.Models;

namespace WorkForceGovProject.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Citizen> Citizens { get; set; }
    }
}