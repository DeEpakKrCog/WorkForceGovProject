-- =====================================================
-- WorkForce Government Project - Updated Sample Data
-- Includes Type field for Benefits
-- =====================================================

USE [WorkForceGovDB];
GO

-- Clear existing data (in correct order due to foreign keys)
DELETE FROM Benefits;
DELETE FROM Resources;
DELETE FROM Trainings;
DELETE FROM EmploymentPrograms;
DELETE FROM Citizens;
GO

-- =====================================================
-- 1. INSERT CITIZENS (10 sample citizens)
-- =====================================================
SET IDENTITY_INSERT Citizens ON;

INSERT INTO Citizens (CitizenID, Name, Email, Phone, Address, City, State, ZipCode, DateOfBirth, RegistrationDate)
VALUES 
    (1, 'John Smith', 'john.smith@email.com', '555-0101', '123 Main St', 'Springfield', 'IL', '62701', '1985-03-15', GETDATE()),
    (2, 'Maria Garcia', 'maria.garcia@email.com', '555-0102', '456 Oak Ave', 'Chicago', 'IL', '60601', '1990-07-22', GETDATE()),
    (3, 'David Johnson', 'david.j@email.com', '555-0103', '789 Elm St', 'Peoria', 'IL', '61602', '1988-11-30', GETDATE()),
    (4, 'Sarah Williams', 'sarah.w@email.com', '555-0104', '321 Pine Rd', 'Rockford', 'IL', '61101', '1992-05-18', GETDATE()),
    (5, 'Michael Brown', 'michael.b@email.com', '555-0105', '654 Maple Dr', 'Aurora', 'IL', '60505', '1987-09-25', GETDATE()),
    (6, 'Lisa Anderson', 'lisa.a@email.com', '555-0106', '987 Cedar Ln', 'Naperville', 'IL', '60540', '1991-02-14', GETDATE()),
    (7, 'James Martinez', 'james.m@email.com', '555-0107', '147 Birch Ct', 'Joliet', 'IL', '60432', '1989-12-08', GETDATE()),
    (8, 'Jennifer Taylor', 'jennifer.t@email.com', '555-0108', '258 Spruce Way', 'Elgin', 'IL', '60120', '1993-04-20', GETDATE()),
    (9, 'Robert Wilson', 'robert.w@email.com', '555-0109', '369 Willow Pl', 'Champaign', 'IL', '61820', '1986-08-12', GETDATE()),
    (10, 'Patricia Moore', 'patricia.m@email.com', '555-0110', '741 Ash Blvd', 'Bloomington', 'IL', '61701', '1994-01-05', GETDATE());

SET IDENTITY_INSERT Citizens OFF;
GO

-- =====================================================
-- 2. INSERT EMPLOYMENT PROGRAMS (5 programs)
-- =====================================================
SET IDENTITY_INSERT EmploymentPrograms ON;

INSERT INTO EmploymentPrograms (ProgramID, Title, Description, StartDate, EndDate, Budget, Status)
VALUES 
    (1, 'Tech Skills Initiative', 'Technology training and certification program for unemployed workers', '2024-01-01', '2024-12-31', 500000, 'Active'),
    (2, 'Healthcare Career Path', 'Medical assistant and healthcare training program', '2024-02-01', '2024-11-30', 350000, 'Active'),
    (3, 'Manufacturing Excellence', 'Advanced manufacturing and automation training', '2024-03-01', '2025-02-28', 200000, 'Active'),
    (4, 'Green Jobs Program', 'Renewable energy and environmental careers training', '2023-06-01', '2024-05-31', 100000, 'Completed'),
    (5, 'Small Business Support', 'Entrepreneurship and small business development', '2024-04-01', '2024-10-31', 50000, 'Active');

SET IDENTITY_INSERT EmploymentPrograms OFF;
GO

-- =====================================================
-- 3. INSERT TRAININGS (7 trainings)
-- =====================================================
SET IDENTITY_INSERT Trainings ON;

INSERT INTO Trainings (TrainingID, Title, Description, StartDate, EndDate, Location, Capacity, EnrolledCount, Instructor, ProgramID, Status)
VALUES 
    (1, 'Python Programming Basics', 'Introduction to Python programming for beginners', '2024-01-15', '2024-03-15', 'Springfield Community Center', 30, 28, 'Dr. Sarah Chen', 1, 'Active'),
    (2, 'Web Development Bootcamp', 'Full-stack web development intensive course', '2024-02-01', '2024-05-01', 'Chicago Tech Hub', 25, 22, 'Mark Johnson', 1, 'Active'),
    (3, 'Medical Assistant Certification', 'CMA certification preparation course', '2024-02-15', '2024-06-15', 'Healthcare Training Institute', 20, 18, 'Dr. Amanda Rivera', 2, 'Active'),
    (4, 'CNC Machine Operation', 'Computer numerical control machine training', '2024-03-15', '2024-08-15', 'Manufacturing Training Center', 15, 12, 'Robert Smith', 3, 'Active'),
    (5, 'Solar Panel Installation', 'Solar energy systems installation and maintenance', '2023-07-01', '2024-01-01', 'Green Energy Academy', 18, 15, 'Michael Green', 4, 'Completed'),
    (6, 'Business Plan Development', 'Creating effective business plans and financial projections', '2024-04-15', '2024-07-15', 'Small Business Center', 20, 16, 'Jennifer Adams', 5, 'Active'),
    (7, 'Digital Marketing Essentials', 'Social media and online marketing strategies', '2024-05-01', '2024-07-31', 'Online Platform', 40, 35, 'David Martinez', 5, 'Active');

SET IDENTITY_INSERT Trainings OFF;
GO

-- =====================================================
-- 4. INSERT RESOURCES (9 resources)
-- =====================================================
SET IDENTITY_INSERT Resources ON;

INSERT INTO Resources (ResourceID, Type, Description, Quantity, AllocatedDate, ProgramID)
VALUES 
    (1, 'Equipment', 'Laptop computers for training participants', 30, '2024-01-01', 1),
    (2, 'Materials', 'Python programming textbooks and materials', 50, '2024-01-01', 1),
    (3, 'Software', 'Development tools and IDE licenses', 25, '2024-02-01', 1),
    (4, 'Equipment', 'Medical training equipment and supplies', 20, '2024-02-01', 2),
    (5, 'Materials', 'Healthcare certification study guides', 25, '2024-02-15', 2),
    (6, 'Equipment', 'CNC training machines', 5, '2024-03-01', 3),
    (7, 'Materials', 'Manufacturing safety equipment', 30, '2024-03-15', 3),
    (8, 'Materials', 'Business planning software subscriptions', 20, '2024-04-01', 5),
    (9, 'Equipment', 'Presentation equipment for workshops', 10, '2024-04-15', 5);

SET IDENTITY_INSERT Resources OFF;
GO

-- =====================================================
-- 5. INSERT BENEFITS (15 benefits with Type field)
-- =====================================================
SET IDENTITY_INSERT Benefits ON;

INSERT INTO Benefits (BenefitID, Amount, Type, Date, ProgramID, CitizenID)
VALUES 
    -- Tech Skills Initiative Benefits
    (1, 2500.00, 'Training Stipend', '2024-01-20', 1, 1),
    (2, 1500.00, 'Equipment Grant', '2024-02-10', 1, 1),
    (3, 2000.00, 'Training Stipend', '2024-02-15', 1, 2),
    (4, 3000.00, 'Certification Bonus', '2024-03-01', 1, 3),
    
    -- Healthcare Career Path Benefits
    (5, 1800.00, 'Training Stipend', '2024-02-20', 2, 4),
    (6, 2200.00, 'Certification Fee', '2024-03-15', 2, 5),
    (7, 1500.00, 'Training Stipend', '2024-03-20', 2, 6),
    
    -- Manufacturing Excellence Benefits
    (8, 2000.00, 'Training Stipend', '2024-03-25', 3, 7),
    (9, 1700.00, 'Safety Equipment', '2024-04-10', 3, 8),
    
    -- Green Jobs Program Benefits
    (10, 1600.00, 'Training Stipend', '2023-08-15', 4, 9),
    (11, 2500.00, 'Certification Fee', '2023-12-20', 4, 10),
    
    -- Small Business Support Benefits
    (12, 1200.00, 'Business Grant', '2024-05-01', 5, 2),
    (13, 1500.00, 'Marketing Support', '2024-05-15', 5, 4),
    (14, 2000.00, 'Startup Capital', '2024-06-01', 5, 6),
    (15, 1800.00, 'Business Grant', '2024-06-10', 5, 8);

SET IDENTITY_INSERT Benefits OFF;
GO

-- =====================================================
-- VERIFICATION QUERIES
-- =====================================================

PRINT '==========================================';
PRINT 'DATA INSERTION VERIFICATION';
PRINT '==========================================';
PRINT '';

-- Count records
SELECT 'Citizens' AS TableName, COUNT(*) AS RecordCount FROM Citizens
UNION ALL
SELECT 'EmploymentPrograms', COUNT(*) FROM EmploymentPrograms
UNION ALL
SELECT 'Trainings', COUNT(*) FROM Trainings
UNION ALL
SELECT 'Resources', COUNT(*) FROM Resources
UNION ALL
SELECT 'Benefits', COUNT(*) FROM Benefits;

PRINT '';
PRINT '==========================================';
PRINT 'PROGRAM SUMMARY WITH BENEFITS';
PRINT '==========================================';

SELECT 
    p.ProgramID,
    p.Title AS ProgramTitle,
    p.Budget AS TotalBudget,
    COUNT(DISTINCT t.TrainingID) AS TotalTrainings,
    COUNT(DISTINCT b.BenefitID) AS TotalBenefits,
    ISNULL(SUM(b.Amount), 0) AS TotalSpent,
    p.Budget - ISNULL(SUM(b.Amount), 0) AS RemainingBudget,
    CASE 
        WHEN p.Budget > 0 THEN CAST(ISNULL(SUM(b.Amount), 0) / p.Budget * 100 AS DECIMAL(5,2))
        ELSE 0 
    END AS UtilizationPercent
FROM EmploymentPrograms p
LEFT JOIN Trainings t ON p.ProgramID = t.ProgramID
LEFT JOIN Benefits b ON p.ProgramID = b.ProgramID
GROUP BY p.ProgramID, p.Title, p.Budget
ORDER BY p.ProgramID;

PRINT '';
PRINT '==========================================';
PRINT 'BENEFITS BY TYPE';
PRINT '==========================================';

SELECT 
    Type AS BenefitType,
    COUNT(*) AS Count,
    SUM(Amount) AS TotalAmount,
    AVG(Amount) AS AverageAmount
FROM Benefits
GROUP BY Type
ORDER BY TotalAmount DESC;

PRINT '';
PRINT '==========================================';
PRINT 'BENEFICIARY SUMMARY';
PRINT '==========================================';

SELECT 
    COUNT(DISTINCT CitizenID) AS TotalBeneficiaries,
    COUNT(*) AS TotalBenefitsDistributed,
    SUM(Amount) AS TotalAmountPaid,
    AVG(Amount) AS AveragePerBenefit
FROM Benefits;

PRINT '';
PRINT 'Sample data insertion completed successfully!';
