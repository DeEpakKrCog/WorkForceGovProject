# 📊 Citizen Module - Architecture & Workflow Diagram

## System Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                    WORKFORCEGOV CITIZEN MODULE               │
├─────────────────────────────────────────────────────────────┤
│                                                              │
│  ┌──────────────┐      ┌─────────────┐     ┌─────────────┐ │
│  │   VIEWS      │      │ CONTROLLERS │     │   MODELS    │ │
│  ├──────────────┤      ├─────────────┤     ├─────────────┤ │
│  │ Dashboard    │◄─────┤             │    ►│ Citizen     │ │
│  │ Profile      │      │             │     │ Document    │ │
│  │ Documents    │      │ Citizen     │     │ JobOpening  │ │
│  │ JobSearch    │      │ Controller  │     │ Application │ │
│  │ Applications │      │   (13       │     │ Benefit     │ │
│  │ Benefits     │      │   Actions)  │     │ Program     │ │
│  │ Notifications│      │             │     │ Notification│ │
│  └──────────────┘      └─────────────┘     └─────────────┘ │
│       ▲                       ▲                    ▲         │
│       └───────────────────────┴────────────────────┘         │
│                       HTTP Requests                          │
│                                                              │
└─────────────────────────────────────────────────────────────┘
                            ▼
         ┌──────────────────────────────────┐
         │    ApplicationDbContext          │
         │  ├─ DbSet<Citizen>              │
         │  ├─ DbSet<CitizenDocument>      │
         │  ├─ DbSet<JobOpening>           │
         │  ├─ DbSet<Application>          │
         │  ├─ DbSet<Benefit>              │
         │  ├─ DbSet<EmploymentProgram>    │
         │  └─ DbSet<Notification>         │
         └──────────────────────────────────┘
                            ▼
         ┌──────────────────────────────────┐
         │      SQL Server Database         │
         │  ├─ Users Table                 │
         │  ├─ Citizens Table              │
         │  ├─ CitizenDocuments Table      │
         │  ├─ JobOpenings Table           │
         │  ├─ Applications Table          │
         │  ├─ Benefits Table              │
         │  ├─ EmploymentPrograms Table    │
         │  └─ Notifications Table         │
         └──────────────────────────────────┘
```

---

## User Journey Flow

```
┌─────────────────────────────────────────────────────────────┐
│                   CITIZEN USER JOURNEY                       │
└─────────────────────────────────────────────────────────────┘

START
  │
  ├─► REGISTER
  │   ├─ Fill form
  │   ├─ Select "Citizen" role
  │   └─ Create Account
  │
  ├─► LOGIN
  │   ├─ Enter credentials
  │   ├─ Session created
  │   └─ Redirect to Dashboard
  │
  ├─► DASHBOARD
  │   ├─ View stats (0 initially)
  │   ├─ View quick links
  │   └─ Navigate to features
  │
  ├─► PROFILE MANAGEMENT
  │   ├─ Click "My Profile"
  │   ├─ Update information
  │   └─ Save changes ✅
  │
  ├─► DOCUMENT UPLOAD
  │   ├─ Click "Verification Docs"
  │   ├─ Click "Upload New Document"
  │   ├─ Select document type
  │   ├─ Choose file
  │   └─ Upload ✅
  │
  ├─► JOB SEARCH
  │   ├─ Click "Job Board"
  │   ├─ View available jobs
  │   ├─ Filter (optional)
  │   └─ Select job
  │
  ├─► JOB APPLICATION
  │   ├─ Click "Apply Now"
  │   ├─ Enter cover letter (optional)
  │   ├─ Submit application ✅
  │   ├─ Application created
  │   ├─ Notification sent
  │   └─ Update dashboard stats
  │
  ├─► TRACK APPLICATIONS
  │   ├─ Click "My Applications"
  │   ├─ View application history
  │   ├─ Check status
  │   └─ Withdraw if pending
  │
  ├─► VIEW BENEFITS
  │   ├─ Click "Benefits"
  │   ├─ View enrolled programs
  │   └─ Track amounts
  │
  ├─► CHECK NOTIFICATIONS
  │   ├─ Click "Notifications"
  │   ├─ View all messages
  │   └─ Mark as read
  │
  └─► LOGOUT
      └─ End session

END
```

---

## Database Relationship Diagram

```
                          USER
                           │
                ┌──────────┴──────────┐
                │                     │
            Citizen          Notification
           /  |  | \          (UserId FK)
          /   |  |  \
         /    |  |   \
    Doc1  Doc2 │  │  Benefit
             App1│
             App2│
                │
           JobOpening
```

**Detailed Relationships:**

```
┌─────────┐
│  User   │ (1)
└────┬────┘
     │
     │ 1:N
     │
     ├──►┌──────────┐
     │   │ Citizen  │ (N)
     │   └────┬─────┘
     │        │
     │        │ 1:N
     │        ├──►┌──────────────────┐
     │        │   │ CitizenDocument  │
     │        │   └──────────────────┘
     │        │
     │        │ 1:N
     │        ├──►┌──────────────────┐
     │        │   │  Application     │◄────┐
     │        │   └──────────────────┘     │
     │        │                            │ 1:N
     │        │                       ┌────┴──────┐
     │        │                       │ JobOpening│
     │        │                       └───────────┘
     │        │
     │        │ 1:N
     │        └──►┌──────────────────┐
     │            │   Benefit        │◄────┐
     │            └──────────────────┘     │
     │                                     │ 1:N
     │                                ┌────┴──────────┐
     │                                │EmploymentProgr│
     │                                │am             │
     │                                └────────────────┘
     │
     │ 1:N
     └──►┌──────────────────┐
         │  Notification    │
         └──────────────────┘
```

---

## Data Flow Diagram

```
┌──────────────────────────────────────────────────────────────┐
│ JOB APPLICATION DATA FLOW                                    │
└──────────────────────────────────────────────────────────────┘

Citizen Clicks "Apply For Job"
              │
              ▼
    ┌──────────────────────┐
    │ JobApplicationForm   │
    │ ├─ jobId            │
    │ └─ coverLetter      │
    └──────────────────────┘
              │
              ▼
    ┌──────────────────────────────────┐
    │ CitizenController                │
    │ .ApplyForJob(jobId, coverLetter) │
    └──────────────────────────────────┘
              │
              ▼
    ┌──────────────────────────────────────┐
    │ Validation & Business Logic          │
    │ ├─ Check if already applied         │
    │ ├─ Get citizen from session         │
    │ └─ Get job details                  │
    └──────────────────────────────────────┘
              │
              ▼
    ┌──────────────────────────────────────┐
    │ Create Application Entity            │
    │ ├─ CitizenId: {loggedInCitizen}     │
    │ ├─ JobOpeningId: {jobId}            │
    │ ├─ Status: "Pending"                │
    │ ├─ SubmittedDate: now()             │
    │ └─ CoverLetter: {text}              │
    └──────────────────────────────────────┘
              │
              ├─► Save to Database
              │       │
              │       └──► Applications Table
              │
              ├─► Create Notification
              │       │
              │       └──► Notifications Table
              │
              └─► Update Job Stats
                      │
                      └──► Increment TotalApplications

              ▼
    Display Success Message & Redirect
```

---

## Authentication & Session Flow

```
┌──────────────────────────────────────────────────────────────┐
│ AUTHENTICATION FLOW                                          │
└──────────────────────────────────────────────────────────────┘

REGISTER                    LOGIN                  ACCESS
    │                         │                       │
    ▼                         ▼                       ▼
User fills              User enters          Request to
form with               credentials          /Citizen/Dashboard
email & password            │                       │
    │                       ▼                       ▼
    ▼                   Validate in              Check Session
Save User &             database                 │
Citizen                 │                        ├─ UserId
    │                   ▼                        │
    ▼              Create Session            SessionData
Show Login         with UserId                  │
Message            │                           ▼
                   ▼                      ┌──────────────┐
                Redirect to           ┌──┤ Authenticated│
                Dashboard             │  └──────────────┘
                                      │
                                      ├─ UserId exists
                                      ├─ Session active
                                      └─ Proceed

Logout:
  ├─ Clear Session
  ├─ Delete SessionID
  └─ Redirect to Login
```

---

## Page Navigation Map

```
                    ┌─────────────┐
                    │   Login     │
                    └──────┬──────┘
                           │
                           ▼
                    ┌─────────────────┐
                    │   Dashboard     │ ◄────┐
                    └──────┬──────────┘      │
                           │                │
            ┌──────────────┼──────────────┐  │
            │              │              │  │
            ▼              ▼              ▼  │
        ┌──────────┐ ┌─────────┐ ┌──────────┤
        │ Profile  │ │Documents│ │JobSearch │
        └──────────┘ └─────────┘ └──────────┘
                           ▼
                    ┌──────────────┐
                    │Applications  │
                    └──────────────┘
                           ▼
                    ┌──────────────┐
                    │  Benefits    │
                    └──────────────┘
                           ▼
                    ┌──────────────┐
                    │Notifications │
                    └──────┬───────┘
                           │
                           ▼
                    ┌──────────────┐
                    │   Logout     │
                    └──────────────┘
```

---

## API Request/Response Flow

```
┌──────────────────────────────────────────────────────────────┐
│ REQUEST: GET /Citizen/Dashboard                              │
└──────────────────────────────────────────────────────────────┘

Browser          HTTP              Server
  │              Request              │
  ├─────────────────────────────────►│
  │   GET /Citizen/Dashboard         │
  │   Session: {UserId: 1}           │
  │                                  ▼
  │                         CitizenController
  │                         .Dashboard()
  │                                  │
  │                                  ├─ GetLoggedInCitizen()
  │                                  ├─ Calculate stats
  │                                  └─ Render View
  │                                  │
  │◄─────────────────────────────────┤
  │   HTTP 200 OK                    │
  │   Content-Type: text/html        │
  │   [Dashboard.cshtml rendered]    │
  │
  Display Dashboard
```

---

## Error Handling Flow

```
┌──────────────────────────────────────────────────────────────┐
│ ERROR HANDLING                                               │
└──────────────────────────────────────────────────────────────┘

User Action
    │
    ▼
Try Action
    │
    ├─ Success? ──────────────────► Show Success Message
    │                                    │
    │                                    ▼
    └─ Error? ────────────────► TempData["ErrorMessage"]
                                    │
                                    ▼
                          Redirect to same page
                                    │
                                    ▼
                          Display error in alert
```

---

## Security Architecture

```
┌──────────────────────────────────────────────────────────────┐
│ SECURITY LAYERS                                              │
└──────────────────────────────────────────────────────────────┘

┌─ Layer 1: Authentication ──────────────────┐
│ ├─ Session-based login                    │
│ ├─ Credential validation                  │
│ └─ Session timeout (30 minutes)           │
└──────────────────────────────────────────┘
              │
              ▼
┌─ Layer 2: Authorization ──────────────────┐
│ ├─ Role-based access (Citizen)            │
│ ├─ User isolation                         │
│ └─ Endpoint protection                    │
└──────────────────────────────────────────┘
              │
              ▼
┌─ Layer 3: Data Protection ────────────────┐
│ ├─ Anti-forgery tokens                    │
│ ├─ Input validation                       │
│ ├─ File upload validation                 │
│ └─ SQL injection prevention (EF Core)     │
└──────────────────────────────────────────┘
              │
              ▼
┌─ Layer 4: Communication ──────────────────┐
│ ├─ HTTPS (production)                     │
│ ├─ Secure cookies                         │
│ └─ HttpOnly flag on auth cookies          │
└──────────────────────────────────────────┘
```

---

## Deployment Architecture

```
┌──────────────────────────────────────────────────────────────┐
│ PRODUCTION DEPLOYMENT                                        │
└──────────────────────────────────────────────────────────────┘

┌─────────────────────────┐
│   Browser/Client        │
│                         │
│  Request ───────────►  │
│              ◄──────── Response
└─────────────────────────┘
         │
         │ HTTPS
         ▼
┌─────────────────────────┐
│   Web Server (IIS)      │
│                         │
│  ┌─────────────────┐   │
│  │ ASP.NET Core    │   │
│  │ Application     │   │
│  └────────┬────────┘   │
└───────────┼─────────────┘
            │
            │
            ▼
┌─────────────────────────┐
│   SQL Server            │
│                         │
│  ├─ Users              │
│  ├─ Citizens           │
│  ├─ Applications       │
│  ├─ Jobs              │
│  ├─ Benefits          │
│  ├─ Programs          │
│  ├─ Documents         │
│  └─ Notifications     │
└─────────────────────────┘
            │
            ▼
┌─────────────────────────┐
│   File Storage          │
│                         │
│  /wwwroot/documents/    │
│   ├─ resume.pdf        │
│   ├─ cert.pdf          │
│   └─ id_proof.jpg      │
└─────────────────────────┘
```

---

**Architecture Documentation Complete** ✅

All diagrams show the complete system architecture, data flow, user journey, and security implementation of the Citizen Module.
