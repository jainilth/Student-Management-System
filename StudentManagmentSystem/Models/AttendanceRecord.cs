using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StudentManagmentSystem.Models
{
    [Index(nameof(SessionId), nameof(StudentSemesterId), IsUnique = true)]
    [Index(nameof(StudentSemesterId))]
    public class AttendanceRecord
    {
        [Key]
        public int AttendanceRecordId { get; set; }
        [ForeignKey(nameof(Session))]
        public int SessionId { get; set; }
        [ForeignKey(nameof(StudentSemester))]
        public int StudentSemesterId { get; set; }

        [Required, MaxLength(20)]
        public string Status { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Remarks { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ClassSession Session { get; set; } = null!;

        public StudentSemester StudentSemester { get; set; } = null!;
    }
}