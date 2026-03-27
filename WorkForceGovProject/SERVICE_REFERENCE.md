# 📋 SERVICE & REPOSITORY CHEAT SHEET

## Service Method Quick Reference

### 🔐 AccountService
```csharp
// Login
await _accountService.LoginAsync(email, password);
// Returns: (bool success, string message, User user)

// Register
await _accountService.RegisterAsync(user);
// Returns: (bool success, string message)

// Get User
await _accountService.GetUserByIdAsync(userId);
await _accountService.GetUserByEmailAsync(email);

// Update
await _accountService.UpdateUserAsync(user);
await _accountService.ChangePasswordAsync(userId, oldPassword, newPassword);

// Check
await _accountService.UserExistsAsync(email);
```

### 👤 CitizenService
```csharp
// Get Profile
await _citizenService.GetCitizenByIdAsync(id);
await _citizenService.GetCitizenByUserIdAsync(userId);

// Create/Update
await _citizenService.CreateCitizenProfileAsync(userId, fullName, email);
await _citizenService.UpdateCitizenProfileAsync(citizen);
await _citizenService.UpdatePersonalInfoAsync(citizenId, fullName, dob, gender, address, phone);

// Dashboard
await _citizenService.GetDashboardStatsAsync(citizenId);
// Returns: (int activeApps, decimal balance, string docStatus, int jobMatches)
```

### 📄 DocumentService
```csharp
// Upload
await _documentService.UploadDocumentAsync(citizenId, documentType, file);
// Returns: (bool success, string message, CitizenDocument doc)

// Verify
await _documentService.VerifyDocumentAsync(documentId);
await _documentService.RejectDocumentAsync(documentId, reason);

// Get
await _documentService.GetCitizenDocumentsAsync(citizenId);
await _documentService.GetDocumentVerificationStatusAsync(citizenId);

// Delete
await _documentService.DeleteDocumentAsync(documentId);
```

### 💼 JobService
```csharp
// Search
await _jobService.GetOpenJobsAsync();
await _jobService.SearchJobsAsync(location, category);
await _jobService.GetJobsByLocationAsync(location);
await _jobService.GetJobsByCategoryAsync(category);
await _jobService.GetRecentJobsAsync(count);

// Manage
await _jobService.CreateJobAsync(job);
await _jobService.UpdateJobAsync(job);
await _jobService.CloseJobAsync(jobId);
```

### 📋 ApplicationService
```csharp
// Apply
await _applicationService.ApplyForJobAsync(citizenId, jobId, coverLetter);
// Returns: (bool success, string message, Application app)

// Get
await _applicationService.GetApplicationsByCitizenAsync(citizenId);
await _applicationService.GetApplicationsByJobAsync(jobId);
await _applicationService.GetApplicationByIdAsync(id);

// Status
await _applicationService.HasAppliedAsync(citizenId, jobId);
await _applicationService.WithdrawApplicationAsync(applicationId);

// Review
await _applicationService.ApproveApplicationAsync(applicationId);
await _applicationService.RejectApplicationAsync(applicationId);
```

### 🔔 NotificationService
```csharp
// Create
await _notificationService.CreateNotificationAsync(userId, message, category, entityId, entityType);

// Get
await _notificationService.GetUserNotificationsAsync(userId);
await _notificationService.GetUnreadNotificationsAsync(userId);
await _notificationService.GetUnreadCountAsync(userId);

// Mark
await _notificationService.MarkAsReadAsync(notificationId);
await _notificationService.MarkAllAsReadAsync(userId);

// Delete
await _notificationService.DeleteNotificationAsync(notificationId);
await _notificationService.DeleteOldNotificationsAsync(daysOld);
```

### 💰 BenefitService
```csharp
// Get Benefits
await _benefitService.GetCitizenBenefitsAsync(citizenId);
await _benefitService.GetActiveBenefitsAsync(citizenId);
await _benefitService.GetBenefitByIdAsync(id);

// Allocate
await _benefitService.AllocateBenefitAsync(citizenId, programId, benefitType, amount);

// Programs
await _benefitService.GetAllProgramsAsync();
await _benefitService.CreateProgramAsync(program);
await _benefitService.UpdateProgramAsync(program);

// Stats
await _benefitService.GetTotalBenefitAmountAsync(citizenId);
await _benefitService.GetActiveBenefitCountAsync(citizenId);
```

---

## Repository Methods Quick Reference

### Generic Repository<T>
```csharp
// GET
GetByIdAsync(id)
GetAllAsync()
FindAsync(predicate)
FirstOrDefaultAsync(predicate)
AnyAsync(predicate)
CountAsync()
CountAsync(predicate)
GetPagedAsync(pageNumber, pageSize)

// CREATE
AddAsync(entity)
AddRangeAsync(entities)

// UPDATE
UpdateAsync(entity)
UpdateRangeAsync(entities)

// DELETE
DeleteAsync(id)
DeleteAsync(entity)
DeleteRangeAsync(entities)
```

### UnitOfWork
```csharp
// Generic Repositories
Users
Citizens
CitizenDocuments
JobOpenings
Applications
Benefits
EmploymentPrograms
Notifications

// Specific Repositories
UserRepository
CitizenRepository
JobRepository
ApplicationRepository
NotificationRepository

// Transactions
SaveChangesAsync()
BeginTransactionAsync()
CommitTransactionAsync()
RollbackTransactionAsync()
```

---

## Controller Usage Examples

### Login Flow
```csharp
[HttpPost]
public async Task<IActionResult> Login(string email, string password)
{
    var (success, message, user) = await _accountService.LoginAsync(email, password);
    
    if (success)
    {
        HttpContext.Session.SetInt32("UserId", user.Id);
        HttpContext.Session.SetString("UserName", user.FullName);
        return RedirectToAction("Dashboard", "Citizen");
    }
    
    ViewBag.Error = message;
    return View();
}
```

### Dashboard Display
```csharp
[HttpGet]
public async Task<IActionResult> Dashboard()
{
    int? userId = HttpContext.Session.GetInt32("UserId");
    var citizen = await _citizenService.GetCitizenByUserIdAsync(userId.Value);
    
    if (citizen == null) return RedirectToAction("Login", "Account");
    
    await _citizenService.UpdateDashboardStatsAsync(citizen.Id);
    return View(citizen);
}
```

### Job Application
```csharp
[HttpPost]
public async Task<IActionResult> ApplyForJob(int jobId, string coverLetter)
{
    var citizen = await GetLoggedInCitizen();
    var (success, message, app) = await _applicationService
        .ApplyForJobAsync(citizen.Id, jobId, coverLetter);
    
    TempData[success ? "Success" : "Error"] = message;
    return RedirectToAction("JobApplications");
}
```

### Document Upload
```csharp
[HttpPost]
public async Task<IActionResult> UploadDocument(IFormFile file, string documentType)
{
    var citizen = await GetLoggedInCitizen();
    var (success, message, doc) = await _documentService
        .UploadDocumentAsync(citizen.Id, documentType, file);
    
    TempData[success ? "Success" : "Error"] = message;
    return RedirectToAction("Documents");
}
```

---

## Common Code Patterns

### Pattern: Get and Update
```csharp
var entity = await _unitOfWork.MyEntities.GetByIdAsync(id);
if (entity == null) return NotFound();

entity.Property = newValue;
await _unitOfWork.MyEntities.UpdateAsync(entity);
var saved = await _unitOfWork.SaveChangesAsync();
```

### Pattern: Service Response
```csharp
var (success, message) = await _service.DoSomethingAsync();
if (!success)
{
    ViewBag.Error = message;
    return View();
}
return RedirectToAction("Index");
```

### Pattern: Get with Filter
```csharp
var items = await _unitOfWork.MyItems.FindAsync(x => 
    x.Status == "Active" && x.UserId == userId);
```

### Pattern: Pagination
```csharp
var pageSize = 10;
var items = await _unitOfWork.MyItems.GetPagedAsync(pageNumber, pageSize);
```

### Pattern: Check Exists
```csharp
var exists = await _unitOfWork.Users.AnyAsync(u => u.Email == email);
if (exists)
    return (false, "Email already exists");
```

---

## Error Handling

### Service Response Pattern
```csharp
public async Task<(bool Success, string Message)> DoSomethingAsync()
{
    try
    {
        // Operation
        await _unitOfWork.SaveChangesAsync();
        return (true, "Operation successful");
    }
    catch (Exception ex)
    {
        _logger.LogError($"Error: {ex.Message}");
        return (false, "An error occurred");
    }
}
```

### Controller Error Handling
```csharp
public async Task<IActionResult> MyAction()
{
    try
    {
        var result = await _service.GetAsync(id);
        return Ok(result);
    }
    catch (InvalidOperationException ex)
    {
        return BadRequest(ex.Message);
    }
    catch (Exception ex)
    {
        return StatusCode(500, "Internal server error");
    }
}
```

---

## Session Management

```csharp
// Set
HttpContext.Session.SetInt32("UserId", user.Id);
HttpContext.Session.SetString("UserName", user.FullName);

// Get
int? userId = HttpContext.Session.GetInt32("UserId");
string userName = HttpContext.Session.GetString("UserName");

// Clear
HttpContext.Session.Clear();
```

---

**Print this for quick reference!** 🖨️
