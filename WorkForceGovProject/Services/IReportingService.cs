using WorkForceGovProject.Models;

namespace WorkForceGovProject.Services
{
    public interface IReportingService
    {
        Task<Report> GenerateComplianceReportAsync(string scope);
        Task<Report> GenerateProgramReportAsync();
        Task<Report> GenerateApplicationReportAsync();
        Task<List<Report>> GetRecentReportsAsync(int days = 30);
        Task<Dictionary<string, object>> GetDashboardMetricsAsync();
    }
}
