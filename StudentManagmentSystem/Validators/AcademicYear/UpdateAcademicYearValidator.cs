using FluentValidation;
using StudentManagmentSystem.Dto;

namespace StudentManagmentSystem.Validators.AcademicYear
{
    public class UpdateAcademicYearValidator : AbstractValidator<UpdateAcademicYearDto>
    {
        public UpdateAcademicYearValidator()
        {
            RuleFor(x => x.Year)
                .NotEmpty().WithMessage("Academic year is required.")
                .MaximumLength(20).WithMessage("Academic year cannot exceed 20 characters.")
                .Matches(@"^\d{4}-\d{2,4}$").WithMessage("Academic year must be in format YYYY-YY or YYYY-YYYY (e.g. 2024-25).");
        }
    }
}
