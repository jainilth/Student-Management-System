using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StudentManagmentSystem.Models
{
    [Index(nameof(UserName), IsUnique = true)]
    [Index(nameof(Email), IsUnique = true)]
    [Index(nameof(RoleId))]
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required, MaxLength(100)]
        public string UserName { get; set; } = string.Empty;

        [Required, MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string PasswordHash { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Address { get; set; }

        public DateTime? Dob { get; set; }

        [MaxLength(15)]
        public string Mobilenumber { get; set; }

        [MaxLength(500)]
        public string ProfilePhoto { get; set; }

        public bool IsActivate { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public int RoleId { get; set; }

        [ForeignKey(nameof(RoleId))]
        public Role? Role { get; set; }

        public Student? Student { get; set; }
        public Faculty? Faculty { get; set; }
        public ICollection<Material> UploadedMaterials { get; set; } = new List<Material>();
    }
}
