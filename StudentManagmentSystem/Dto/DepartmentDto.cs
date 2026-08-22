using System.ComponentModel.DataAnnotations;

namespace StudentManagmentSystem.Dto
{
    public class CreateDepartmentDto
    {
        [Required, MaxLength(150)]
        public string DepartmentName { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string DepartmentCode { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
    }

    public class UpdateDepartmentDto
    {
        [Required, MaxLength(150)]
        public string DepartmentName { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string DepartmentCode { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;

        public bool IsActive { get; set; }
    }

    public class DepartmentResponseDto
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public string DepartmentCode { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
