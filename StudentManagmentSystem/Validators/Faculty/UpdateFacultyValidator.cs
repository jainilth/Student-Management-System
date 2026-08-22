using FluentValidation;
using StudentManagmentSystem.Dto;

namespace StudentManagmentSystem.Validators.Faculty
{
    public class UpdateFacultyValidator : AbstractValidator<UpdateFacultyDto>
    {
        public UpdateFacultyValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("A valid User ID is required.");

            RuleFor(x => x.EmployeeNumber)
                .NotEmpty().WithMessage("Employee number is required.")
                .MaximumLength(50).WithMessage("Employee number cannot exceed 50 characters.");

            RuleFor(x => x.DepartmentId)
                .GreaterThan(0).WithMessage("A valid Department ID is required.");

            RuleFor(x => x.Designation)
                .MaximumLength(100).WithMessage("Designation cannot exceed 100 characters.");

            RuleFor(x => x.JoiningDate)
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Joining date cannot be in the future.");
        }
    }
}
