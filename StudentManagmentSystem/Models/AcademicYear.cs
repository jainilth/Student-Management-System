using System.ComponentModel.DataAnnotations;

namespace StudentManagmentSystem.Models
{
    public class AcademicYear
    {
        [Key]
        public int AcademicYearId { get; set; }

        [Required]
        public string Year {  get; set; }

        public ICollection<StudentSemester> StudentSemesters { get; set; }
        = new List<StudentSemester>();

        public ICollection<FacultySubject> FacultySubjects { get; set; }
            = new List<FacultySubject>();
    }
}
