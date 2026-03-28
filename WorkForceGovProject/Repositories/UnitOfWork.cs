using Microsoft.EntityFrameworkCore.Storage;
using WorkForceGovProject.Data;
using WorkForceGovProject.Models;

namespace WorkForceGovProject.Repositories
{
    /// <summary>
    /// Unit of Work Implementation
    /// Manages all repositories and transactions
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction _transaction;

        // Generic repositories
        private IRepository<User> _userRepository;
        private IRepository<Citizen> _citizenRepository;
        private IRepository<CitizenDocument> _documentRepository;
        private IRepository<JobOpening> _jobRepository;
        private IRepository<Application> _applicationRepository;
        private IRepository<Benefit> _benefitRepository;
        private IRepository<EmploymentProgram> _programRepository;
        private IRepository<Notification> _notificationRepository;
        private IRepository<ComplianceRecord> _complianceRepository;
        private IRepository<Audit> _auditRepository;
        private IRepository<Report> _reportRepository;

        // Specific repositories
        private IUserRepository _userRepositorySpecific;
        private ICitizenRepository _citizenRepositorySpecific;
        private IJobRepository _jobRepositorySpecific;
        private IApplicationRepository _applicationRepositorySpecific;
        private INotificationRepository _notificationRepositorySpecific;
        private IComplianceRepository _complianceRepositorySpecific;
        private IAuditRepository _auditRepositorySpecific;
        private IReportRepository _reportRepositorySpecific;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        // Generic repository properties
        public IRepository<User> Users => _userRepository ??= new Repository<User>(_context);
        public IRepository<Citizen> Citizens => _citizenRepository ??= new Repository<Citizen>(_context);
        public IRepository<CitizenDocument> CitizenDocuments => _documentRepository ??= new Repository<CitizenDocument>(_context);
        public IRepository<JobOpening> JobOpenings => _jobRepository ??= new Repository<JobOpening>(_context);
        public IRepository<Application> Applications => _applicationRepository ??= new Repository<Application>(_context);
        public IRepository<Benefit> Benefits => _benefitRepository ??= new Repository<Benefit>(_context);
        public IRepository<EmploymentProgram> EmploymentPrograms => _programRepository ??= new Repository<EmploymentProgram>(_context);
        public IRepository<Notification> Notifications => _notificationRepository ??= new Repository<Notification>(_context);
        public IRepository<ComplianceRecord> ComplianceRecords => _complianceRepository ??= new Repository<ComplianceRecord>(_context);
        public IRepository<Audit> Audits => _auditRepository ??= new Repository<Audit>(_context);
        public IRepository<Report> Reports => _reportRepository ??= new Repository<Report>(_context);

        // Specific repository properties
        public IUserRepository UserRepository => _userRepositorySpecific ??= new UserRepository(_context);
        public ICitizenRepository CitizenRepository => _citizenRepositorySpecific ??= new CitizenRepository(_context);
        public IJobRepository JobRepository => _jobRepositorySpecific ??= new JobRepository(_context);
        public IApplicationRepository ApplicationRepository => _applicationRepositorySpecific ??= new ApplicationRepository(_context);
        public INotificationRepository NotificationRepository => _notificationRepositorySpecific ??= new NotificationRepository(_context);
        public IComplianceRepository ComplianceRepository => _complianceRepositorySpecific ??= new ComplianceRepository(_context);
        public IAuditRepository AuditRepository => _auditRepositorySpecific ??= new AuditRepository(_context);
        public IReportRepository ReportRepository => _reportRepositorySpecific ??= new ReportRepository(_context);

        public async Task<bool> SaveChangesAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
            return _transaction != null;
        }

        public async Task<bool> CommitTransactionAsync()
        {
            try
            {
                if (_transaction != null)
                {
                    await SaveChangesAsync();
                    await _transaction.CommitAsync();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        public async Task<bool> RollbackTransactionAsync()
        {
            try
            {
                if (_transaction != null)
                {
                    await _transaction.RollbackAsync();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        public void ChangeTracker(bool enabled)
        {
            _context.ChangeTracker.AutoDetectChangesEnabled = enabled;
        }

        public async ValueTask DisposeAsync()
        {
            if (_transaction != null)
                await _transaction.DisposeAsync();

            await _context.DisposeAsync();
        }
    }
}
