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

                entity.HasMany(s => s.FacultySubjects)
                    .WithOne(fs => fs.Semester)
                    .HasForeignKey(fs => fs.SemesterId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(s => s.Attendances)
                    .WithOne(a => a.Semester)
                    .HasForeignKey(a => a.SemesterId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(s => s.Projects)
                    .WithOne(p => p.Semester)
                    .HasForeignKey(p => p.SemesterId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(s => s.Materials)
                    .WithOne(m => m.Semester)
                    .HasForeignKey(m => m.SemesterId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasOne(s => s.User)
                    .WithOne(u => u.Student)
                    .HasForeignKey<Student>(s => s.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(s => s.StudentSemesters)
                    .WithOne(ss => ss.Student)
                    .HasForeignKey(ss => ss.StudentId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(s => s.Attendances)
                    .WithOne(a => a.Student)
                    .HasForeignKey(a => a.StudentId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(s => s.AttendanceRecords)
                    .WithOne(ar => ar.Student)
                    .HasForeignKey(ar => ar.StudentId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(s => s.ProjectAllocations)
                    .WithOne(pa => pa.Student)
                    .HasForeignKey(pa => pa.StudentId)
                    .OnDelete(DeleteBehavior.Cascade);
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
                entity.HasMany(sr => sr.SubjectResults)
                    .WithOne(su => su.SemesterResult)
                    .HasForeignKey(su => su.SemesterResultId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<SubjectResult>(entity =>
            {
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

                entity.HasMany(f => f.AttendanceRecords)
                    .WithOne(ar => ar.Faculty)
                    .HasForeignKey(ar => ar.FacultyId)
                    .OnDelete(DeleteBehavior.Restrict);

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

            modelBuilder.Entity<FacultySubject>(entity =>
            {
                entity.HasOne(fs => fs.Subject)
                    .WithMany(s => s.FacultySubjects)
                    .HasForeignKey(fs => fs.SubjectId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Attendance>(entity =>
            {
                entity.HasOne(a => a.Subject)
                    .WithMany(s => s.Attendances)
                    .HasForeignKey(a => a.SubjectId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<AttendanceRecord>(entity =>
            {
                entity.HasOne(ar => ar.Subject)
                    .WithMany(s => s.AttendanceRecords)
                    .HasForeignKey(ar => ar.SubjectId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasMany(p => p.ProjectAllocations)
                    .WithOne(pa => pa.Project)
                    .HasForeignKey(pa => pa.ProjectId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ProjectAllocation>(entity =>
            {
                entity.HasMany(pa => pa.Tasks)
                    .WithOne(t => t.ProjectAllocation)
                    .HasForeignKey(t => t.ProjectAllocationId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Material>(entity =>
            {
                entity.Property(m => m.FileSize).HasColumnType("bigint");
            });
        }
    }
}
