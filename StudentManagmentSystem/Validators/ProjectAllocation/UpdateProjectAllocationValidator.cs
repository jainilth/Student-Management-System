using FluentValidation;
using StudentManagmentSystem.Dto;

namespace StudentManagmentSystem.Validators.ProjectAllocation
{
    public class UpdateProjectAllocationValidator : AbstractValidator<UpdateProjectAllocationDto>
    {
        public UpdateProjectAllocationValidator()
        {
            RuleFor(x => x.ProjectId).GreaterThan(0).WithMessage("A valid Project ID is required.");
            RuleFor(x => x.StudentId).GreaterThan(0).WithMessage("A valid Student ID is required.");
            RuleFor(x => x.FacultyId).GreaterThan(0).WithMessage("A valid Faculty ID is required.");
            RuleFor(x => x.FinalScore).InclusiveBetween(0, 100).WithMessage("Final score must be between 0 and 100.");
            RuleFor(x => x.Grade).NotEmpty().WithMessage("Grade is required.")
                .MaximumLength(10).WithMessage("Grade cannot exceed 10 characters.");
            RuleFor(x => x.Status).NotEmpty().WithMessage("Status is required.")
                .MaximumLength(50).WithMessage("Status cannot exceed 50 characters.");
            RuleFor(x => x.RepositoryUrl).MaximumLength(500).WithMessage("Repository URL cannot exceed 500 characters.");
        }
    }
}
