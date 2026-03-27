# 🚀 Citizen Module Setup Guide

## Prerequisites
- Visual Studio 2022+
- .NET 10 SDK
- SQL Server LocalDB or SQL Server Express
- Entity Framework Core Tools

## Step 1: Update Database

### Option A: Using Package Manager Console
```powershell
# Open Package Manager Console (Tools → NuGet Package Manager → Package Manager Console)

# Create a new migration
Add-Migration AddCitizenModuleSchema

# Apply migration to database
Update-Database
```

### Option B: Using .NET CLI
```bash
# Create migration
dotnet ef migrations add AddCitizenModuleSchema

# Update database
dotnet ef database update
```

### Option C: Manual SQL (if migrations fail)
```sql
-- Run these SQL commands against your database
-- (Tables will be created by EF Core)

-- Verify tables created:
SELECT name FROM sys.tables;
```

## Step 2: Verify Build

```bash
# Clean solution
dotnet clean

# Build solution
dotnet build

# You should see: "Build succeeded"
```

## Step 3: Run the Application

```bash
# Start the application
dotnet run

# Or use Visual Studio → F5 or Debug → Start Debugging
```

## Step 4: Test the Module

### Register a Citizen
1. Navigate to: `https://localhost:5001/Account/Register`
2. Fill in form:
   - Full Name: John Doe
   - Email: john@example.com
   - Password: Test@1234
   - Role: **Citizen** ← Important!
3. Click "Create Account"
4. Should redirect to Login page

### Login
1. Navigate to: `https://localhost:5001/Account/Login`
2. Enter credentials from registration
3. Click "Sign In"
4. Should redirect to: `https://localhost:5001/Citizen/Dashboard`

### Test Dashboard
- [ ] See "Welcome, John Doe"
- [ ] See 4 stat cards (0 values initially)
- [ ] See quick action buttons
- [ ] Click sidebar navigation links

### Test Profile
1. Click "👤 My Profile" in sidebar
2. Update fields:
   - Full Name (change)
   - Date of Birth (set)
   - Gender (select)
   - Phone Number (enter)
   - Address (enter)
3. Click "Save Changes"
4. See "Profile updated successfully!" message

### Test Documents
1. Click "📄 Verification Docs" in sidebar
2. Click "Upload New Document"
3. Select: "Identity Proof (Aadhar, Passport, DL)"
4. Choose a file (any PDF or image file)
5. Click "Upload Document"
6. Should see document in list with "Pending" status

### Test Job Search
1. Click "🔍 Job Board" in sidebar
2. Enter filter (optional)
3. See "No jobs found" (no test data yet)
4. OR add test job data first (see below)

### Test Job Application
1. After adding test job data (see below)
2. Go to Job Board
3. Click "Apply Now"
4. Check "📋 My Applications" - should appear with "Pending" status
5. Check "🔔 Notifications" - should show application notification

### Test Notifications
1. Click "🔔 Notifications" in sidebar
2. See notifications from job applications
3. Click "Mark as Read"
4. Notification background changes

## Step 5: Add Test Data (Optional)

### Add Sample Job via Database

```sql
-- Insert sample job
INSERT INTO JobOpenings (JobTitle, Description, EmployerId, Location, JobCategory, SalaryMin, SalaryMax, PostedDate, Status, TotalApplications)
VALUES 
('Senior Software Engineer', 'Develop and maintain web applications using .NET', 1, 'Mumbai', 'IT', 800000, 1200000, GETDATE(), 'Open', 0),
('Data Analyst', 'Analyze business data and create reports', 1, 'Bangalore', 'Finance', 600000, 900000, GETDATE(), 'Open', 0),
('QA Engineer', 'Test software applications and report bugs', 1, 'Delhi', 'IT', 500000, 750000, GETDATE(), 'Open', 0);
```

### Add Sample Employment Program

```sql
-- Insert sample employment program
INSERT INTO EmploymentPrograms (ProgramName, Description, ProgramType, TotalBudget, StartDate, Status)
VALUES 
('Skill Development Program', 'Free technical training for job seekers', 'Training', 5000000, GETDATE(), 'Active'),
('Youth Employment Subsidy', 'Cash assistance for first-time job seekers', 'Cash', 10000000, GETDATE(), 'Active');
```

### Add Sample Benefit

```sql
-- First, get a citizen ID (substitute your actual citizen ID)
-- Then insert benefit:
INSERT INTO Benefits (ProgramId, CitizenId, BenefitType, Amount, BenefitDate, Status)
VALUES 
(1, 1, 'Training', 50000, GETDATE(), 'Active');
```

## Troubleshooting

### Issue: "Add-Migration" command not found
**Solution:** Install EF Core Tools
```powershell
dotnet tool install --global dotnet-ef
```

### Issue: Database connection error
**Solution:** Check `appsettings.json`
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=WorkForceGovDb;Trusted_Connection=true;"
}
```

### Issue: "Object reference not set to an instance"
**Solution:** Ensure:
- User is logged in (session exists)
- Database migration was run
- Citizen record exists for user

### Issue: File upload fails
**Solution:** Ensure `wwwroot/documents` folder exists
```bash
mkdir wwwroot\documents
```

### Issue: Views not updating
**Solution:** Clear cache and rebuild
```powershell
# Package Manager Console
Remove-Migration -Force
Add-Migration AddCitizenModuleSchema
Update-Database
```

## File Upload Configuration

Documents are uploaded to: `wwwroot/documents/`

**Allowed extensions:** `.pdf, .doc, .docx, .jpg, .jpeg, .png`

**File naming:** `{CitizenId}_{DocumentType}_{Timestamp}.{ext}`

Example: `1_Identity_Proof_637456789123456789.pdf`

## Database Schema Overview

```
Users
├── Id (PK)
├── FullName
├── Email
├── Password
└── Role

Citizens
├── Id (PK)
├── UserId (FK → Users)
├── FullName
├── Email
├── DOB
├── Gender
├── Address
├── PhoneNumber
├── ActiveApplications
├── AccountBalance
├── DocumentStatus
└── NewJobMatches

CitizenDocuments
├── Id (PK)
├── CitizenId (FK → Citizens)
├── DocumentType
├── FileName
├── FilePath
├── UploadedDate
├── VerificationStatus
├── RejectionReason
└── VerificationDate

JobOpenings
├── Id (PK)
├── JobTitle
├── Description
├── EmployerId
├── Location
├── JobCategory
├── SalaryMin
├── SalaryMax
├── PostedDate
├── ClosingDate
├── Status
└── TotalApplications

Applications
├── Id (PK)
├── CitizenId (FK → Citizens)
├── JobOpeningId (FK → JobOpenings)
├── SubmittedDate
├── Status
├── CoverLetter
├── ReviewedDate
└── ReviewNotes

EmploymentPrograms
├── Id (PK)
├── ProgramName
├── Description
├── ProgramType
├── TotalBudget
├── StartDate
├── EndDate
└── Status

Benefits
├── Id (PK)
├── ProgramId (FK → EmploymentPrograms)
├── CitizenId (FK → Citizens)
├── BenefitType
├── Amount
├── BenefitDate
├── Status
└── Description

Notifications
├── Id (PK)
├── UserId (FK → Users)
├── Message
├── Category
├── EntityId
├── EntityType
├── CreatedDate
├── IsRead
└── Status
```

## API Endpoints Reference

### Citizen Module Routes
```
GET  /Citizen/Dashboard           - View dashboard
GET  /Citizen/Profile             - View profile
POST /Citizen/UpdateProfile       - Update profile
GET  /Citizen/Documents           - List documents
POST /Citizen/UploadDocument      - Upload document
POST /Citizen/DeleteDocument      - Delete document
GET  /Citizen/JobSearch           - Search jobs
GET  /Citizen/JobApplications     - List applications
POST /Citizen/ApplyForJob         - Apply for job
POST /Citizen/WithdrawApplication - Withdraw application
GET  /Citizen/Benefits            - View benefits
GET  /Citizen/Notifications       - View notifications
POST /Citizen/MarkNotificationAsRead - Mark notification as read
```

## Performance Tips

1. **Session Caching:** User ID is cached in session
2. **Lazy Loading:** Use `.Include()` for related data
3. **Pagination:** Consider adding pagination for large lists
4. **Indexing:** Add database indexes on:
   - UserId in Citizens
   - CitizenId in Documents, Applications, Benefits
   - JobOpeningId in Applications

## Security Checklist

- [x] Authentication required (session-based)
- [x] Anti-forgery tokens on POST/PUT/DELETE
- [x] User data isolation
- [x] File upload validation
- [x] Input sanitization
- [ ] HTTPS enabled (configure in production)
- [ ] Password hashing (review hash algorithm)
- [ ] Rate limiting (implement if needed)
- [ ] Audit logging (implement if needed)

## Next Steps

1. ✅ Run migrations
2. ✅ Test citizen registration & login
3. ✅ Test all module pages
4. ✅ Add test data
5. ✅ Verify file uploads
6. 🔲 Implement Employer module
7. 🔲 Implement Admin module
8. 🔲 Add email notifications
9. 🔲 Deploy to production

---

**Happy Testing! 🎉**

If you encounter any issues, check the build output and browser developer console (F12) for error details.
