using FluentValidation;
using StudentManagmentSystem.Dto;

namespace StudentManagmentSystem.Validators.AcademicProgram
{
    public class UpdateAcademicProgramValidator : AbstractValidator<UpdateAcademicProgramDto>
    {
        public UpdateAcademicProgramValidator()
        {
            RuleFor(x => x.ProgramName)
                .NotEmpty().WithMessage("Program name is required.")
                .MaximumLength(150).WithMessage("Program name cannot exceed 150 characters.");

            RuleFor(x => x.ProgramCode)
                .NotEmpty().WithMessage("Program code is required.")
                .MaximumLength(50).WithMessage("Program code cannot exceed 50 characters.");

            RuleFor(x => x.DepartmentId)
                .GreaterThan(0).WithMessage("A valid Department ID is required.");

            RuleFor(x => x.DurationYears)
                .GreaterThan(0).WithMessage("Duration years must be greater than 0.");

            RuleFor(x => x.TotalSemesters)
                .GreaterThan(0).WithMessage("Total semesters must be greater than 0.");
        }
    }
}
