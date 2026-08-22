using FluentValidation;
using StudentManagmentSystem.Dto;

namespace StudentManagmentSystem.Validators.SubjectResult
{
    public class CreateSubjectResultValidator : AbstractValidator<CreateSubjectResultDto>
    {
        public CreateSubjectResultValidator()
        {
            RuleFor(x => x.SemesterResultId).GreaterThan(0).WithMessage("A valid Semester Result ID is required.");
            RuleFor(x => x.SemesterSubjectId).GreaterThan(0).WithMessage("A valid Semester Subject ID is required.");
            RuleFor(x => x.InternalMarks).GreaterThanOrEqualTo(0).WithMessage("Internal marks cannot be negative.");
            RuleFor(x => x.ExternalMarks).GreaterThanOrEqualTo(0).WithMessage("External marks cannot be negative.");
            RuleFor(x => x.PracticalMarks).GreaterThanOrEqualTo(0).WithMessage("Practical marks cannot be negative.");
            RuleFor(x => x.TotalMarks).GreaterThanOrEqualTo(0).WithMessage("Total marks cannot be negative.");
            RuleFor(x => x.GradeId).GreaterThan(0).WithMessage("A valid Grade ID is required.");
            RuleFor(x => x.CreditsEarned).GreaterThanOrEqualTo(0).WithMessage("Credits earned cannot be negative.");
            RuleFor(x => x.ResultStatus).NotEmpty().WithMessage("Result status is required.")
                .MaximumLength(50).WithMessage("Result status cannot exceed 50 characters.");
        }
    }
}
