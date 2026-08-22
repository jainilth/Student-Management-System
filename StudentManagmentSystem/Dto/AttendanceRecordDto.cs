using System.ComponentModel.DataAnnotations;

namespace StudentManagmentSystem.Dto
{
    public class CreateAttendanceRecordDto
    {
        [Required]
        public int SessionId { get; set; }

        [Required]
        public int StudentSemesterId { get; set; }

        [Required, MaxLength(20)]
        public string Status { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Remarks { get; set; } = string.Empty;
    }

    public class UpdateAttendanceRecordDto
    {
        [Required]
        public int SessionId { get; set; }

        [Required]
        public int StudentSemesterId { get; set; }

        [Required, MaxLength(20)]
        public string Status { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Remarks { get; set; } = string.Empty;
    }

    public class AttendanceRecordResponseDto
    {
        public int AttendanceRecordId { get; set; }
        public int SessionId { get; set; }
        public string Topic { get; set; } = string.Empty;
        public int StudentSemesterId { get; set; }
        public string StudentEnrollmentNumber { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Remarks { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
