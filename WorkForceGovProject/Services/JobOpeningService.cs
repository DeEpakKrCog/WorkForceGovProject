using WorkForceGovProject.Interfaces;
using WorkForceGovProject.Models;

namespace WorkForceGovProject.Services
{
    public class JobOpeningService : IJobOpeningService
    {
        private readonly IJobOpeningRepository _jobOpeningRepository;
        private readonly IEmployerRepository _employerRepository;
        private readonly IApplicationRepository _applicationRepository;

        public JobOpeningService(
            IJobOpeningRepository jobOpeningRepository,
            IEmployerRepository employerRepository,
            IApplicationRepository applicationRepository)
        {
            _jobOpeningRepository = jobOpeningRepository;
            _employerRepository = employerRepository;
            _applicationRepository = applicationRepository;
        }

        public async Task<(bool Success, string Message, JobOpening Job)> CreateJobOpeningAsync(JobOpening jobOpening, int employerId)
        {
            // Validate employer exists
            var employer = await _employerRepository.GetEmployerByIdAsync(employerId);
            if (employer == null)
            {
                return (false, "Employer not found.", null);
            }

            // Validate job data
            if (string.IsNullOrWhiteSpace(jobOpening.Title))
            {
                return (false, "Job title is required.", null);
            }

            if (string.IsNullOrWhiteSpace(jobOpening.Description))
            {
                return (false, "Job description is required.", null);
            }

            // Set job properties
            jobOpening.EmployerId = employerId;
            jobOpening.PostedDate = DateTime.Now;
            jobOpening.Status = "Open";

            var createdJob = await _jobOpeningRepository.CreateJobOpeningAsync(jobOpening);

            return (true, "Job posted successfully!", createdJob);
        }

        public async Task<JobOpening> GetJobOpeningByIdAsync(int id)
        {
            return await _jobOpeningRepository.GetJobOpeningByIdAsync(id);
        }

        public async Task<IEnumerable<JobOpening>> GetJobOpeningsByEmployerIdAsync(int employerId)
        {
            return await _jobOpeningRepository.GetJobOpeningsByEmployerIdAsync(employerId);
        }

        public async Task<(bool Success, string Message)> UpdateJobOpeningAsync(JobOpening jobOpening, int employerId)
        {
            var existingJob = await _jobOpeningRepository.GetJobOpeningByIdAsync(jobOpening.Id);
            if (existingJob == null)
            {
                return (false, "Job not found.");
            }

            if (existingJob.EmployerId != employerId)
            {
                return (false, "Unauthorized access.");
            }

            // Update fields
            existingJob.Title = jobOpening.Title;
            existingJob.Description = jobOpening.Description;
            existingJob.Location = jobOpening.Location;
            existingJob.Salary = jobOpening.Salary;
            existingJob.Requirements = jobOpening.Requirements;
            existingJob.ApplicationDeadline = jobOpening.ApplicationDeadline;
            existingJob.Status = jobOpening.Status;

            await _jobOpeningRepository.UpdateJobOpeningAsync(existingJob);

            return (true, "Job updated successfully!");
        }

        public async Task<(bool Success, string Message)> DeleteJobOpeningAsync(int jobId, int employerId)
        {
            var job = await _jobOpeningRepository.GetJobOpeningByIdAsync(jobId);
            if (job == null)
            {
                return (false, "Job not found.");
            }

            if (job.EmployerId != employerId)
            {
                return (false, "Unauthorized access.");
            }

            // Check if there are applications
            var applicationsCount = await _applicationRepository.GetApplicationsCountByJobIdAsync(jobId);
            if (applicationsCount > 0)
            {
                return (false, "Cannot delete job with existing applications. Consider closing it instead.");
            }

            await _jobOpeningRepository.DeleteJobOpeningAsync(jobId);

            return (true, "Job deleted successfully!");
        }

        public async Task<(bool Success, string Message)> CloseJobOpeningAsync(int jobId, int employerId)
        {
            var job = await _jobOpeningRepository.GetJobOpeningByIdAsync(jobId);
            if (job == null)
            {
                return (false, "Job not found.");
            }

            if (job.EmployerId != employerId)
            {
                return (false, "Unauthorized access.");
            }

            if (job.Status == "Closed")
            {
                return (false, "Job is already closed.");
            }

            await _jobOpeningRepository.UpdateJobStatusAsync(jobId, "Closed");

            return (true, "Job closed successfully!");
        }

        public async Task<(bool Success, string Message)> ReopenJobOpeningAsync(int jobId, int employerId)
        {
            var job = await _jobOpeningRepository.GetJobOpeningByIdAsync(jobId);
            if (job == null)
            {
                return (false, "Job not found.");
            }

            if (job.EmployerId != employerId)
            {
                return (false, "Unauthorized access.");
            }

            if (job.Status == "Open")
            {
                return (false, "Job is already open.");
            }

            await _jobOpeningRepository.UpdateJobStatusAsync(jobId, "Open");

            return (true, "Job reopened successfully!");
        }

        public async Task<IEnumerable<JobOpening>> GetActiveJobOpeningsAsync()
        {
            return await _jobOpeningRepository.GetActiveJobOpeningsAsync();
        }

        public async Task<IEnumerable<JobOpening>> SearchJobOpeningsAsync(string searchTerm)
        {
            return await _jobOpeningRepository.SearchJobOpeningsAsync(searchTerm);
        }

        public async Task<IEnumerable<JobOpening>> GetJobOpeningsByLocationAsync(string location)
        {
            return await _jobOpeningRepository.GetJobOpeningsByLocationAsync(location);
        }

        public async Task<IEnumerable<JobOpening>> GetRecentJobOpeningsAsync(int count)
        {
            return await _jobOpeningRepository.GetRecentJobOpeningsAsync(count);
        }

        public async Task<int> GetActiveJobsCountByEmployerAsync(int employerId)
        {
            return await _jobOpeningRepository.GetActiveJobsCountByEmployerAsync(employerId);
        }

        public async Task<int> GetApplicationsCountForJobAsync(int jobId)
        {
            return await _applicationRepository.GetApplicationsCountByJobIdAsync(jobId);
        }

        public async Task<bool> CanEmployerAccessJobAsync(int jobId, int employerId)
        {
            var job = await _jobOpeningRepository.GetJobOpeningByIdAsync(jobId);
            return job?.EmployerId == employerId;
        }

        public async Task<bool> IsJobOpenForApplicationsAsync(int jobId)
        {
            var job = await _jobOpeningRepository.GetJobOpeningByIdAsync(jobId);
            if (job == null || job.Status != "Open")
            {
                return false;
            }

            // Check deadline
            if (job.ApplicationDeadline.HasValue && job.ApplicationDeadline.Value < DateTime.Now)
            {
                return false;
            }

            return true;
        }
    }
}
