using WorkForceGovProject.Interfaces;
using WorkForceGovProject.Models;

namespace WorkForceGovProject.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly ICitizenRepository _citizenRepository;
        private readonly IJobOpeningRepository _jobOpeningRepository;
        private readonly IUserRepository _userRepository;
        private readonly INotificationService _notificationService;

        public ApplicationService(
            IApplicationRepository applicationRepository,
            ICitizenRepository citizenRepository,
            IJobOpeningRepository jobOpeningRepository,
            IUserRepository userRepository,
            INotificationService notificationService)
        {
            _applicationRepository = applicationRepository;
            _citizenRepository = citizenRepository;
            _jobOpeningRepository = jobOpeningRepository;
            _userRepository = userRepository;
            _notificationService = notificationService;
        }

        public async Task<(bool Success, string Message, Application Application)> SubmitApplicationAsync(int citizenId, int jobId)
        {
            // Check if citizen exists
            var citizen = await _citizenRepository.GetCitizenByIdAsync(citizenId);
            if (citizen == null)
            {
                return (false, "Citizen not found.", null);
            }

            // Check if job exists and is open
            var job = await _jobOpeningRepository.GetJobOpeningByIdAsync(jobId);
            if (job == null)
            {
                return (false, "Job not found.", null);
            }

            if (job.Status != "Open")
            {
                return (false, "This job is no longer accepting applications.", null);
            }

            // Check if already applied
            if (await _applicationRepository.HasCitizenAppliedForJobAsync(citizenId, jobId))
            {
                return (false, "You have already applied for this job.", null);
            }

            // Create application
            var application = new Application
            {
                CitizenId = citizenId,
                JobId = jobId,
                SubmittedDate = DateTime.Now,
                Status = "Pending"
            };

            var createdApplication = await _applicationRepository.SubmitApplicationAsync(application);

            // Increment citizen's active applications
            await _citizenRepository.IncrementActiveApplicationsAsync(citizenId);

            // Notify employer
            await _notificationService.CreateJobApplicationNotificationAsync(jobId, job.EmployerId);

            return (true, "Application submitted successfully!", createdApplication);
        }

        public async Task<Application> GetApplicationByIdAsync(int id)
        {
            return await _applicationRepository.GetApplicationByIdAsync(id);
        }

        public async Task<IEnumerable<Application>> GetApplicationsByCitizenIdAsync(int citizenId)
        {
            return await _applicationRepository.GetApplicationsByCitizenIdAsync(citizenId);
        }

        public async Task<IEnumerable<Application>> GetApplicationsByJobIdAsync(int jobId)
        {
            return await _applicationRepository.GetApplicationsByJobIdAsync(jobId);
        }

        public async Task<IEnumerable<Application>> GetApplicationsByEmployerIdAsync(int employerId)
        {
            return await _applicationRepository.GetApplicationsByEmployerIdAsync(employerId);
        }

        public async Task<ApplicationDetailsDto> GetApplicationDetailsAsync(int applicationId)
        {
            var application = await _applicationRepository.GetApplicationByIdAsync(applicationId);
            if (application == null)
            {
                return null;
            }

            var job = await _jobOpeningRepository.GetJobOpeningByIdAsync(application.JobId);
            var citizen = await _citizenRepository.GetCitizenByIdAsync(application.CitizenId);
            var citizenUser = citizen != null ? await _userRepository.GetUserByIdAsync(citizen.UserId) : null;

            return new ApplicationDetailsDto
            {
                Application = application,
                Job = job,
                Citizen = citizen,
                CitizenUser = citizenUser
            };
        }

        public async Task<(bool Success, string Message)> ShortlistApplicationAsync(int applicationId, int employerId)
        {
            var application = await _applicationRepository.GetApplicationByIdAsync(applicationId);
            if (application == null)
            {
                return (false, "Application not found.");
            }

            // Verify employer owns this job
            var job = await _jobOpeningRepository.GetJobOpeningByIdAsync(application.JobId);
            if (job == null || job.EmployerId != employerId)
            {
                return (false, "Unauthorized access.");
            }

            if (application.Status == "Shortlisted")
            {
                return (false, "Application is already shortlisted.");
            }

            await _applicationRepository.ShortlistApplicationAsync(applicationId);

            // Notify citizen
            await _notificationService.CreateApplicationStatusNotificationAsync(applicationId, "Shortlisted");

            return (true, "Application shortlisted successfully!");
        }

        public async Task<(bool Success, string Message)> ApproveApplicationAsync(int applicationId, int employerId)
        {
            var application = await _applicationRepository.GetApplicationByIdAsync(applicationId);
            if (application == null)
            {
                return (false, "Application not found.");
            }

            // Verify employer owns this job
            var job = await _jobOpeningRepository.GetJobOpeningByIdAsync(application.JobId);
            if (job == null || job.EmployerId != employerId)
            {
                return (false, "Unauthorized access.");
            }

            if (application.Status == "Approved")
            {
                return (false, "Application is already approved.");
            }

            await _applicationRepository.ApproveApplicationAsync(applicationId);

            // Notify citizen
            await _notificationService.CreateApplicationStatusNotificationAsync(applicationId, "Approved");

            return (true, "Candidate hired successfully!");
        }

        public async Task<(bool Success, string Message)> RejectApplicationAsync(int applicationId, int employerId)
        {
            var application = await _applicationRepository.GetApplicationByIdAsync(applicationId);
            if (application == null)
            {
                return (false, "Application not found.");
            }

            // Verify employer owns this job
            var job = await _jobOpeningRepository.GetJobOpeningByIdAsync(application.JobId);
            if (job == null || job.EmployerId != employerId)
            {
                return (false, "Unauthorized access.");
            }

            if (application.Status == "Rejected")
            {
                return (false, "Application is already rejected.");
            }

            await _applicationRepository.RejectApplicationAsync(applicationId);

            // Decrement citizen's active applications
            await _citizenRepository.DecrementActiveApplicationsAsync(application.CitizenId);

            // Notify citizen
            await _notificationService.CreateApplicationStatusNotificationAsync(applicationId, "Rejected");

            return (true, "Application rejected.");
        }

        public async Task<IEnumerable<Application>> GetShortlistedApplicationsAsync(int employerId)
        {
            return await _applicationRepository.GetShortlistedApplicationsAsync(employerId);
        }

        public async Task<IEnumerable<Application>> GetPendingApplicationsAsync(int employerId)
        {
            var allApplications = await _applicationRepository.GetApplicationsByEmployerIdAsync(employerId);
            return allApplications.Where(a => a.Status == "Pending");
        }

        public async Task<int> GetPendingApplicationsCountAsync(int employerId)
        {
            return await _applicationRepository.GetPendingApplicationsCountAsync(employerId);
        }

        public async Task<bool> HasCitizenAppliedForJobAsync(int citizenId, int jobId)
        {
            return await _applicationRepository.HasCitizenAppliedForJobAsync(citizenId, jobId);
        }

        public async Task<bool> CanEmployerAccessApplicationAsync(int applicationId, int employerId)
        {
            var application = await _applicationRepository.GetApplicationByIdAsync(applicationId);
            if (application == null)
            {
                return false;
            }

            var job = await _jobOpeningRepository.GetJobOpeningByIdAsync(application.JobId);
            return job?.EmployerId == employerId;
        }
    }
}
