Yes. Since you're building a **large-scale Student Management System with .NET 10 + EF Core + SQL Server**, I would not put all constraints only through `[DataAnnotation]`. For this schema, use:

* **Models/entities** → relationships and basic property definitions
* **Fluent API in `OnModelCreating`** → indexes, composite unique constraints, delete behavior, precision, check constraints
* **DTOs** → API input/output, rather than exposing entities directly

I'm also applying the improvements we discussed:

* `AcademicYear` as a table
* `SemesterSubject` as the academic mapping
* `StudentSemester` as the student's semester enrollment
* `ClassSession` for individual classes
* `AttendanceRecord` linked to `ClassSession`
* `FacultySubject` linked to `SemesterSubject`
* `Material` linked to `SemesterSubject`
* No redundant `DepartmentId` in `Student`
* `PasswordHash` instead of `Password`

Below is a solid production-oriented entity model.

---

# 1. Project structure

I recommend:

```text
StudentManagementSystem
│
├── Domain
│   ├── Entities
│   │   ├── Role.cs
│   │   ├── User.cs
│   │   ├── Department.cs
│   │   ├── Program.cs
│   │   ├── AcademicYear.cs
│   │   ├── Semester.cs
│   │   ├── Subject.cs
│   │   ├── SemesterSubject.cs
│   │   ├── Student.cs
│   │   ├── StudentSemester.cs
│   │   ├── SemesterResult.cs
│   │   ├── SubjectResult.cs
│   │   ├── Faculty.cs
│   │   ├── FacultySubject.cs
│   │   ├── ClassSession.cs
│   │   ├── Attendance.cs
│   │   ├── AttendanceRecord.cs
│   │   ├── Project.cs
│   │   ├── ProjectAllocation.cs
│   │   ├── Task.cs
│   │   └── Material.cs
│   │
│   └── Enums
│       ├── StudentStatus.cs
│       ├── ResultStatus.cs
│       ├── AttendanceStatus.cs
│       ├── ProjectStatus.cs
│       ├── TaskStatus.cs
│       └── MaterialType.cs
│
├── Infrastructure
│   └── Persistence
│       └── ApplicationDbContext.cs
│
└── API
```

---

# 2. Base entity

Since almost every entity has `CreatedAt` and `UpdatedAt`, don't repeat those properties everywhere.

### `BaseEntity.cs`

```csharp
namespace StudentManagementSystem.Domain.Entities;

public abstract class BaseEntity
{
    public int CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
```

However, if you don't yet have an audit-user system, I would actually keep it simpler:

```csharp
namespace StudentManagementSystem.Domain.Entities;

public abstract class BaseEntity
{
    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
```

---

# 3. Role

### `Role.cs`

```csharp
namespace StudentManagementSystem.Domain.Entities;

public class Role : BaseEntity
{
    public int RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    // Navigation
    public ICollection<User> Users { get; set; } = new List<User>();
}
```

Relationship:

```text
Role 1 ─────────< User
```

---

# 4. User

### `User.cs`

```csharp
namespace StudentManagementSystem.Domain.Entities;

public class User : BaseEntity
{
    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string? Address { get; set; }

    public DateTime? Dob { get; set; }

    public string? ProfilePhoto { get; set; }

    public int RoleId { get; set; }

    public bool IsActive { get; set; }

    // Navigation
    public Role Role { get; set; } = null!;

    public Student? Student { get; set; }

    public Faculty? Faculty { get; set; }

    public ICollection<Material> UploadedMaterials { get; set; }
        = new List<Material>();
}
```

Important:

```csharp
public string PasswordHash { get; set; }
```

Never store the actual password.

---

# 5. Department

### `Department.cs`

```csharp
namespace StudentManagementSystem.Domain.Entities;

public class Department : BaseEntity
{
    public int DepartmentId { get; set; }

    public string DepartmentName { get; set; } = null!;

    public string DepartmentCode { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    // Navigation
    public ICollection<Program> Programs { get; set; }
        = new List<Program>();

    public ICollection<Faculty> Faculties { get; set; }
        = new List<Faculty>();
}
```

Notice that I removed `Students`.

Why?

```text
Student → Program → Department
```

Student doesn't need a duplicate `DepartmentId`.

---

# 6. Program

Because `Program` can conflict with the C# concept of `Program` in some contexts, you can call the entity `AcademicProgram`.

I recommend that.

### `AcademicProgram.cs`

```csharp
namespace StudentManagementSystem.Domain.Entities;

public class AcademicProgram : BaseEntity
{
    public int ProgramId { get; set; }

    public string ProgramName { get; set; } = null!;

    public string ProgramCode { get; set; } = null!;

    public int DepartmentId { get; set; }

    public int DurationYears { get; set; }

    public int TotalSemesters { get; set; }

    public bool IsActive { get; set; }

    // Navigation
    public Department Department { get; set; } = null!;

    public ICollection<Student> Students { get; set; }
        = new List<Student>();

    public ICollection<SemesterSubject> SemesterSubjects { get; set; }
        = new List<SemesterSubject>();

    public ICollection<Project> Projects { get; set; }
        = new List<Project>();
}
```

---

# 7. AcademicYear

### `AcademicYear.cs`

```csharp
namespace StudentManagementSystem.Domain.Entities;

public class AcademicYear : BaseEntity
{
    public int AcademicYearId { get; set; }

    public string YearName { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public bool IsActive { get; set; }

    // Navigation
    public ICollection<StudentSemester> StudentSemesters { get; set; }
        = new List<StudentSemester>();

    public ICollection<FacultySubject> FacultySubjects { get; set; }
        = new List<FacultySubject>();
}
```

Example:

```text
2026-27
2027-28
2028-29
```

---

# 8. Semester

### `Semester.cs`

```csharp
namespace StudentManagementSystem.Domain.Entities;

public class Semester : BaseEntity
{
    public int SemesterId { get; set; }

    public int SemesterNumber { get; set; }

    public string SemesterName { get; set; } = null!;

    public bool IsActive { get; set; }

    // Navigation
    public ICollection<SemesterSubject> SemesterSubjects { get; set; }
        = new List<SemesterSubject>();

    public ICollection<StudentSemester> StudentSemesters { get; set; }
        = new List<StudentSemester>();

    public ICollection<Project> Projects { get; set; }
        = new List<Project>();
}
```

---

# 9. Subject

### `Subject.cs`

```csharp
namespace StudentManagementSystem.Domain.Entities;

public class Subject : BaseEntity
{
    public int SubjectId { get; set; }

    public string SubjectCode { get; set; } = null!;

    public string SubjectName { get; set; } = null!;

    public string SubjectType { get; set; } = null!;

    // Navigation
    public ICollection<SemesterSubject> SemesterSubjects { get; set; }
        = new List<SemesterSubject>();
}
```

Later, you can replace `SubjectType` with an enum or lookup table.

---

# 10. SemesterSubject

This is one of the most important entities.

### `SemesterSubject.cs`

```csharp
namespace StudentManagementSystem.Domain.Entities;

public class SemesterSubject : BaseEntity
{
    public int SemesterSubjectId { get; set; }

    public int ProgramId { get; set; }

    public int SemesterId { get; set; }

    public int SubjectId { get; set; }

    public decimal Credits { get; set; }

    // Navigation
    public AcademicProgram Program { get; set; } = null!;

    public Semester Semester { get; set; } = null!;

    public Subject Subject { get; set; } = null!;

    public ICollection<SubjectResult> SubjectResults { get; set; }
        = new List<SubjectResult>();

    public ICollection<FacultySubject> FacultySubjects { get; set; }
        = new List<FacultySubject>();

    public ICollection<Attendance> Attendances { get; set; }
        = new List<Attendance>();

    public ICollection<ClassSession> ClassSessions { get; set; }
        = new List<ClassSession>();

    public ICollection<Material> Materials { get; set; }
        = new List<Material>();
}
```

---

# 11. Student

### `Student.cs`

```csharp
namespace StudentManagementSystem.Domain.Entities;

public class Student : BaseEntity
{
    public int StudentId { get; set; }

    public int UserId { get; set; }

    public string EnrollmentNumber { get; set; } = null!;

    public int AdmissionYear { get; set; }

    public int ProgramId { get; set; }

    public int? CurrentSemesterId { get; set; }

    // Navigation
    public User User { get; set; } = null!;

    public AcademicProgram Program { get; set; } = null!;

    public Semester? CurrentSemester { get; set; }

    public ICollection<StudentSemester> StudentSemesters { get; set; }
        = new List<StudentSemester>();

    public ICollection<Attendance> Attendances { get; set; }
        = new List<Attendance>();

    public ICollection<AttendanceRecord> AttendanceRecords { get; set; }
        = new List<AttendanceRecord>();

    public ICollection<ProjectAllocation> ProjectAllocations { get; set; }
        = new List<ProjectAllocation>();
}
```

---

# 12. StudentSemester

### `StudentSemester.cs`

```csharp
namespace StudentManagementSystem.Domain.Entities;

public class StudentSemester : BaseEntity
{
    public int StudentSemesterId { get; set; }

    public int StudentId { get; set; }

    public int SemesterId { get; set; }

    public int AcademicYearId { get; set; }

    public DateTime EnrollmentDate { get; set; }

    public string Status { get; set; } = null!;

    // Navigation
    public Student Student { get; set; } = null!;

    public Semester Semester { get; set; } = null!;

    public AcademicYear AcademicYear { get; set; } = null!;

    public SemesterResult? SemesterResult { get; set; }

    public ICollection<Attendance> Attendances { get; set; }
        = new List<Attendance>();

    public ICollection<AttendanceRecord> AttendanceRecords { get; set; }
        = new List<AttendanceRecord>();
}
```

---

# 13. SemesterResult

### `SemesterResult.cs`

```csharp
namespace StudentManagementSystem.Domain.Entities;

public class SemesterResult : BaseEntity
{
    public int SemesterResultId { get; set; }

    public int StudentSemesterId { get; set; }

    public decimal SGPA { get; set; }

    public decimal TotalCredits { get; set; }

    public decimal EarnedCredits { get; set; }

    public string ResultStatus { get; set; } = null!;

    // Navigation
    public StudentSemester StudentSemester { get; set; } = null!;

    public ICollection<SubjectResult> SubjectResults { get; set; }
        = new List<SubjectResult>();
}
```

---

# 14. SubjectResult

### `SubjectResult.cs`

```csharp
namespace StudentManagementSystem.Domain.Entities;

public class SubjectResult : BaseEntity
{
    public int SubjectResultId { get; set; }

    public int SemesterResultId { get; set; }

    public int SemesterSubjectId { get; set; }

    public decimal InternalMarks { get; set; }

    public decimal ExternalMarks { get; set; }

    public decimal PracticalMarks { get; set; }

    public decimal TotalMarks { get; set; }

    public string Grade { get; set; } = null!;

    public decimal GradePoint { get; set; }

    public decimal CreditsEarned { get; set; }

    public string ResultStatus { get; set; } = null!;

    // Navigation
    public SemesterResult SemesterResult { get; set; } = null!;

    public SemesterSubject SemesterSubject { get; set; } = null!;
}
```

---

# 15. Faculty

### `Faculty.cs`

```csharp
namespace StudentManagementSystem.Domain.Entities;

public class Faculty : BaseEntity
{
    public int FacultyId { get; set; }

    public int UserId { get; set; }

    public string EmployeeNumber { get; set; } = null!;

    public int DepartmentId { get; set; }

    public string Designation { get; set; } = null!;

    public DateTime JoiningDate { get; set; }

    // Navigation
    public User User { get; set; } = null!;

    public Department Department { get; set; } = null!;

    public ICollection<FacultySubject> FacultySubjects { get; set; }
        = new List<FacultySubject>();

    public ICollection<ClassSession> ClassSessions { get; set; }
        = new List<ClassSession>();

    public ICollection<ProjectAllocation> ProjectAllocations { get; set; }
        = new List<ProjectAllocation>();
}
```

---

# 16. FacultySubject

### `FacultySubject.cs`

```csharp
namespace StudentManagementSystem.Domain.Entities;

public class FacultySubject : BaseEntity
{
    public int FacultySubjectId { get; set; }

    public int FacultyId { get; set; }

    public int SemesterSubjectId { get; set; }

    public int AcademicYearId { get; set; }

    // Navigation
    public Faculty Faculty { get; set; } = null!;

    public SemesterSubject SemesterSubject { get; set; } = null!;

    public AcademicYear AcademicYear { get; set; } = null!;
}
```

---

# 17. ClassSession

I strongly recommend this addition for attendance.

### `ClassSession.cs`

```csharp
namespace StudentManagementSystem.Domain.Entities;

public class ClassSession : BaseEntity
{
    public int SessionId { get; set; }

    public int SemesterSubjectId { get; set; }

    public int FacultyId { get; set; }

    public DateTime SessionDate { get; set; }

    public TimeSpan StartTime { get; set; }

    public TimeSpan EndTime { get; set; }

    public string? Topic { get; set; }

    // Navigation
    public SemesterSubject SemesterSubject { get; set; } = null!;

    public Faculty Faculty { get; set; } = null!;

    public ICollection<AttendanceRecord> AttendanceRecords { get; set; }
        = new List<AttendanceRecord>();
}
```

This solves the problem of:

```text
Java
14-Aug
Lecture 1
Lecture 2
```

being two separate sessions.

---

# 18. Attendance

### `Attendance.cs`

```csharp
namespace StudentManagementSystem.Domain.Entities;

public class Attendance : BaseEntity
{
    public int AttendanceId { get; set; }

    public int StudentSemesterId { get; set; }

    public int SemesterSubjectId { get; set; }

    public int ClassesHeld { get; set; }

    public int ClassesAttended { get; set; }

    public decimal AttendancePercentage { get; set; }

    // Navigation
    public StudentSemester StudentSemester { get; set; } = null!;

    public SemesterSubject SemesterSubject { get; set; } = null!;
}
```

This is your **aggregated attendance**.

For example:

```text
Java
Classes Held     = 40
Classes Attended = 35
Percentage       = 87.5%
```

---

# 19. AttendanceRecord

### `AttendanceRecord.cs`

```csharp
namespace StudentManagementSystem.Domain.Entities;

public class AttendanceRecord : BaseEntity
{
    public int AttendanceRecordId { get; set; }

    public int SessionId { get; set; }

    public int StudentSemesterId { get; set; }

    public string Status { get; set; } = null!;

    public string? Remarks { get; set; }

    // Navigation
    public ClassSession Session { get; set; } = null!;

    public StudentSemester StudentSemester { get; set; } = null!;
}
```

Now:

```text
ClassSession
     │
     └──< AttendanceRecord >── StudentSemester
```

---

# 20. Project

### `Project.cs`

```csharp
namespace StudentManagementSystem.Domain.Entities;

public class Project : BaseEntity
{
    public int ProjectId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int SemesterId { get; set; }

    public int ProgramId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    // Navigation
    public Semester Semester { get; set; } = null!;

    public AcademicProgram Program { get; set; } = null!;

    public ICollection<ProjectAllocation> ProjectAllocations { get; set; }
        = new List<ProjectAllocation>();
}
```

---

# 21. ProjectAllocation

### `ProjectAllocation.cs`

```csharp
namespace StudentManagementSystem.Domain.Entities;

public class ProjectAllocation : BaseEntity
{
    public int AllocationId { get; set; }

    public int ProjectId { get; set; }

    public int StudentId { get; set; }

    public int FacultyId { get; set; }

    public decimal? FinalScore { get; set; }

    public string? Grade { get; set; }

    public string Status { get; set; } = null!;

    public string? RepositoryUrl { get; set; }

    // Navigation
    public Project Project { get; set; } = null!;

    public Student Student { get; set; } = null!;

    public Faculty Faculty { get; set; } = null!;

    public ICollection<ProjectTask> Tasks { get; set; }
        = new List<ProjectTask>();
}
```

---

# 22. Task

I recommend naming the entity `ProjectTask` because `Task` can become confusing with `System.Threading.Tasks.Task`.

### `ProjectTask.cs`

```csharp
namespace StudentManagementSystem.Domain.Entities;

public class ProjectTask : BaseEntity
{
    public int TaskId { get; set; }

    public int ProjectAllocationId { get; set; }

    public string TaskTitle { get; set; } = null!;

    public string? TaskDescription { get; set; }

    public string TaskStatus { get; set; } = null!;

    public decimal AssignedScore { get; set; }

    public decimal EarnedScore { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime DueDate { get; set; }

    public DateTime? CompletedDate { get; set; }

    public string? FacultyRemarks { get; set; }

    public string? StudentRemarks { get; set; }

    // Navigation
    public ProjectAllocation ProjectAllocation { get; set; } = null!;
}
```

---

# 23. Material

### `Material.cs`

```csharp
namespace StudentManagementSystem.Domain.Entities;

public class Material : BaseEntity
{
    public int MaterialId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int SemesterSubjectId { get; set; }

    public int UploadedByUserId { get; set; }

    public string MaterialType { get; set; } = null!;

    public string FileName { get; set; } = null!;

    public string FileUrl { get; set; } = null!;

    public long FileSize { get; set; }

    // Navigation
    public SemesterSubject SemesterSubject { get; set; } = null!;

    public User UploadedByUser { get; set; } = null!;
}
```

---

# 24. Now the important part: DbContext

This is where I'd put the **majority of your database constraints and indexes**.

### `ApplicationDbContext.cs`

```csharp
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Role> Roles => Set<Role>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<AcademicProgram> Programs => Set<AcademicProgram>();
    public DbSet<AcademicYear> AcademicYears => Set<AcademicYear>();
    public DbSet<Semester> Semesters => Set<Semester>();
    public DbSet<Subject> Subjects => Set<Subject>();
    public DbSet<SemesterSubject> SemesterSubjects => Set<SemesterSubject>();

    public DbSet<Student> Students => Set<Student>();
    public DbSet<StudentSemester> StudentSemesters => Set<StudentSemester>();
    public DbSet<SemesterResult> SemesterResults => Set<SemesterResult>();
    public DbSet<SubjectResult> SubjectResults => Set<SubjectResult>();

    public DbSet<Faculty> Faculties => Set<Faculty>();
    public DbSet<FacultySubject> FacultySubjects => Set<FacultySubject>();

    public DbSet<ClassSession> ClassSessions => Set<ClassSession>();
    public DbSet<Attendance> Attendances => Set<Attendance>();
    public DbSet<AttendanceRecord> AttendanceRecords => Set<AttendanceRecord>();

    public DbSet<Project> Projects => Set<Project>();
    public DbSet<ProjectAllocation> ProjectAllocations => Set<ProjectAllocation>();
    public DbSet<ProjectTask> ProjectTasks => Set<ProjectTask>();

    public DbSet<Material> Materials => Set<Material>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureRole(modelBuilder);
        ConfigureUser(modelBuilder);
        ConfigureDepartment(modelBuilder);
        ConfigureProgram(modelBuilder);
        ConfigureAcademicYear(modelBuilder);
        ConfigureSemester(modelBuilder);
        ConfigureSubject(modelBuilder);
        ConfigureSemesterSubject(modelBuilder);

        ConfigureStudent(modelBuilder);
        ConfigureStudentSemester(modelBuilder);
        ConfigureSemesterResult(modelBuilder);
        ConfigureSubjectResult(modelBuilder);

        ConfigureFaculty(modelBuilder);
        ConfigureFacultySubject(modelBuilder);

        ConfigureClassSession(modelBuilder);
        ConfigureAttendance(modelBuilder);
        ConfigureAttendanceRecord(modelBuilder);

        ConfigureProject(modelBuilder);
        ConfigureProjectAllocation(modelBuilder);
        ConfigureProjectTask(modelBuilder);

        ConfigureMaterial(modelBuilder);
    }
}
```

Now let's configure each entity.

---

# 25. Role configuration

```csharp
private static void ConfigureRole(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Role>(entity =>
    {
        entity.ToTable("Roles");

        entity.HasKey(x => x.RoleId);

        entity.Property(x => x.RoleName)
            .HasMaxLength(50)
            .IsRequired();

        entity.HasIndex(x => x.RoleName)
            .IsUnique();

        entity.Property(x => x.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        entity.Property(x => x.UpdatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        entity.HasMany(x => x.Users)
            .WithOne(x => x.Role)
            .HasForeignKey(x => x.RoleId)
            .OnDelete(DeleteBehavior.Restrict);
    });
}
```

---

# 26. User configuration

```csharp
private static void ConfigureUser(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<User>(entity =>
    {
        entity.ToTable("Users");

        entity.HasKey(x => x.UserId);

        entity.Property(x => x.UserName)
            .HasMaxLength(100)
            .IsRequired();

        entity.Property(x => x.Email)
            .HasMaxLength(255)
            .IsRequired();

        entity.Property(x => x.PasswordHash)
            .HasMaxLength(500)
            .IsRequired();

        entity.Property(x => x.Address)
            .HasMaxLength(500);

        entity.Property(x => x.ProfilePhoto)
            .HasMaxLength(500);

        entity.Property(x => x.IsActive)
            .HasDefaultValue(true);

        entity.HasIndex(x => x.UserName)
            .IsUnique();

        entity.HasIndex(x => x.Email)
            .IsUnique();

        entity.HasIndex(x => x.RoleId);

        entity.HasOne(x => x.Role)
            .WithMany(x => x.Users)
            .HasForeignKey(x => x.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(x => x.Student)
            .WithOne(x => x.User)
            .HasForeignKey<Student>(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(x => x.Faculty)
            .WithOne(x => x.User)
            .HasForeignKey<Faculty>(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    });
}
```

---

# 27. Department

```csharp
private static void ConfigureDepartment(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Department>(entity =>
    {
        entity.ToTable("Departments");

        entity.HasKey(x => x.DepartmentId);

        entity.Property(x => x.DepartmentName)
            .HasMaxLength(150)
            .IsRequired();

        entity.Property(x => x.DepartmentCode)
            .HasMaxLength(20)
            .IsRequired();

        entity.Property(x => x.Description)
            .HasMaxLength(500);

        entity.Property(x => x.IsActive)
            .HasDefaultValue(true);

        entity.HasIndex(x => x.DepartmentName)
            .IsUnique();

        entity.HasIndex(x => x.DepartmentCode)
            .IsUnique();
    });
}
```

---

# 28. Program

```csharp
private static void ConfigureProgram(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<AcademicProgram>(entity =>
    {
        entity.ToTable("Programs");

        entity.HasKey(x => x.ProgramId);

        entity.Property(x => x.ProgramName)
            .HasMaxLength(150)
            .IsRequired();

        entity.Property(x => x.ProgramCode)
            .HasMaxLength(30)
            .IsRequired();

        entity.Property(x => x.DurationYears)
            .IsRequired();

        entity.Property(x => x.TotalSemesters)
            .IsRequired();

        entity.Property(x => x.IsActive)
            .HasDefaultValue(true);

        entity.HasIndex(x => x.ProgramCode)
            .IsUnique();

        entity.HasIndex(x => x.DepartmentId);

        entity.HasIndex(x => new
        {
            x.DepartmentId,
            x.ProgramName
        })
        .IsUnique();

        entity.ToTable(t =>
        {
            t.HasCheckConstraint(
                "CK_Program_DurationYears",
                "[DurationYears] > 0");

            t.HasCheckConstraint(
                "CK_Program_TotalSemesters",
                "[TotalSemesters] > 0");
        });

        entity.HasOne(x => x.Department)
            .WithMany(x => x.Programs)
            .HasForeignKey(x => x.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);
    });
}
```

---

# 29. AcademicYear

```csharp
private static void ConfigureAcademicYear(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<AcademicYear>(entity =>
    {
        entity.ToTable("AcademicYears");

        entity.HasKey(x => x.AcademicYearId);

        entity.Property(x => x.YearName)
            .HasMaxLength(20)
            .IsRequired();

        entity.HasIndex(x => x.YearName)
            .IsUnique();

        entity.HasIndex(x => x.IsActive);

        entity.ToTable(t =>
        {
            t.HasCheckConstraint(
                "CK_AcademicYear_DateRange",
                "[EndDate] > [StartDate]");
        });
    });
}
```

---

# 30. Semester

```csharp
private static void ConfigureSemester(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Semester>(entity =>
    {
        entity.ToTable("Semesters");

        entity.HasKey(x => x.SemesterId);

        entity.Property(x => x.SemesterName)
            .HasMaxLength(50)
            .IsRequired();

        entity.HasIndex(x => x.SemesterNumber)
            .IsUnique();

        entity.HasIndex(x => x.SemesterName)
            .IsUnique();

        entity.Property(x => x.IsActive)
            .HasDefaultValue(true);
    });
}
```

---

# 31. Subject

```csharp
private static void ConfigureSubject(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Subject>(entity =>
    {
        entity.ToTable("Subjects");

        entity.HasKey(x => x.SubjectId);

        entity.Property(x => x.SubjectCode)
            .HasMaxLength(30)
            .IsRequired();

        entity.Property(x => x.SubjectName)
            .HasMaxLength(150)
            .IsRequired();

        entity.Property(x => x.SubjectType)
            .HasMaxLength(50)
            .IsRequired();

        entity.HasIndex(x => x.SubjectCode)
            .IsUnique();

        entity.HasIndex(x => x.SubjectName);
    });
}
```

---

# 32. SemesterSubject

```csharp
private static void ConfigureSemesterSubject(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<SemesterSubject>(entity =>
    {
        entity.ToTable("SemesterSubjects");

        entity.HasKey(x => x.SemesterSubjectId);

        entity.Property(x => x.Credits)
            .HasPrecision(5, 2)
            .IsRequired();

        entity.HasIndex(x => new
        {
            x.ProgramId,
            x.SemesterId,
            x.SubjectId
        })
        .IsUnique();

        entity.HasIndex(x => new
        {
            x.ProgramId,
            x.SemesterId
        })
        .IncludeProperties(x => new
        {
            x.SubjectId,
            x.Credits
        });

        entity.ToTable(t =>
        {
            t.HasCheckConstraint(
                "CK_SemesterSubject_Credits",
                "[Credits] > 0");
        });

        entity.HasOne(x => x.Program)
            .WithMany(x => x.SemesterSubjects)
            .HasForeignKey(x => x.ProgramId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(x => x.Semester)
            .WithMany(x => x.SemesterSubjects)
            .HasForeignKey(x => x.SemesterId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(x => x.Subject)
            .WithMany(x => x.SemesterSubjects)
            .HasForeignKey(x => x.SubjectId)
            .OnDelete(DeleteBehavior.Restrict);
    });
}
```

---

# 33. Student

```csharp
private static void ConfigureStudent(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Student>(entity =>
    {
        entity.ToTable("Students");

        entity.HasKey(x => x.StudentId);

        entity.Property(x => x.EnrollmentNumber)
            .HasMaxLength(50)
            .IsRequired();

        entity.HasIndex(x => x.EnrollmentNumber)
            .IsUnique();

        entity.HasIndex(x => x.UserId)
            .IsUnique();

        entity.HasIndex(x => x.ProgramId);

        entity.HasIndex(x => x.CurrentSemesterId);

        entity.HasOne(x => x.User)
            .WithOne(x => x.Student)
            .HasForeignKey<Student>(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(x => x.Program)
            .WithMany(x => x.Students)
            .HasForeignKey(x => x.ProgramId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(x => x.CurrentSemester)
            .WithMany()
            .HasForeignKey(x => x.CurrentSemesterId)
            .OnDelete(DeleteBehavior.Restrict);
    });
}
```

---

# 34. StudentSemester

```csharp
private static void ConfigureStudentSemester(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<StudentSemester>(entity =>
    {
        entity.ToTable("StudentSemesters");

        entity.HasKey(x => x.StudentSemesterId);

        entity.Property(x => x.Status)
            .HasMaxLength(30)
            .IsRequired();

        entity.HasIndex(x => new
        {
            x.StudentId,
            x.SemesterId,
            x.AcademicYearId
        })
        .IsUnique();

        entity.HasIndex(x => x.SemesterId);

        entity.HasIndex(x => x.AcademicYearId);

        entity.HasOne(x => x.Student)
            .WithMany(x => x.StudentSemesters)
            .HasForeignKey(x => x.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(x => x.Semester)
            .WithMany(x => x.StudentSemesters)
            .HasForeignKey(x => x.SemesterId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(x => x.AcademicYear)
            .WithMany(x => x.StudentSemesters)
            .HasForeignKey(x => x.AcademicYearId)
            .OnDelete(DeleteBehavior.Restrict);
    });
}
```

---

# 35. SemesterResult

```csharp
private static void ConfigureSemesterResult(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<SemesterResult>(entity =>
    {
        entity.ToTable("SemesterResults");

        entity.HasKey(x => x.SemesterResultId);

        entity.Property(x => x.SGPA)
            .HasPrecision(4, 2);

        entity.Property(x => x.TotalCredits)
            .HasPrecision(5, 2);

        entity.Property(x => x.EarnedCredits)
            .HasPrecision(5, 2);

        entity.Property(x => x.ResultStatus)
            .HasMaxLength(30)
            .IsRequired();

        entity.HasIndex(x => x.StudentSemesterId)
            .IsUnique();

        entity.ToTable(t =>
        {
            t.HasCheckConstraint(
                "CK_SemesterResult_SGPA",
                "[SGPA] >= 0 AND [SGPA] <= 10");

            t.HasCheckConstraint(
                "CK_SemesterResult_Credits",
                "[EarnedCredits] >= 0 AND [EarnedCredits] <= [TotalCredits]");
        });

        entity.HasOne(x => x.StudentSemester)
            .WithOne(x => x.SemesterResult)
            .HasForeignKey<SemesterResult>(x => x.StudentSemesterId)
            .OnDelete(DeleteBehavior.Restrict);
    });
}
```

---

# 36. SubjectResult

```csharp
private static void ConfigureSubjectResult(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<SubjectResult>(entity =>
    {
        entity.ToTable("SubjectResults");

        entity.HasKey(x => x.SubjectResultId);

        entity.Property(x => x.InternalMarks)
            .HasPrecision(5, 2);

        entity.Property(x => x.ExternalMarks)
            .HasPrecision(5, 2);

        entity.Property(x => x.PracticalMarks)
            .HasPrecision(5, 2);

        entity.Property(x => x.TotalMarks)
            .HasPrecision(5, 2);

        entity.Property(x => x.Grade)
            .HasMaxLength(5)
            .IsRequired();

        entity.Property(x => x.GradePoint)
            .HasPrecision(4, 2);

        entity.Property(x => x.CreditsEarned)
            .HasPrecision(5, 2);

        entity.Property(x => x.ResultStatus)
            .HasMaxLength(30)
            .IsRequired();

        entity.HasIndex(x => new
        {
            x.SemesterResultId,
            x.SemesterSubjectId
        })
        .IsUnique();

        entity.HasIndex(x => x.SemesterSubjectId);

        entity.ToTable(t =>
        {
            t.HasCheckConstraint(
                "CK_SubjectResult_Marks",
                "[InternalMarks] >= 0 AND " +
                "[ExternalMarks] >= 0 AND " +
                "[PracticalMarks] >= 0 AND " +
                "[TotalMarks] >= 0");

            t.HasCheckConstraint(
                "CK_SubjectResult_GradePoint",
                "[GradePoint] >= 0 AND [GradePoint] <= 10");

            t.HasCheckConstraint(
                "CK_SubjectResult_CreditsEarned",
                "[CreditsEarned] >= 0");
        });

        entity.HasOne(x => x.SemesterResult)
            .WithMany(x => x.SubjectResults)
            .HasForeignKey(x => x.SemesterResultId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(x => x.SemesterSubject)
            .WithMany(x => x.SubjectResults)
            .HasForeignKey(x => x.SemesterSubjectId)
            .OnDelete(DeleteBehavior.Restrict);
    });
}
```

---

# 37. Faculty

```csharp
private static void ConfigureFaculty(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Faculty>(entity =>
    {
        entity.ToTable("Faculties");

        entity.HasKey(x => x.FacultyId);

        entity.Property(x => x.EmployeeNumber)
            .HasMaxLength(50)
            .IsRequired();

        entity.Property(x => x.Designation)
            .HasMaxLength(100)
            .IsRequired();

        entity.HasIndex(x => x.UserId)
            .IsUnique();

        entity.HasIndex(x => x.EmployeeNumber)
            .IsUnique();

        entity.HasIndex(x => x.DepartmentId);

        entity.HasOne(x => x.User)
            .WithOne(x => x.Faculty)
            .HasForeignKey<Faculty>(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(x => x.Department)
            .WithMany(x => x.Faculties)
            .HasForeignKey(x => x.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);
    });
}
```

---

# 38. FacultySubject

```csharp
private static void ConfigureFacultySubject(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<FacultySubject>(entity =>
    {
        entity.ToTable("FacultySubjects");

        entity.HasKey(x => x.FacultySubjectId);

        entity.HasIndex(x => new
        {
            x.FacultyId,
            x.SemesterSubjectId,
            x.AcademicYearId
        })
        .IsUnique();

        entity.HasIndex(x => x.SemesterSubjectId);

        entity.HasIndex(x => x.AcademicYearId);

        entity.HasOne(x => x.Faculty)
            .WithMany(x => x.FacultySubjects)
            .HasForeignKey(x => x.FacultyId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(x => x.SemesterSubject)
            .WithMany(x => x.FacultySubjects)
            .HasForeignKey(x => x.SemesterSubjectId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(x => x.AcademicYear)
            .WithMany(x => x.FacultySubjects)
            .HasForeignKey(x => x.AcademicYearId)
            .OnDelete(DeleteBehavior.Restrict);
    });
}
```

---

# 39. ClassSession

```csharp
private static void ConfigureClassSession(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<ClassSession>(entity =>
    {
        entity.ToTable("ClassSessions");

        entity.HasKey(x => x.SessionId);

        entity.Property(x => x.Topic)
            .HasMaxLength(300);

        entity.HasIndex(x => new
        {
            x.SemesterSubjectId,
            x.SessionDate
        });

        entity.HasIndex(x => new
        {
            x.FacultyId,
            x.SessionDate
        });

        entity.HasOne(x => x.SemesterSubject)
            .WithMany(x => x.ClassSessions)
            .HasForeignKey(x => x.SemesterSubjectId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(x => x.Faculty)
            .WithMany(x => x.ClassSessions)
            .HasForeignKey(x => x.FacultyId)
            .OnDelete(DeleteBehavior.Restrict);
    });
}
```

---

# 40. Attendance

```csharp
private static void ConfigureAttendance(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Attendance>(entity =>
    {
        entity.ToTable("Attendances");

        entity.HasKey(x => x.AttendanceId);

        entity.Property(x => x.AttendancePercentage)
            .HasPrecision(5, 2);

        entity.HasIndex(x => new
        {
            x.StudentSemesterId,
            x.SemesterSubjectId
        })
        .IsUnique();

        entity.HasIndex(x => x.SemesterSubjectId);

        entity.ToTable(t =>
        {
            t.HasCheckConstraint(
                "CK_Attendance_Classes",
                "[ClassesHeld] >= 0 AND " +
                "[ClassesAttended] >= 0 AND " +
                "[ClassesAttended] <= [ClassesHeld]");

            t.HasCheckConstraint(
                "CK_Attendance_Percentage",
                "[AttendancePercentage] >= 0 AND " +
                "[AttendancePercentage] <= 100");
        });

        entity.HasOne(x => x.StudentSemester)
            .WithMany(x => x.Attendances)
            .HasForeignKey(x => x.StudentSemesterId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(x => x.SemesterSubject)
            .WithMany(x => x.Attendances)
            .HasForeignKey(x => x.SemesterSubjectId)
            .OnDelete(DeleteBehavior.Restrict);
    });
}
```

---

# 41. AttendanceRecord

```csharp
private static void ConfigureAttendanceRecord(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<AttendanceRecord>(entity =>
    {
        entity.ToTable("AttendanceRecords");

        entity.HasKey(x => x.AttendanceRecordId);

        entity.Property(x => x.Status)
            .HasMaxLength(20)
            .IsRequired();

        entity.Property(x => x.Remarks)
            .HasMaxLength(500);

        entity.HasIndex(x => new
        {
            x.SessionId,
            x.StudentSemesterId
        })
        .IsUnique();

        entity.HasIndex(x => x.StudentSemesterId);

        entity.HasOne(x => x.Session)
            .WithMany(x => x.AttendanceRecords)
            .HasForeignKey(x => x.SessionId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(x => x.StudentSemester)
            .WithMany(x => x.AttendanceRecords)
            .HasForeignKey(x => x.StudentSemesterId)
            .OnDelete(DeleteBehavior.Restrict);
    });
}
```

---

# 42. Project

```csharp
private static void ConfigureProject(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Project>(entity =>
    {
        entity.ToTable("Projects");

        entity.HasKey(x => x.ProjectId);

        entity.Property(x => x.Title)
            .HasMaxLength(200)
            .IsRequired();

        entity.Property(x => x.Description)
            .HasMaxLength(2000);

        entity.HasIndex(x => new
        {
            x.ProgramId,
            x.SemesterId
        });

        entity.ToTable(t =>
        {
            t.HasCheckConstraint(
                "CK_Project_DateRange",
                "[EndDate] >= [StartDate]");
        });

        entity.HasOne(x => x.Program)
            .WithMany(x => x.Projects)
            .HasForeignKey(x => x.ProgramId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(x => x.Semester)
            .WithMany(x => x.Projects)
            .HasForeignKey(x => x.SemesterId)
            .OnDelete(DeleteBehavior.Restrict);
    });
}
```

---

# 43. ProjectAllocation

```csharp
private static void ConfigureProjectAllocation(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<ProjectAllocation>(entity =>
    {
        entity.ToTable("ProjectAllocations");

        entity.HasKey(x => x.AllocationId);

        entity.Property(x => x.FinalScore)
            .HasPrecision(5, 2);

        entity.Property(x => x.Grade)
            .HasMaxLength(5);

        entity.Property(x => x.Status)
            .HasMaxLength(30)
            .IsRequired();

        entity.Property(x => x.RepositoryUrl)
            .HasMaxLength(500);

        entity.HasIndex(x => new
        {
            x.ProjectId,
            x.StudentId
        })
        .IsUnique();

        entity.HasIndex(x => x.StudentId);

        entity.HasIndex(x => x.FacultyId);

        entity.ToTable(t =>
        {
            t.HasCheckConstraint(
                "CK_ProjectAllocation_FinalScore",
                "[FinalScore] IS NULL OR " +
                "([FinalScore] >= 0 AND [FinalScore] <= 100)");
        });

        entity.HasOne(x => x.Project)
            .WithMany(x => x.ProjectAllocations)
            .HasForeignKey(x => x.ProjectId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(x => x.Student)
            .WithMany(x => x.ProjectAllocations)
            .HasForeignKey(x => x.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(x => x.Faculty)
            .WithMany(x => x.ProjectAllocations)
            .HasForeignKey(x => x.FacultyId)
            .OnDelete(DeleteBehavior.Restrict);
    });
}
```

---

# 44. ProjectTask

```csharp
private static void ConfigureProjectTask(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<ProjectTask>(entity =>
    {
        entity.ToTable("ProjectTasks");

        entity.HasKey(x => x.TaskId);

        entity.Property(x => x.TaskTitle)
            .HasMaxLength(200)
            .IsRequired();

        entity.Property(x => x.TaskDescription)
            .HasMaxLength(2000);

        entity.Property(x => x.TaskStatus)
            .HasMaxLength(30)
            .IsRequired();

        entity.Property(x => x.AssignedScore)
            .HasPrecision(5, 2);

        entity.Property(x => x.EarnedScore)
            .HasPrecision(5, 2);

        entity.Property(x => x.FacultyRemarks)
            .HasMaxLength(1000);

        entity.Property(x => x.StudentRemarks)
            .HasMaxLength(1000);

        entity.HasIndex(x => x.ProjectAllocationId);

        entity.HasIndex(x => new
        {
            x.ProjectAllocationId,
            x.TaskStatus
        });

        entity.HasIndex(x => x.DueDate);

        entity.ToTable(t =>
        {
            t.HasCheckConstraint(
                "CK_ProjectTask_Scores",
                "[AssignedScore] >= 0 AND " +
                "[EarnedScore] >= 0 AND " +
                "[EarnedScore] <= [AssignedScore]");

            t.HasCheckConstraint(
                "CK_ProjectTask_DateRange",
                "[DueDate] >= [StartDate]");

            t.HasCheckConstraint(
                "CK_ProjectTask_CompletedDate",
                "[CompletedDate] IS NULL OR " +
                "[CompletedDate] >= [StartDate]");
        });

        entity.HasOne(x => x.ProjectAllocation)
            .WithMany(x => x.Tasks)
            .HasForeignKey(x => x.ProjectAllocationId)
            .OnDelete(DeleteBehavior.Restrict);
    });
}
```

---

# 45. Material

```csharp
private static void ConfigureMaterial(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Material>(entity =>
    {
        entity.ToTable("Materials");

        entity.HasKey(x => x.MaterialId);

        entity.Property(x => x.Title)
            .HasMaxLength(200)
            .IsRequired();

        entity.Property(x => x.Description)
            .HasMaxLength(1000);

        entity.Property(x => x.MaterialType)
            .HasMaxLength(30)
            .IsRequired();

        entity.Property(x => x.FileName)
            .HasMaxLength(255)
            .IsRequired();

        entity.Property(x => x.FileUrl)
            .HasMaxLength(1000)
            .IsRequired();

        entity.HasIndex(x => x.SemesterSubjectId);

        entity.HasIndex(x => x.UploadedByUserId);

        entity.HasOne(x => x.SemesterSubject)
            .WithMany(x => x.Materials)
            .HasForeignKey(x => x.SemesterSubjectId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(x => x.UploadedByUser)
            .WithMany(x => x.UploadedMaterials)
            .HasForeignKey(x => x.UploadedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.ToTable(t =>
        {
            t.HasCheckConstraint(
                "CK_Material_FileSize",
                "[FileSize] > 0");
        });
    });
}
```

---

# 46. One thing missing: audit timestamps

Since you're using:

```csharp
CreatedAt
UpdatedAt
```

I'd handle them automatically in your DbContext.

Add:

```csharp
public override async Task<int> SaveChangesAsync(
    CancellationToken cancellationToken = default)
{
    var entries = ChangeTracker
        .Entries<BaseEntity>();

    foreach (var entry in entries)
    {
        if (entry.State == EntityState.Added)
        {
            entry.Entity.CreatedAt = DateTime.UtcNow;
            entry.Entity.UpdatedAt = DateTime.UtcNow;
        }
        else if (entry.State == EntityState.Modified)
        {
            entry.Entity.UpdatedAt = DateTime.UtcNow;
        }
    }

    return await base.SaveChangesAsync(cancellationToken);
}
```

And similarly for synchronous `SaveChanges()` if your application uses it.

---

# 47. One important correction about `CreatedAt`

If you use:

```csharp
CreatedAt
    .HasDefaultValueSql("GETUTCDATE()");
```

**and** also set `CreatedAt` in `SaveChangesAsync`, you don't really need both.

For an application-controlled architecture, I'd prefer:

```text
EF Core SaveChanges
       ↓
CreatedAt / UpdatedAt
```

and remove the SQL defaults.

Or use database defaults and let SQL Server own the timestamps.

Don't unnecessarily maintain two sources of truth.

---

# 48. Migration

Once these are in place:

```powershell
Add-Migration InitialCreate
Update-Database
```

Or CLI:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

EF Core will generate:

* tables
* PKs
* FKs
* unique indexes
* normal indexes
* composite indexes
* check constraints
* relationships

---

# 49. Final relationship picture

Your final model is approximately:

```text
                           ┌──────────┐
                           │   Role   │
                           └────┬─────┘
                                │ 1
                                │
                                │ *
                           ┌────▼─────┐
                           │   User   │
                           └────┬─────┘
                         ┌──────┴──────┐
                         │             │
                         ▼             ▼
                    ┌─────────┐   ┌─────────┐
                    │ Student │   │ Faculty │
                    └────┬────┘   └────┬────┘
                         │              │
                         │              │
                    ┌────▼───────┐      │
                    │ Student    │      │
                    │ Semester   │      │
                    └────┬───────┘      │
                         │              │
                    ┌────▼───────┐      │
                    │  Semester  │      │
                    │   Result   │      │
                    └────┬───────┘      │
                         │              │
                    ┌────▼───────┐      │
                    │  Subject   │      │
                    │   Result   │      │
                    └────┬───────┘      │
                         │              │
                         ▼              ▼
                   SemesterSubject ◄ FacultySubject
                         │
              ┌──────────┼───────────┐
              │          │           │
              ▼          ▼           ▼
           Material   ClassSession Attendance
                         │
                         ▼
                  AttendanceRecord


Department
    │
    └──< AcademicProgram
             │
             └──< SemesterSubject


Project
    │
    └──< ProjectAllocation
             │
        ┌────┴─────┐
        ▼          ▼
     Student     Faculty
        │
        └──< ProjectTask
```

This is a much stronger foundation than simply translating your original tables directly into C# classes.

**One architectural point:** don't put validation such as `InternalMarks <= 30`, allowed grades, project statuses, attendance statuses, etc. blindly into the entity classes. Those are **business rules**. Keep structural integrity in the database/EF configuration and business rules in your Application/Domain services.

If you're going to build this with **Clean Architecture**, the next logical step is to split these entities into **Domain entities + EF Core configurations (`IEntityTypeConfiguration<T>`) + repositories + services + DTOs**, rather than keeping this huge `ApplicationDbContext` configuration file.
