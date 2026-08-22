using FluentValidation;
using StudentManagmentSystem.Dto;

namespace StudentManagmentSystem.Validators.Subject
{
    public class UpdateSubjectValidator : AbstractValidator<UpdateSubjectDto>
    {
        public UpdateSubjectValidator()
        {
            RuleFor(x => x.SubjectCode)
                .NotEmpty().WithMessage("Subject code is required.")
                .MaximumLength(50).WithMessage("Subject code cannot exceed 50 characters.");

            RuleFor(x => x.SubjectName)
                .NotEmpty().WithMessage("Subject name is required.")
                .MaximumLength(150).WithMessage("Subject name cannot exceed 150 characters.");

            RuleFor(x => x.SubjectType)
                .NotEmpty().WithMessage("Subject type is required.")
                .MaximumLength(50).WithMessage("Subject type cannot exceed 50 characters.");
        }
    }
}
