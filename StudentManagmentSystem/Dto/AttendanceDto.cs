using System.ComponentModel.DataAnnotations;

namespace StudentManagmentSystem.Dto
{
    public class CreateAttendanceDto
    {
        [Required]
        public int StudentSemesterId { get; set; }

        [Required]
        public int SemesterSubjectId { get; set; }

        [Required]
        public int ClassesHeld { get; set; }

        [Required]
        public int ClassesAttended { get; set; }
    }

    public class UpdateAttendanceDto
    {
        [Required]
        public int StudentSemesterId { get; set; }

        [Required]
        public int SemesterSubjectId { get; set; }

        [Required]
        public int ClassesHeld { get; set; }

        [Required]
        public int ClassesAttended { get; set; }
    }

    public class AttendanceResponseDto
    {
        public int AttendanceId { get; set; }
        public int StudentSemesterId { get; set; }
        public string StudentEnrollmentNumber { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public int SemesterSubjectId { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public int ClassesHeld { get; set; }
        public int ClassesAttended { get; set; }
        public decimal AttendancePercentage { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
