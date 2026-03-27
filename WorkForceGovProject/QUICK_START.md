# 🚀 QUICK START GUIDE - CITIZEN MODULE

## Prerequisites
- Visual Studio 2026 or higher
- .NET 10 SDK
- SQL Server
- Git (for version control)

---

## ⚡ 5-Minute Setup

### Step 1: Database Migration (2 min)
```powershell
# Open Package Manager Console
# In Visual Studio: Tools → NuGet Package Manager → Package Manager Console

# Run migration
Add-Migration AddCitizenModule
Update-Database
```

### Step 2: Build Project (1 min)
```bash
# In terminal
dotnet build
```

### Step 3: Run Application (1 min)
```bash
# In terminal
dotnet run
```

### Step 4: Access Application (1 min)
```
Open browser: https://localhost:5001
```

---

## 📋 Testing the Module

### Test 1: Register Citizen (2 min)
1. Click "Register" on home page
2. Fill in:
   - Full Name: `John Doe`
   - Email: `john@example.com`
   - DOB: `01/01/1990`
   - Gender: `Male`
   - Phone: `9876543210`
   - Address: `123 Main St, City`
   - Password: `Password123!`
3. Click "Register as Citizen"
4. ✅ Should see "Registration Successful" message

### Test 2: Login (1 min)
1. Click "Login"
2. Enter: `john@example.com` / `Password123!`
3. Click "Login"
4. ✅ Should see Dashboard with "Welcome, John Doe!"

### Test 3: Update Profile (2 min)
1. Click "Update Profile" button
2. Change phone number
3. Click "Save Changes"
4. ✅ Should see success message

### Test 4: Upload Document (3 min)
1. Click "Upload Documents" button
2. Select document type: `Identity Proof`
3. Upload a PDF file (max 5MB)
4. Click "Upload Document"
5. ✅ Document should appear in list with "Pending" status

### Test 5: Search & Apply for Jobs (5 min)
1. Click "Search Jobs" button
2. View available jobs
3. Click "Apply" on any job
4. ✅ Should see confirmation

### Test 6: Track Applications (2 min)
1. Click "View Applications" button
2. ✅ Should see your application with status

### Test 7: View Benefits (1 min)
1. Click "View Benefits" button
2. ✅ Should see benefits page (may be empty if none allocated)

---

## 🎯 Key Features to Test

| Feature | Path | Expected Result |
|---------|------|-----------------|
| Registration | `/Account/Register` | New user created |
| Login | `/Account/Login` | Session started |
| Dashboard | `/Citizen/Dashboard` | Stats displayed |
| Profile | `/Citizen/Profile` | Can update info |
| Documents | `/Citizen/Documents` | Can upload files |
| Job Search | `/Citizen/JobSearch` | Jobs displayed |
| Applications | `/Citizen/JobApplications` | Applications listed |
| Benefits | `/Citizen/Benefits` | Benefits displayed |

---

## 📁 File Structure Quick Reference

```
Main Files for Citizen Module:

Models/
  ├── User.cs ..................... User credentials
  ├── Citizen.cs .................. Citizen profile
  ├── CitizenDocument.cs .......... Document uploads
  ├── Application.cs .............. Job applications
  ├── Benefit.cs .................. Government benefits
  ├── JobOpening.cs ............... Job postings
  └── EmploymentProgram.cs ........ Programs

Controllers/
  ├── AccountController.cs ........ Login/Register
  └── CitizenController.cs ........ All citizen actions

Services/
  ├── AccountService.cs .......... Authentication
  ├── CitizenService.cs .......... Profile management
  ├── DocumentService.cs ......... Document upload
  ├── JobService.cs .............. Job search
  ├── ApplicationService.cs ...... Applications
  ├── BenefitService.cs .......... Benefit tracking
  └── NotificationService.cs .... Notifications

Repositories/
  ├── UserRepository.cs .......... User queries
  ├── CitizenRepository.cs ....... Citizen queries
  ├── JobRepository.cs ........... Job queries
  └── ApplicationRepository.cs ... Application queries

Views/
  ├── Account/
  │   ├── Login.cshtml ........... Login page
  │   └── Register.cshtml ........ Registration page
  └── Citizen/
      ├── Dashboard.cshtml ....... Dashboard
      ├── Profile.cshtml ......... Profile page
      ├── Documents.cshtml ....... Document upload
      ├── JobSearch.cshtml ....... Job search
      ├── JobApplications.cshtml . Applications
      └── Benefits.cshtml ........ Benefits page
```

---

## 🔧 Common Commands

### Build & Run
```bash
dotnet build
dotnet run
```

### Database
```powershell
# Add migration
Add-Migration MigrationName

# Update database
Update-Database

# Drop & recreate
Drop-Database
Update-Database
```

### View Database
```
# In Visual Studio
View → SQL Server Object Explorer
Navigate to: LocalDB → Databases → WorkForceGovDb
```

---

## 🧪 Sample Data

### Test Citizen Account
```
Email: citizen@test.com
Password: Citizen@123
Name: Test Citizen
```

### Test Job (Create in DB)
```
Title: Software Developer
Location: New York
Category: IT
Description: Develop software applications
Status: Open
```

---

## ⚙️ Configuration Files

### appsettings.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=WorkForceGovDb;Trusted_Connection=true;TrustServerCertificate=true;"
  }
}
```

### Program.cs (Main Changes)
- Session timeout: 30 minutes
- DI configuration for services
- Repository registration

---

## 🐛 Troubleshooting

### Error: "Database not found"
```powershell
# Solution:
Update-Database
```

### Error: "wwwroot not found"
```
# Solution: Create folder
mkdir wwwroot/documents
```

### Error: "Session data lost"
```
# Check Program.cs
app.UseSession() is configured before app.UseAuthorization()
```

### Error: "File upload fails"
```
# Check file size < 5MB
# Check file type is: pdf, doc, docx, jpg, jpeg, png
# Check wwwroot/documents folder exists
```

---

## 📊 Testing Scenarios

### Scenario 1: Complete User Journey (10 min)
1. Register as citizen ✅
2. Login ✅
3. Update profile ✅
4. Upload documents ✅
5. Search jobs ✅
6. Apply for job ✅
7. View application ✅
8. View benefits ✅

### Scenario 2: Document Upload & Verification (5 min)
1. Login
2. Go to Documents page
3. Upload document
4. Verify upload successful
5. Try to delete (should work if pending)

### Scenario 3: Job Application Workflow (5 min)
1. Login
2. Go to Job Search
3. Search by location/category
4. View job details
5. Apply for job
6. View application status
7. Check notification

---

## 🎓 Learning Resources

### Key Concepts
- **MVC Pattern**: Model-View-Controller architecture
- **Dependency Injection**: Services injected in controllers
- **Async/Await**: Non-blocking database operations
- **Repository Pattern**: Data access abstraction
- **Session Management**: User session handling

### Files to Study
1. `AccountController.cs` - Authentication flow
2. `CitizenController.cs` - Business logic
3. `AccountService.cs` - Service pattern
4. `Repository.cs` - Generic repository pattern
5. `Dashboard.cshtml` - Razor view syntax

---

## 📈 Next Steps

### After Setup
1. ✅ Run the application
2. ✅ Test all features
3. ✅ Create sample data
4. ✅ Verify database persistence
5. ✅ Test all pages

### For Development
1. Add custom business logic
2. Create admin features
3. Add employer functionality
4. Implement email notifications
5. Add advanced search filters

### For Production
1. Enable HTTPS
2. Configure proper database
3. Setup email service
4. Enable logging
5. Configure backup strategy

---

## 📞 Support

### Documentation Files
- `CITIZEN_MODULE_COMPLETE.md` - Full module documentation
- `LAYERED_ARCHITECTURE.md` - Architecture details
- `SERVICE_REFERENCE.md` - Service methods reference
- `DATABASE_MIGRATION.md` - Database setup

### Quick Help
- **Login Issues?** Check `TROUBLESHOOTING.md`
- **Database Issues?** Check `DATABASE_MIGRATION.md`
- **Service Usage?** Check `SERVICE_REFERENCE.md`

---

## ✅ Verification Checklist

Before considering setup complete:

- [ ] Application builds without errors
- [ ] Database migration successful
- [ ] Application starts (dotnet run)
- [ ] Can register as citizen
- [ ] Can login with registered account
- [ ] Dashboard displays correctly
- [ ] Can upload documents
- [ ] Can search jobs
- [ ] Can apply for jobs
- [ ] Can view applications
- [ ] Can view benefits

---

## 🎉 You're All Set!

The Citizen/Job Seeker Module is now fully set up and ready to use.

**Status:** ✅ READY FOR TESTING

**Build:** ✅ SUCCESS

**Database:** ✅ CONFIGURED

**Module:** ✅ FULLY FUNCTIONAL

---

**Happy Testing!** 🚀

For detailed information, refer to individual documentation files.
