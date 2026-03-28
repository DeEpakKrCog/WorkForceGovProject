using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WorkForceGovProject.Data;
using WorkForceGovProject.Models;
using Microsoft.EntityFrameworkCore;

namespace YourProject.Controllers
{
    public class LaborOfficerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LaborOfficerController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard()
        {
            ViewBag.PendingDocuments = 12;
            ViewBag.ComplianceAlerts = 4;
            ViewBag.PendingApplications = 20;
            ViewBag.ApprovedApplications = 45;
            ViewBag.RejectedApplications = 6;

            ViewBag.Notifications = new List<string>
            {
                "New document pending verification",
                "Employer compliance violation detected"
            };

            return View();
        }

        public IActionResult PendingDocuments()
        {
            var pendingDocs = _context.CitizenDocuments
                .Include(d => d.Citizen)
                .Where(d => d.VerificationStatus == "Pending")
                .ToList();

            return View("DocumentVerifications", pendingDocs);
        }

        public IActionResult DocumentVerifications()
        {
            var pendingDocs = _context.CitizenDocuments
                .Include(d => d.Citizen)
                .Where(d => d.VerificationStatus == "Pending")
                .ToList();

            return View(pendingDocs);
        }

        [HttpPost]
        public IActionResult ApproveDocument(int documentId)
        {
            var document = _context.CitizenDocuments.Find(documentId);
            if (document != null)
            {
                document.VerificationStatus = "Verified";
                document.VerificationNotes = "Approved by officer on " + DateTime.Now.ToString();
                _context.SaveChanges();
            }
            return RedirectToAction("DocumentVerifications");
        }

        [HttpPost]
        public IActionResult RejectDocument(int documentId, string notes)
        {
            var document = _context.CitizenDocuments.Find(documentId);
            if (document != null)
            {
                document.VerificationStatus = "Rejected";
                document.VerificationNotes = notes ?? "Rejected by officer on " + DateTime.Now.ToString();
                _context.SaveChanges();
            }
            return RedirectToAction("DocumentVerifications");
        }

        public IActionResult Applications()
        {
            var applications = _context.JobApplications
                .Include(a => a.Citizen)
                .Where(a => a.Status == "Pending" || a.Status == "UnderReview")
                .OrderByDescending(a => a.ApplicationDate)
                .ToList();

            return View(applications);
        }

        [HttpPost]
        public IActionResult ApproveApplication(int applicationId)
        {
            var application = _context.JobApplications.Find(applicationId);
            if (application != null)
            {
                application.Status = "Approved";
                application.ReviewedDate = DateTime.Now;
                application.ReviewedBy = "Labor Officer";
                application.OfficerNotes = "Application approved on " + DateTime.Now.ToString();
                _context.SaveChanges();
            }
            return RedirectToAction("Applications");
        }

        [HttpPost]
        public IActionResult RejectApplication(int applicationId, string notes)
        {
            var application = _context.JobApplications.Find(applicationId);
            if (application != null)
            {
                application.Status = "Rejected";
                application.ReviewedDate = DateTime.Now;
                application.ReviewedBy = "Labor Officer";
                application.OfficerNotes = notes ?? "Application rejected on " + DateTime.Now.ToString();
                _context.SaveChanges();
            }
            return RedirectToAction("Applications");
        }

        [HttpPost]
        public IActionResult SetUnderReview(int applicationId)
        {
            var application = _context.JobApplications.Find(applicationId);
            if (application != null)
            {
                application.Status = "UnderReview";
                _context.SaveChanges();
            }
            return RedirectToAction("Applications");
        }

        public IActionResult Compliance()
        {
            return View();
        }

        public IActionResult Employers()
        {
            return View();
        }

        public IActionResult Reports()
        {
            return View();
        }

        public IActionResult Audits()
        {
            return View();
        }
    }
}
