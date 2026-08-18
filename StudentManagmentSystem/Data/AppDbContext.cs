using Microsoft.EntityFrameworkCore;
using StudentManagmentSystem.Models;

namespace StudentManagmentSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<AcademicProgram> Programs { get; set; }
        public DbSet<Semester> Semesters { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<SemesterSubject> SemesterSubjects { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentSemester> StudentSemesters { get; set; }
        public DbSet<SemesterResult> SemesterResults { get; set; }
        public DbSet<SubjectResult> SubjectResults { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<FacultySubject> FacultySubjects { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<AttendanceRecord> AttendanceRecords { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectAllocation> ProjectAllocations { get; set; }
        public DbSet<ProjectTask> ProjectTasks { get; set; }
        public DbSet<Material> Materials { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasMany(r => r.Users)
                    .WithOne(u => u.Role)
                    .HasForeignKey(u => u.RoleId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasMany(d => d.Programs)
                    .WithOne(p => p.Department)
                    .HasForeignKey(p => p.DepartmentId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(d => d.Faculties)
                    .WithOne(f => f.Department)
                    .HasForeignKey(f => f.DepartmentId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<AcademicProgram>(entity =>
            {

                entity.ToTable(t =>
                {
                    t.HasCheckConstraint(
                        "CK_Program_DurationYears",
                        "[DurationYears] > 0");

                    t.HasCheckConstraint(
                        "CK_Program_TotalSemesters",
                        "[TotalSemesters] > 0");
                });

                entity.HasMany(p => p.SemesterSubjects)
                    .WithOne(ss => ss.AcademicProgram)
                    .HasForeignKey(ss => ss.ProgramId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(p => p.Students)
                    .WithOne(s => s.AcademicProgram)
                    .HasForeignKey(s => s.ProgramId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(p => p.Projects)
                    .WithOne(pr => pr.AcademicProgram)
                    .HasForeignKey(pr => pr.ProgramId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Semester>(entity =>
            {
                entity.HasMany(s => s.SemesterSubjects)
                    .WithOne(ss => ss.Semester)
                    .HasForeignKey(ss => ss.SemesterId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(s => s.StudentsAsCurrentSemester)
                    .WithOne(st => st.CurrentSemester)
                    .HasForeignKey(st => st.CurrentSemesterId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasMany(s => s.StudentSemesters)
                    .WithOne(ss => ss.Semester)
                    .HasForeignKey(ss => ss.SemesterId)
                    .OnDelete(DeleteBehavior.Restrict);

             

                entity.HasMany(s => s.Projects)
                    .WithOne(p => p.Semester)
                    .HasForeignKey(p => p.SemesterId)
                    .OnDelete(DeleteBehavior.Restrict);

            });

            modelBuilder.Entity<SemesterSubject>(entity =>
            {
                entity.ToTable(t =>
                {
                    t.HasCheckConstraint(
                        "CK_SemesterSubject_Credits",
                        "[Credits] > 0");
                });
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasOne(x => x.User)
                .WithOne(x => x.Student)
                .HasForeignKey<Student>(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.AcademicProgram)
                    .WithMany(x => x.Students)
                    .HasForeignKey(x => x.ProgramId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.CurrentSemester)
                    .WithMany()
                    .HasForeignKey(x => x.CurrentSemesterId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<StudentSemester>(entity =>
            {
                entity.HasOne(ss => ss.Student)
                    .WithMany(s => s.StudentSemesters)
                    .HasForeignKey(ss => ss.StudentId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ss => ss.Semester)
                    .WithMany(s => s.StudentSemesters)
                    .HasForeignKey(ss => ss.SemesterId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(ss => ss.SemesterResult)
                    .WithOne(sr => sr.StudentSemester)
                    .HasForeignKey<SemesterResult>(sr => sr.StudentSemesterId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<SemesterResult>(entity =>
            {
                entity.ToTable(t =>
                {
                    t.HasCheckConstraint(
                        "CK_SemesterResult_SGPA",
                        "[SGPA] >= 0 AND [SGPA] <= 10");

                    t.HasCheckConstraint(
                        "CK_SemesterResult_Credits",
                        "[EarnedCredits] >= 0 AND [EarnedCredits] <= [TotalCredits]");
                });
                entity.HasMany(sr => sr.SubjectResults)
                    .WithOne(su => su.SemesterResult)
                    .HasForeignKey(su => su.SemesterResultId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<SubjectResult>(entity =>
            {
                entity.ToTable(t =>
                {
                    t.HasCheckConstraint(
                        "CK_SubjectResult_Marks",
                        "[InternalMarks] >= 0 AND " +
                        "[ExternalMarks] >= 0 AND " +
                        "[PracticalMarks] >= 0 AND " +
                        "[TotalMarks] >= 0");

                    t.HasCheckConstraint(
                        "CK_SubjectResult_CreditsEarned",
                        "[CreditsEarned] >= 0");
                });

                entity.HasOne(sr => sr.SemesterSubject)
                    .WithMany(ss => ss.SubjectResults)
                    .HasForeignKey(sr => sr.SemesterSubjectId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Faculty>(entity =>
            {
                entity.HasOne(f => f.User)
                    .WithOne(u => u.Faculty)
                    .HasForeignKey<Faculty>(f => f.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(f => f.FacultySubjects)
                    .WithOne(fs => fs.Faculty)
                    .HasForeignKey(fs => fs.FacultyId)
                    .OnDelete(DeleteBehavior.Cascade);


                entity.HasMany(f => f.ProjectAllocations)
                    .WithOne(pa => pa.Faculty)
                    .HasForeignKey(pa => pa.FacultyId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasMany(u => u.UploadedMaterials)
                    .WithOne(m => m.UploadedByUser)
                    .HasForeignKey(m => m.UploadedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            modelBuilder.Entity<Attendance>(entity =>
            {
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
            });

            

            modelBuilder.Entity<Project>(entity =>
            {
                entity.ToTable(t =>
                {
                    t.HasCheckConstraint(
                        "CK_Project_DateRange",
                        "[EndDate] >= [StartDate]");
                });
                entity.HasMany(p => p.ProjectAllocations)
                    .WithOne(pa => pa.Project)
                    .HasForeignKey(pa => pa.ProjectId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ProjectAllocation>(entity =>
            {
                entity.ToTable(t =>
                {
                    t.HasCheckConstraint(
                        "CK_ProjectAllocation_FinalScore",
                        "[FinalScore] IS NULL OR " +
                        "([FinalScore] >= 0 AND [FinalScore] <= 100)");
                });
                entity.HasMany(pa => pa.Tasks)
                    .WithOne(t => t.ProjectAllocation)
                    .HasForeignKey(t => t.ProjectAllocationId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ProjectTask>(entity =>
            {
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
            });

            modelBuilder.Entity<Material>(entity =>
            {
                entity.ToTable(t =>
                {
                    t.HasCheckConstraint(
                        "CK_Material_FileSize",
                        "[FileSize] > 0");
                });
                entity.Property(m => m.FileSize).HasColumnType("bigint");
            });
        }
    }
}
