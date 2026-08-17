using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace StudentManagmentSystem.Models
{
    [Index(nameof(SubjectId))]
    [Index(nameof(SemesterId))]
    [Index(nameof(UploadedBy))]
    public class Material
    {
        [Key]
        public int MaterialId { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;

        public int SubjectId { get; set; }
        public int SemesterId { get; set; }
        public int UploadedBy { get; set; }

        [Required, MaxLength(50)]
        public string MaterialType { get; set; } = string.Empty;

        [MaxLength(255)]
        public string FileName { get; set; } = string.Empty;

        [MaxLength(500)]
        public string FileUrl { get; set; } = string.Empty;

        public long FileSize { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public Subject Subject { get; set; } = null!;
        public Semester Semester { get; set; } = null!;
        public User UploadedByUser { get; set; } = null!;
    }
}