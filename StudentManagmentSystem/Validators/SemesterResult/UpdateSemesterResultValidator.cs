using FluentValidation;
using StudentManagmentSystem.Dto;

namespace StudentManagmentSystem.Validators.SemesterResult
{
    public class UpdateSemesterResultValidator : AbstractValidator<UpdateSemesterResultDto>
    {
        public UpdateSemesterResultValidator()
        {
            RuleFor(x => x.StudentSemesterId).GreaterThan(0).WithMessage("A valid Student Semester ID is required.");
            RuleFor(x => x.SGPA).InclusiveBetween(0, 10).WithMessage("SGPA must be between 0 and 10.");
            RuleFor(x => x.TotalCredits).GreaterThan(0).WithMessage("Total credits must be greater than 0.");
            RuleFor(x => x.EarnedCredits)
                .GreaterThanOrEqualTo(0).WithMessage("Earned credits cannot be negative.")
                .LessThanOrEqualTo(x => x.TotalCredits).WithMessage("Earned credits cannot exceed total credits.");
            RuleFor(x => x.ResultStatus).NotEmpty().WithMessage("Result status is required.")
                .MaximumLength(50).WithMessage("Result status cannot exceed 50 characters.");
        }
    }
}
