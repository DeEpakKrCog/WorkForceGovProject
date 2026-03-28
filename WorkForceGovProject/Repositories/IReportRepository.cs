using WorkForceGovProject.Models;

namespace WorkForceGovProject.Repositories
{
    public interface IReportRepository : IRepository<Report>
    {
        Task<List<Report>> GetByReportTypeAsync(string reportType);
        Task<List<Report>> GetRecentReportsAsync(DateTime startDate);
        Task<Report> GetLatestReportByTypeAsync(string reportType);
    }
}
