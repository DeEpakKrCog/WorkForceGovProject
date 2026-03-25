using WorkForceGovProject.Data;
using WorkForceGovProject.Models;

namespace WorkForceGovProject.Data
{
    public static class DbSeeder
    {
        public static void SeedDatabase(ApplicationDbContext context)
        {
            // Check if data already exists
            if (context.Users.Any())
            {
                return; // Database has been seeded
            }

            // 1. Create Test Users
            var employerUser = new User
            {
                FullName = "Tech Solutions Inc",
                Email = "employer@test.com",
                Password = "Test123!",
                Role = "Employer"
            };

            var citizenUser1 = new User
            {
                FullName = "John Smith",
                Email = "john@test.com",
                Password = "Test123!",
                Role = "Citizen"
            };

            var citizenUser2 = new User
            {
                FullName = "Sarah Johnson",
                Email = "sarah@test.com",
                Password = "Test123!",
                Role = "Citizen"
            };

            var citizenUser3 = new User
            {
                FullName = "Michael Brown",
                Email = "michael@test.com",
                Password = "Test123!",
                Role = "Citizen"
            };

            context.Users.AddRange(employerUser, citizenUser1, citizenUser2, citizenUser3);
            context.SaveChanges();

            // 2. Create Employer Profile
            var employer = new Employer
            {
                UserId = employerUser.Id,
                CompanyName = "Tech Solutions Inc",
                Industry = "Information Technology",
                Address = "123 Silicon Valley Blvd, San Francisco, CA 94105",
                ContactInfo = "+1-555-0123",
                RegistrationDate = DateTime.Now.AddMonths(-3),
                Status = "Verified"
            };

            context.Employers.Add(employer);
            context.SaveChanges();

            // 3. Create Citizens
            var citizen1 = new Citizen
            {
                UserId = citizenUser1.Id,
                FullName = "John Smith",
                Email = "john@test.com",
                ActiveApplications = 2,
                AccountBalance = 1500.00m,
                DocumentStatus = "Verified",
                NewJobMatches = 5
            };

            var citizen2 = new Citizen
            {
                UserId = citizenUser2.Id,
                FullName = "Sarah Johnson",
                Email = "sarah@test.com",
                ActiveApplications = 1,
                AccountBalance = 2000.00m,
                DocumentStatus = "Verified",
                NewJobMatches = 3
            };

            var citizen3 = new Citizen
            {
                UserId = citizenUser3.Id,
                FullName = "Michael Brown",
                Email = "michael@test.com",
                ActiveApplications = 1,
                AccountBalance = 800.00m,
                DocumentStatus = "Pending",
                NewJobMatches = 8
            };

            context.Citizens.AddRange(citizen1, citizen2, citizen3);
            context.SaveChanges();

            // 4. Create Job Openings
            var job1 = new JobOpening
            {
                EmployerId = employer.Id,
                Title = "Senior Software Developer",
                Description = "We are looking for an experienced software developer to join our team. You will work on cutting-edge projects using the latest technologies.",
                Location = "San Francisco, CA (Hybrid)",
                Salary = 120000.00m,
                Requirements = "5+ years experience in C#, .NET Core, SQL Server, Azure. Bachelor's degree in Computer Science required.",
                PostedDate = DateTime.Now.AddDays(-15),
                ApplicationDeadline = DateTime.Now.AddDays(15),
                Status = "Open"
            };

            var job2 = new JobOpening
            {
                EmployerId = employer.Id,
                Title = "Frontend Developer",
                Description = "Join our frontend team to build beautiful and responsive user interfaces using modern web technologies.",
                Location = "Remote",
                Salary = 90000.00m,
                Requirements = "3+ years experience with React, TypeScript, HTML5, CSS3. Portfolio required.",
                PostedDate = DateTime.Now.AddDays(-10),
                ApplicationDeadline = DateTime.Now.AddDays(20),
                Status = "Open"
            };

            var job3 = new JobOpening
            {
                EmployerId = employer.Id,
                Title = "DevOps Engineer",
                Description = "Looking for a DevOps engineer to manage our cloud infrastructure and CI/CD pipelines.",
                Location = "San Francisco, CA",
                Salary = 110000.00m,
                Requirements = "Experience with Azure, Docker, Kubernetes, Jenkins. Strong scripting skills required.",
                PostedDate = DateTime.Now.AddDays(-20),
                ApplicationDeadline = DateTime.Now.AddDays(10),
                Status = "Open"
            };

            var job4 = new JobOpening
            {
                EmployerId = employer.Id,
                Title = "Junior Developer",
                Description = "Entry-level position for recent graduates. Great learning opportunity!",
                Location = "San Francisco, CA",
                Salary = 65000.00m,
                Requirements = "Bachelor's degree in Computer Science. Fresh graduates welcome.",
                PostedDate = DateTime.Now.AddDays(-30),
                Status = "Closed"
            };

            context.JobOpenings.AddRange(job1, job2, job3, job4);
            context.SaveChanges();

            // 5. Create Applications
            var app1 = new Application
            {
                CitizenId = citizen1.Id,
                JobId = job1.Id,
                SubmittedDate = DateTime.Now.AddDays(-12),
                Status = "Shortlisted"
            };

            var app2 = new Application
            {
                CitizenId = citizen2.Id,
                JobId = job1.Id,
                SubmittedDate = DateTime.Now.AddDays(-10),
                Status = "Pending"
            };

            var app3 = new Application
            {
                CitizenId = citizen1.Id,
                JobId = job2.Id,
                SubmittedDate = DateTime.Now.AddDays(-8),
                Status = "Approved"
            };

            var app4 = new Application
            {
                CitizenId = citizen3.Id,
                JobId = job2.Id,
                SubmittedDate = DateTime.Now.AddDays(-6),
                Status = "Pending"
            };

            var app5 = new Application
            {
                CitizenId = citizen2.Id,
                JobId = job3.Id,
                SubmittedDate = DateTime.Now.AddDays(-4),
                Status = "Rejected"
            };

            context.Applications.AddRange(app1, app2, app3, app4, app5);
            context.SaveChanges();

            // 6. Create Notifications
            var notif1 = new Notification
            {
                UserId = employerUser.Id,
                EmployerId = employer.Id,
                EntityId = app2.Id,
                Message = "New application received for Senior Software Developer from Sarah Johnson",
                Category = "Job Application",
                Status = "Unread",
                CreatedDate = DateTime.Now.AddDays(-10)
            };

            var notif2 = new Notification
            {
                UserId = employerUser.Id,
                EmployerId = employer.Id,
                EntityId = app4.Id,
                Message = "New application received for Frontend Developer from Michael Brown",
                Category = "Job Application",
                Status = "Unread",
                CreatedDate = DateTime.Now.AddDays(-6)
            };

            var notif3 = new Notification
            {
                UserId = employerUser.Id,
                EmployerId = employer.Id,
                Message = "Your company documents have been verified successfully!",
                Category = "Document Verification",
                Status = "Read",
                CreatedDate = DateTime.Now.AddMonths(-2)
            };

            var notif4 = new Notification
            {
                UserId = citizenUser1.Id,
                EntityId = app1.Id,
                Message = "Your application for Senior Software Developer has been shortlisted!",
                Category = "Application Update",
                Status = "Read",
                CreatedDate = DateTime.Now.AddDays(-5)
            };

            context.Notifications.AddRange(notif1, notif2, notif3, notif4);
            context.SaveChanges();

            // 7. Create Employer Documents
            var doc1 = new EmployerDocument
            {
                EmployerId = employer.Id,
                DocType = "Business License",
                FileURL = "/uploads/documents/sample-license.pdf",
                UploadedDate = DateTime.Now.AddMonths(-2),
                VerificationStatus = "Verified"
            };

            var doc2 = new EmployerDocument
            {
                EmployerId = employer.Id,
                DocType = "Company Registration",
                FileURL = "/uploads/documents/sample-registration.pdf",
                UploadedDate = DateTime.Now.AddMonths(-2),
                VerificationStatus = "Verified"
            };

            context.EmployerDocuments.AddRange(doc1, doc2);
            context.SaveChanges();
        }
    }
}
