using System.ComponentModel.DataAnnotations;

namespace StudentManagmentSystem.Dto
{
    public class CreateMaterialDto
    {
        [Required, MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int SemesterSubjectId { get; set; }

        [Required]
        public int UploadedBy { get; set; }

        [Required, MaxLength(50)]
        public string MaterialType { get; set; } = string.Empty;

        [MaxLength(255)]
        public string FileName { get; set; } = string.Empty;

        [MaxLength(500)]
        public string FileUrl { get; set; } = string.Empty;

        [Required]
        public long FileSize { get; set; }
    }

    public class UpdateMaterialDto
    {
        [Required, MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int SemesterSubjectId { get; set; }

        [Required]
        public int UploadedBy { get; set; }

        [Required, MaxLength(50)]
        public string MaterialType { get; set; } = string.Empty;

        [MaxLength(255)]
        public string FileName { get; set; } = string.Empty;

        [MaxLength(500)]
        public string FileUrl { get; set; } = string.Empty;

        [Required]
        public long FileSize { get; set; }
    }

    public class MaterialResponseDto
    {
        public int MaterialId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int SemesterSubjectId { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public int UploadedBy { get; set; }
        public string UploadedByUserName { get; set; } = string.Empty;
        public string MaterialType { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string FileUrl { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
