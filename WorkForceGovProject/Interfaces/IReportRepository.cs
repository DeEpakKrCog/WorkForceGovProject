using WorkForceGovProject.Models;

namespace WorkForceGovProject.Interfaces
{
    public interface IReportRepository : IRepository<Report>
    {
        Task<IEnumerable<Report>> GetReportsByTypeAsync(string reportType);
        Task<IEnumerable<Report>> GetReportsByDateRangeAsync(DateTime fromDate, DateTime toDate);
        Task<IEnumerable<Report>> GetReportsByGeneratedByAsync(int generatedBy);
        Task<IEnumerable<Report>> GetRecentReportsAsync(int count = 50);
        Task<Report> GetReportWithDetailsAsync(int reportId);
    }
}
