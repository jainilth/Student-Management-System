using FluentValidation;
using StudentManagmentSystem.Dto;

namespace StudentManagmentSystem.Validators.AttendanceRecord
{
    public class UpdateAttendanceRecordValidator : AbstractValidator<UpdateAttendanceRecordDto>
    {
        public UpdateAttendanceRecordValidator()
        {
            RuleFor(x => x.SessionId).GreaterThan(0).WithMessage("A valid Session ID is required.");
            RuleFor(x => x.StudentSemesterId).GreaterThan(0).WithMessage("A valid Student Semester ID is required.");
            RuleFor(x => x.Status).NotEmpty().WithMessage("Status is required.")
                .MaximumLength(20).WithMessage("Status cannot exceed 20 characters.");
            RuleFor(x => x.Remarks).MaximumLength(500).WithMessage("Remarks cannot exceed 500 characters.");
        }
    }
}
