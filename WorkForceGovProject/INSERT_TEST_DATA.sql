-- ============================================
-- WorkForce Governance System - Test Data
-- ============================================
-- Run this script after database migration to populate test data

-- Test Users
INSERT INTO Users (FullName, Email, Password, Role, Status, CreatedAt)
VALUES 
    ('John Citizen', 'john.citizen@gov.local', 'password123', 'Citizen', 'Active', GETDATE()),
    ('Jane Employer', 'jane.employer@company.com', 'password123', 'Employer', 'Active', GETDATE()),
    ('Officer Smith', 'officer.smith@gov.local', 'password123', 'LaborOfficer', 'Active', GETDATE()),
    ('Admin User', 'admin@gov.local', 'password123', 'SystemAdmin', 'Active', GETDATE()),
    ('Manager Davis', 'davis.manager@gov.local', 'password123', 'ProgramManager', 'Active', GETDATE()),
    ('Officer Compliance', 'compliance@gov.local', 'password123', 'ComplianceOfficer', 'Active', GETDATE()),
    ('Auditor Brown', 'auditor@gov.local', 'password123', 'GovernmentAuditor', 'Active', GETDATE());

-- Test Citizen Profiles
INSERT INTO Citizens (UserId, FullName, Email, DOB, Gender, Address, PhoneNumber, ActiveApplications, AccountBalance, DocumentStatus, NewJobMatches)
VALUES
    (1, 'John Citizen', 'john.citizen@gov.local', '1990-05-15', 'Male', '123 Main Street, City, State 12345', '+1-555-0101', 2, 5000.00, 'Approved', 5),
    (2, 'Jane Employer', 'jane.employer@company.com', '1985-08-20', 'Female', '456 Business Ave, City, State 12345', '+1-555-0102', 0, 0.00, 'N/A', 0);

-- Test Benefits
INSERT INTO Benefits (BenefitName, BenefitDescription, BenefitType, Amount, StartDate, EndDate, Status)
VALUES
    ('Unemployment Assistance', 'Weekly unemployment benefits', 'Cash', 350.00, GETDATE(), DATEADD(MONTH, 3, GETDATE()), 'Active'),
    ('Job Training Program', 'Free vocational training', 'Training', 0.00, GETDATE(), DATEADD(MONTH, 6, GETDATE()), 'Active'),
    ('Health Insurance Subsidy', 'Health insurance premium assistance', 'Healthcare', 200.00, GETDATE(), DATEADD(MONTH, 12, GETDATE()), 'Active');

-- Test Employment Programs
INSERT INTO EmploymentPrograms (ProgramName, Description, StartDate, EndDate, Status, BudgetAllocated, BudgetSpent, TargetParticipants, CurrentParticipants)
VALUES
    ('Skills Development Initiative', 'Comprehensive skills training for workforce', GETDATE(), DATEADD(YEAR, 1, GETDATE()), 'Active', 500000.00, 120000.00, 500, 287),
    ('Youth Employment Program', 'Job placement for ages 18-25', GETDATE(), DATEADD(MONTH, 12, GETDATE()), 'Active', 300000.00, 85000.00, 300, 156),
    ('Senior Worker Support', 'Re-employment for workers 55+', GETDATE(), DATEADD(YEAR, 1, GETDATE()), 'Active', 250000.00, 45000.00, 200, 98);

-- Test Job Openings
INSERT INTO JobOpenings (EmployerId, JobTitle, JobDescription, Location, Category, SalaryMin, SalaryMax, PostedDate, ExpirationDate, Status, OpenPositions)
VALUES
    (2, 'Software Developer', 'Seeking experienced developers', 'New York, NY', 'Technology', 80000.00, 120000.00, GETDATE(), DATEADD(DAY, 30, GETDATE()), 'Open', 3),
    (2, 'Project Manager', 'Looking for project management professionals', 'Chicago, IL', 'Management', 70000.00, 100000.00, GETDATE(), DATEADD(DAY, 30, GETDATE()), 'Open', 2),
    (2, 'Data Analyst', 'Data analysis and reporting role', 'Remote', 'Analytics', 60000.00, 90000.00, GETDATE(), DATEADD(DAY, 30, GETDATE()), 'Open', 1);

-- Test Compliance Records
INSERT INTO ComplianceRecords (EntityID, Type, Result, Notes, Date)
VALUES
    (2, 'Employer', 'Compliant', 'Quarterly compliance check passed', GETDATE()),
    (2, 'Employer', 'Under Review', 'Wage documentation review pending', GETDATE()),
    (1, 'Application', 'Compliant', 'Application meets all requirements', GETDATE());

-- Test Audits
INSERT INTO Audits (OfficerID, Scope, Findings, Date, Status)
VALUES
    (6, 'Employer', 'Review of hiring practices and wage compliance', GETDATE(), 'Open'),
    (6, 'Program', 'Evaluation of program effectiveness and budget utilization', DATEADD(DAY, -5, GETDATE()), 'Completed'),
    (6, 'Application', 'Audit of submitted applications for compliance', DATEADD(DAY, -10, GETDATE()), 'Pending');

-- Test Reports
INSERT INTO Reports (ReportName, GeneratedBy, GeneratedDate, ReportType, ReportContent, StartDate, EndDate)
VALUES
    ('Monthly Compliance Report', 4, GETDATE(), 'Compliance', 'Monthly summary of compliance activities', DATEADD(MONTH, -1, GETDATE()), GETDATE()),
    ('Program Performance Report', 5, GETDATE(), 'Program', 'Analysis of employment program outcomes', DATEADD(MONTH, -1, GETDATE()), GETDATE()),
    ('Quarterly Audit Summary', 7, GETDATE(), 'Audit', 'Summary of quarterly audits and findings', DATEADD(MONTH, -3, GETDATE()), GETDATE());

-- Test System Logs
INSERT INTO SystemLogs (UserId, Action, EntityType, EntityId, Details, LogDate, IpAddress)
VALUES
    (1, 'LOGIN', 'User', 1, 'User logged in successfully', GETDATE(), '192.168.1.100'),
    (2, 'CREATE', 'JobOpening', 1, 'New job opening created', GETDATE(), '192.168.1.101'),
    (6, 'UPDATE', 'ComplianceRecord', 1, 'Compliance record updated', GETDATE(), '192.168.1.102');

-- Test Notifications
INSERT INTO Notifications (UserId, Message, Category, IsRead, CreatedDate)
VALUES
    (1, 'Your job application to ABC Company has been reviewed', 'Application', 0, GETDATE()),
    (1, 'New job matches available in your field', 'JobMatch', 0, GETDATE()),
    (1, 'Unemployment benefits payment processed', 'Benefits', 0, GETDATE()),
    (2, 'New candidate applications received', 'Application', 0, GETDATE());

-- Verify insertion
SELECT 'Users' AS TableName, COUNT(*) AS RecordCount FROM Users
UNION ALL
SELECT 'Citizens', COUNT(*) FROM Citizens
UNION ALL
SELECT 'Benefits', COUNT(*) FROM Benefits
UNION ALL
SELECT 'EmploymentPrograms', COUNT(*) FROM EmploymentPrograms
UNION ALL
SELECT 'JobOpenings', COUNT(*) FROM JobOpenings
UNION ALL
SELECT 'ComplianceRecords', COUNT(*) FROM ComplianceRecords
UNION ALL
SELECT 'Audits', COUNT(*) FROM Audits
UNION ALL
SELECT 'Reports', COUNT(*) FROM Reports;

-- ============================================
-- Test Login Credentials:
-- ============================================
-- Citizen: 
--   Email: john.citizen@gov.local
--   Password: password123
--
-- Employer:
--   Email: jane.employer@company.com
--   Password: password123
--
-- Labor Officer:
--   Email: officer.smith@gov.local
--   Password: password123
--
-- Compliance Officer:
--   Email: compliance@gov.local
--   Password: password123
--
-- Government Auditor:
--   Email: auditor@gov.local
--   Password: password123
-- ============================================
