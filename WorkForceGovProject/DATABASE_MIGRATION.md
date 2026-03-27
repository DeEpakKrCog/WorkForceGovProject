# 🗄️ DATABASE MIGRATION & SETUP GUIDE

## ✅ Prerequisites

- Visual Studio 2026 (or higher)
- .NET 10 SDK
- SQL Server (local or remote)
- Package Manager Console access

---

## Step 1: Verify Connection String

Check your `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=WorkForceGovDb;Trusted_Connection=true;TrustServerCertificate=true;"
  }
}
```

Adjust server name if using:
- **Local SQL Server**: `.` or `(localdb)\\mssqllocaldb`
- **Remote Server**: `your-server.database.windows.net`
- **SQL Express**: `.\\SQLEXPRESS`

---

## Step 2: Open Package Manager Console

In Visual Studio:
1. Go to **Tools** → **NuGet Package Manager** → **Package Manager Console**
2. Ensure "WorkForceGovProject" is selected as Default Project

---

## Step 3: Add Migration

Run this command in Package Manager Console:

```powershell
Add-Migration AddServiceLayer
```

This will:
- ✅ Analyze the DbContext (ApplicationDbContext.cs)
- ✅ Compare current models with last migration
- ✅ Generate migration files
- ✅ Create Migrations folder with timestamp

**Output:**
```
Build started...
Build succeeded.
To undo this action, use Remove-Migration.
```

---

## Step 4: Update Database

Run this command:

```powershell
Update-Database
```

This will:
- ✅ Create or update the database
- ✅ Run all pending migrations
- ✅ Create all tables
- ✅ Set up relationships and constraints

**Expected Output:**
```
Build started...
Build succeeded.
Applying migration '20240115123456_AddServiceLayer'.
Done.
```

---

## Step 5: Verify Database

### Option A: SQL Server Management Studio
1. Open SQL Server Management Studio
2. Connect to your server
3. Navigate to **Databases** → **WorkForceGovDb**
4. Expand **Tables** and verify all tables are created:
   - Users
   - Citizens
   - CitizenDocuments
   - JobOpenings
   - Applications
   - Benefits
   - EmploymentPrograms
   - Notifications

### Option B: Visual Studio
1. Go to **View** → **SQL Server Object Explorer**
2. Right-click on your connection
3. Navigate to the database and tables

---

## Step 6: Build & Run

### Build
```bash
dotnet build
```

Or in Visual Studio:
- **Build** → **Rebuild Solution**

### Run
```bash
dotnet run
```

Or in Visual Studio:
- Press **F5** or **Debug** → **Start Debugging**

### Navigate
- Open browser to: `https://localhost:5001`
- Or use the URL shown in terminal

---

## Step 7: Test the Application

### 1. Register as Citizen
- Navigate to: `/Account/Register`
- Enter:
  - Full Name: `John Doe`
  - Email: `john@example.com`
  - Password: `Password123!`
  - Role: `Citizen`
- Click **Register**

### 2. Login
- Navigate to: `/Account/Login`
- Enter credentials from registration
- Click **Login**

### 3. Access Dashboard
- Should redirect to: `/Citizen/Dashboard`
- Verify stats display correctly

### 4. Test Features
- **Profile**: Update personal information
- **Documents**: Upload a document
- **Job Search**: Search for jobs
- **Applications**: Apply for a job
- **Notifications**: View notifications
- **Benefits**: View benefits

---

## Common Issues & Solutions

### Issue 1: Connection String Error
```
System.Data.SqlClient.SqlException: A network-related or instance-specific 
error occurred while establishing a connection to SQL Server
```

**Solution:**
```json
// Try different connection strings:

// Local with Trusted Connection
"DefaultConnection": "Server=.;Database=WorkForceGovDb;Trusted_Connection=true;TrustServerCertificate=true;"

// Local with User ID
"DefaultConnection": "Server=.;Database=WorkForceGovDb;User Id=sa;Password=YourPassword;Encrypt=true;TrustServerCertificate=true;"

// LocalDB
"DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=WorkForceGovDb;Integrated Security=true;"

// SQL Express
"DefaultConnection": "Server=.\\SQLEXPRESS;Database=WorkForceGovDb;Integrated Security=true;TrustServerCertificate=true;"
```

### Issue 2: Migration Already Applied
```
System.InvalidOperationException: The migration 'AddServiceLayer' has already been applied to the database
```

**Solution:**
```powershell
# Check migration history
Get-Migration

# If you want to remove the migration:
Remove-Migration

# Then add a new one
Add-Migration NameOfNewMigration
```

### Issue 3: Database Already Exists
```
Applying migration...
Error: The database already exists
```

**Solution:**
Option A: Let EF Core update it (usually works)
```powershell
Update-Database
```

Option B: Drop and recreate
```powershell
Drop-Database
Update-Database
```

Option C: Manual reset
1. Delete the database in SQL Server Management Studio
2. Run `Update-Database`

### Issue 4: .NET Runtime Error
```
System.Runtime.InteropServices.RuntimeInformationException: 
Unable to find SQL server on your machine
```

**Solution:**
1. Install SQL Server Express: https://www.microsoft.com/en-us/sql-server/sql-server-editions-express
2. Or use LocalDB: `dotnet tool install -g dotnet-ef`

### Issue 5: Missing Microsoft.EntityFrameworkCore
```
error NU1605: Found invalid package dependency on 'Microsoft.EntityFrameworkCore'
```

**Solution:**
```powershell
# In Package Manager Console
Update-Package Microsoft.EntityFrameworkCore
```

---

## Verification Checklist

- [ ] appsettings.json has valid connection string
- [ ] SQL Server is running
- [ ] Visual Studio shows "Ready" status
- [ ] Add-Migration executed successfully
- [ ] Update-Database executed successfully
- [ ] Database created in SQL Server
- [ ] All 8 tables created
- [ ] No compilation errors
- [ ] Application starts without errors
- [ ] Can navigate to /Account/Register
- [ ] Can register new user
- [ ] Can login with new user
- [ ] Dashboard displays without errors

---

## Useful Commands Reference

```powershell
# List all migrations
Get-Migration

# Get migration info
Get-Migration -All | Format-List

# Add new migration with description
Add-Migration AddNewFeature

# Update database to specific migration
Update-Database -Migration 20240115000000_AddServiceLayer

# Remove last migration
Remove-Migration

# Script migration to SQL file
Script-Migration -From 20240115000000_AddServiceLayer -To 20240116000000_AddNewFeature -Output migration.sql

# Revert to previous migration
Update-Database -Migration PreviousMigrationName

# Check database state
Update-Database -Script

# View migration status
Update-Database -Verbose
```

---

## Post-Migration Setup

### 1. Seed Initial Data (Optional)

Create a seeding method in Program.cs:

```csharp
// After app.Build()
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    
    // Add seed data
    if (!db.JobOpenings.Any())
    {
        db.JobOpenings.AddRange(
            new JobOpening 
            { 
                JobTitle = "Software Developer",
                Description = "Develop software applications",
                Location = "New York",
                JobCategory = "IT",
                Status = "Open",
                PostedDate = DateTime.Now,
                TotalApplications = 0
            }
        );
        db.SaveChanges();
    }
}
```

### 2. Create Admin User (Optional)

```csharp
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    
    if (!db.Users.Any(u => u.Email == "admin@workforce.com"))
    {
        db.Users.Add(new User
        {
            FullName = "Admin",
            Email = "admin@workforce.com",
            Password = "Admin123!", // TODO: Hash this!
            Role = "Admin"
        });
        db.SaveChanges();
    }
}
```

### 3. Create Sample Programs

```csharp
if (!db.EmploymentPrograms.Any())
{
    db.EmploymentPrograms.AddRange(
        new EmploymentProgram
        {
            ProgramName = "Skill Development",
            Description = "Training for skill enhancement",
            Status = "Active",
            StartDate = DateTime.Now
        }
    );
    db.SaveChanges();
}
```

---

## Performance Tips

### 1. Create Indexes on Frequently Queried Columns

In migration file:

```csharp
protected override void Up(MigrationBuilder migrationBuilder)
{
    migrationBuilder.CreateIndex(
        name: "IX_Citizens_UserId",
        table: "Citizens",
        column: "UserId",
        unique: true);
}
```

### 2. Monitor Query Performance

Enable EF Core logging:

```csharp
builder.Services.AddLogging(config => 
    config.AddDebug()
        .SetMinimumLevel(LogLevel.Debug));
```

### 3. Backup Database Regularly

```bash
# SQL Server backup
Backup Database WorkForceGovDb 
To Disk = 'C:\\Backups\\WorkForceGovDb.bak'
```

---

## Rollback Instructions

### Rollback to Previous Migration

```powershell
# See migration history
Get-Migration

# Rollback to specific migration
Update-Database -Migration PreviousMigrationName

# Rollback to initial state (removes all migrations)
Update-Database -Migration 0
```

### Remove Migration Files

```powershell
# Remove last added migration
Remove-Migration

# Force remove (if already applied)
Remove-Migration -Force
```

---

## Backup & Restore

### Backup Database

```bash
# Using SQL Server Management Studio
# Right-click Database → Tasks → Back Up...

# Or using T-SQL
BACKUP DATABASE WorkForceGovDb 
TO DISK = 'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\Backup\WorkForceGovDb.bak'
WITH INIT, CHECKSUM;
```

### Restore Database

```bash
# Using SQL Server Management Studio
# Right-click Databases → Restore Database...

# Or using T-SQL
RESTORE DATABASE WorkForceGovDb 
FROM DISK = 'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\Backup\WorkForceGovDb.bak'
WITH REPLACE;
```

---

## Next Steps

Once database is set up:

1. ✅ Run the application
2. ✅ Register test users
3. ✅ Create test jobs
4. ✅ Test all features
5. ✅ Run integration tests
6. ✅ Deploy to production

---

**Database Setup Complete!** 🎉

Your WorkForceGov application is ready to use. All tables are created and the application is ready for testing.
