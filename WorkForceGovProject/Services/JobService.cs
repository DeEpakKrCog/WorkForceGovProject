using WorkForceGovProject.Models;
using WorkForceGovProject.Repositories;

namespace WorkForceGovProject.Services
{
    /// <summary>
    /// Job Service Implementation
    /// Handles job operations
    /// </summary>
    public class JobService : IJobService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<JobService> _logger;

        public JobService(IUnitOfWork unitOfWork, ILogger<JobService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<JobOpening> GetJobByIdAsync(int id)
        {
            return await _unitOfWork.JobOpenings.GetByIdAsync(id);
        }

        public async Task<IEnumerable<JobOpening>> GetAllJobsAsync()
        {
            return await _unitOfWork.JobOpenings.GetAllAsync();
        }

        public async Task<IEnumerable<JobOpening>> GetOpenJobsAsync()
        {
            return await _unitOfWork.JobRepository.GetOpenJobsAsync();
        }

        public async Task<IEnumerable<JobOpening>> GetRecentJobsAsync(int count)
        {
            return await _unitOfWork.JobRepository.GetRecentJobsAsync(count);
        }

        public async Task<IEnumerable<JobOpening>> SearchJobsAsync(string location = "", string category = "")
        {
            return await _unitOfWork.JobRepository.SearchJobsAsync(location, category);
        }

        public async Task<IEnumerable<JobOpening>> GetJobsByLocationAsync(string location)
        {
            return await _unitOfWork.JobRepository.GetJobsByLocationAsync(location);
        }

        public async Task<IEnumerable<JobOpening>> GetJobsByCategoryAsync(string category)
        {
            return await _unitOfWork.JobRepository.GetJobsByCategoryAsync(category);
        }

        public async Task<(bool Success, string Message, JobOpening Job)> CreateJobAsync(JobOpening job)
        {
            try
            {
                if (job == null || string.IsNullOrEmpty(job.JobTitle) || string.IsNullOrEmpty(job.Description))
                    return (false, "Invalid job information.", null);

                job.PostedDate = DateTime.Now;
                job.Status = "Open";
                job.TotalApplications = 0;

                await _unitOfWork.JobOpenings.AddAsync(job);
                var saved = await _unitOfWork.SaveChangesAsync();

                if (saved)
                {
                    _logger.LogInformation($"Job created: {job.JobTitle}");
                    return (true, "Job created successfully.", job);
                }

                return (false, "Failed to create job.", null);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Create job error: {ex.Message}");
                return (false, "An error occurred while creating job.", null);
            }
        }

        public async Task<(bool Success, string Message)> UpdateJobAsync(JobOpening job)
        {
            try
            {
                await _unitOfWork.JobOpenings.UpdateAsync(job);
                var saved = await _unitOfWork.SaveChangesAsync();
                return saved ? (true, "Job updated successfully.") : (false, "Failed to update job.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Update job error: {ex.Message}");
                return (false, "An error occurred while updating job.");
            }
        }

        public async Task<(bool Success, string Message)> CloseJobAsync(int jobId)
        {
            try
            {
                var job = await GetJobByIdAsync(jobId);
                if (job == null)
                    return (false, "Job not found.");

                job.Status = "Closed";
                return await UpdateJobAsync(job);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Close job error: {ex.Message}");
                return (false, "An error occurred while closing job.");
            }
        }

        public async Task<int> GetJobCountAsync()
        {
            return await _unitOfWork.JobOpenings.CountAsync();
        }

        public async Task<int> GetOpenJobCountAsync()
        {
            return await _unitOfWork.JobOpenings.CountAsync(j => j.Status == "Open");
        }

        public async Task<int> GetApplicationCountForJobAsync(int jobId)
        {
            return await _unitOfWork.ApplicationRepository.GetApplicationCountByJobAsync(jobId);
        }
    }
}
