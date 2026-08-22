using FluentValidation;
using StudentManagmentSystem.Dto;

namespace StudentManagmentSystem.Validators.Semester
{
    public class CreateSemesterValidator : AbstractValidator<CreateSemesterDto>
    {
        public CreateSemesterValidator()
        {
            RuleFor(x => x.SemesterNumber)
                .GreaterThan(0).WithMessage("Semester number must be greater than 0.");

            RuleFor(x => x.SemesterName)
                .NotEmpty().WithMessage("Semester name is required.")
                .MaximumLength(100).WithMessage("Semester name cannot exceed 100 characters.");
        }
    }
}
