using FluentValidation;
using StudentManagmentSystem.Dto;

namespace StudentManagmentSystem.Validators.Attendance
{
    public class UpdateAttendanceValidator : AbstractValidator<UpdateAttendanceDto>
    {
        public UpdateAttendanceValidator()
        {
            RuleFor(x => x.StudentSemesterId).GreaterThan(0).WithMessage("A valid Student Semester ID is required.");
            RuleFor(x => x.SemesterSubjectId).GreaterThan(0).WithMessage("A valid Semester Subject ID is required.");
            RuleFor(x => x.ClassesHeld).GreaterThanOrEqualTo(0).WithMessage("Classes held cannot be negative.");
            RuleFor(x => x.ClassesAttended)
                .GreaterThanOrEqualTo(0).WithMessage("Classes attended cannot be negative.")
                .LessThanOrEqualTo(x => x.ClassesHeld).WithMessage("Classes attended cannot exceed classes held.");
        }
    }
}
