using FluentValidation;
using StudentManagmentSystem.Dto;

namespace StudentManagmentSystem.Validators.Grade
{
    public class CreateGradeValidator : AbstractValidator<CreateGradeDto>
    {
        public CreateGradeValidator()
        {
            RuleFor(x => x.GradeCode)
                .NotEmpty().WithMessage("Grade code is required.");

            RuleFor(x => x.GradeName)
                .NotEmpty().WithMessage("Grade name is required.");

            RuleFor(x => x.GradePoint)
                .GreaterThanOrEqualTo(0).WithMessage("Grade point must be 0 or greater.");

            RuleFor(x => x.MinMarks)
                .GreaterThanOrEqualTo(0).WithMessage("Minimum marks must be 0 or greater.");

            RuleFor(x => x.MaxMarks)
                .GreaterThan(0).WithMessage("Maximum marks must be greater than 0.")
                .GreaterThan(x => x.MinMarks).WithMessage("Maximum marks must be greater than minimum marks.");
        }
    }
}
