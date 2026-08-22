using FluentValidation;
using StudentManagmentSystem.Dto;

namespace StudentManagmentSystem.Validators.Student
{
    public class UpdateStudentValidator : AbstractValidator<UpdateStudentDto>
    {
        public UpdateStudentValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("A valid User ID is required.");

            RuleFor(x => x.EnrollmentNumber)
                .NotEmpty().WithMessage("Enrollment number is required.")
                .MaximumLength(50).WithMessage("Enrollment number cannot exceed 50 characters.");

            RuleFor(x => x.AdmissionYear)
                .GreaterThan(2000).WithMessage("Admission year must be after 2000.")
                .LessThanOrEqualTo(DateTime.UtcNow.Year + 1).WithMessage("Admission year cannot be in the future.");

            RuleFor(x => x.ProgramId)
                .GreaterThan(0).WithMessage("A valid Program ID is required.");

            RuleFor(x => x.CurrentSemesterId)
                .GreaterThan(0).WithMessage("A valid Semester ID is required.");
        }
    }
}
