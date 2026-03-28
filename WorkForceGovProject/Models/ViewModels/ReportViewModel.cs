using System.ComponentModel.DataAnnotations;

namespace WorkForceGovProject.Models.ViewModels
{
    public class ReportGenerationViewModel
    {
        [Required]
        [StringLength(100)]
        public string ReportName { get; set; }

        [Required]
        public string ReportType { get; set; } // Employment, Compliance, Participation

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string FilterCriteria { get; set; }
    }

    public class ReportListViewModel
    {
        public int ReportId { get; set; }

        public string ReportName { get; set; }

        public string ReportType { get; set; }

        public DateTime GeneratedDate { get; set; }

        public string GeneratedByName { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }

    public class SystemActivityViewModel
    {
        public int LogId { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; }

        public string Action { get; set; }

        public string Resource { get; set; }

        public DateTime Timestamp { get; set; }

        public string IpAddress { get; set; }
    }

    public class SystemMonitoringViewModel
    {
        public int TotalActivities { get; set; }

        public int TodayActivities { get; set; }

        public List<SystemActivityViewModel> RecentActivities { get; set; } = new List<SystemActivityViewModel>();

        public DateTime? FilterFromDate { get; set; }

        public DateTime? FilterToDate { get; set; }

        public string FilterAction { get; set; }
    }
}
