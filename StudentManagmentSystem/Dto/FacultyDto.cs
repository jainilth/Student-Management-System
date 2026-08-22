using System.ComponentModel.DataAnnotations;

namespace StudentManagmentSystem.Dto
{
    public class CreateFacultyDto
    {
        [Required]
        public int UserId { get; set; }

        [Required, MaxLength(50)]
        public string EmployeeNumber { get; set; } = string.Empty;

        [Required]
        public int DepartmentId { get; set; }

        [MaxLength(100)]
        public string Designation { get; set; } = string.Empty;

        [Required]
        public DateTime JoiningDate { get; set; }
    }

    public class UpdateFacultyDto
    {
        [Required]
        public int UserId { get; set; }

        [Required, MaxLength(50)]
        public string EmployeeNumber { get; set; } = string.Empty;

        [Required]
        public int DepartmentId { get; set; }

        [MaxLength(100)]
        public string Designation { get; set; } = string.Empty;

        [Required]
        public DateTime JoiningDate { get; set; }
    }

    public class FacultyResponseDto
    {
        public int FacultyId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string EmployeeNumber { get; set; } = string.Empty;
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public string Designation { get; set; } = string.Empty;
        public DateTime JoiningDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
