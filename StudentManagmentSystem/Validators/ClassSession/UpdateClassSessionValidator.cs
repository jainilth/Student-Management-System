using FluentValidation;
using StudentManagmentSystem.Dto;

namespace StudentManagmentSystem.Validators.ClassSession
{
    public class UpdateClassSessionValidator : AbstractValidator<UpdateClassSessionDto>
    {
        public UpdateClassSessionValidator()
        {
            RuleFor(x => x.SemesterSubjectId).GreaterThan(0).WithMessage("A valid Semester Subject ID is required.");
            RuleFor(x => x.FacultyId).GreaterThan(0).WithMessage("A valid Faculty ID is required.");
            RuleFor(x => x.SessionDate).NotEmpty().WithMessage("Session date is required.");
            RuleFor(x => x.StartTime).NotEmpty().WithMessage("Start time is required.");
            RuleFor(x => x.EndTime)
                .NotEmpty().WithMessage("End time is required.")
                .GreaterThan(x => x.StartTime).WithMessage("End time must be after start time.");
            RuleFor(x => x.Topic)
                .MaximumLength(500).When(x => x.Topic != null).WithMessage("Topic cannot exceed 500 characters.");
        }
    }
}
