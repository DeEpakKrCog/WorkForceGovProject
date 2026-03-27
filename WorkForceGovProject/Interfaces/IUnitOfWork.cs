using WorkForceGovProject.Models;

namespace WorkForceGovProject.Interfaces
{
    /// <summary>
    /// Unit of Work Pattern - Manages all repositories
    /// Ensures data consistency across multiple repositories
    /// </summary>
    public interface IUnitOfWork : IAsyncDisposable
    {
        // Repository properties
        IRepository<User> Users { get; }
        IRepository<Citizen> Citizens { get; }
        IRepository<CitizenDocument> CitizenDocuments { get; }
        IRepository<JobOpening> JobOpenings { get; }
        IRepository<Application> Applications { get; }
        IRepository<Benefit> Benefits { get; }
        IRepository<EmploymentProgram> EmploymentPrograms { get; }
        IRepository<Notification> Notifications { get; }

        // Specific repositories (if needed for complex queries)
        IUserRepository UserRepository { get; }
        ICitizenRepository CitizenRepository { get; }
        IJobRepository JobRepository { get; }
        IApplicationRepository ApplicationRepository { get; }
        INotificationRepository NotificationRepository { get; }

        // Transaction management
        Task<bool> SaveChangesAsync();
        Task<bool> BeginTransactionAsync();
        Task<bool> CommitTransactionAsync();
        Task<bool> RollbackTransactionAsync();
        
        // Context management
        void ChangeTracker(bool enabled);
    }
}
