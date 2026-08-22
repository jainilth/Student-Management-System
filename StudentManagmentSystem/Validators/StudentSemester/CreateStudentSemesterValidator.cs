using FluentValidation;
using StudentManagmentSystem.Dto;

namespace StudentManagmentSystem.Validators.StudentSemester
{
    public class CreateStudentSemesterValidator : AbstractValidator<CreateStudentSemesterDto>
    {
        public CreateStudentSemesterValidator()
        {
            RuleFor(x => x.StudentId).GreaterThan(0).WithMessage("A valid Student ID is required.");
            RuleFor(x => x.SemesterId).GreaterThan(0).WithMessage("A valid Semester ID is required.");
            RuleFor(x => x.AcademicYearId).GreaterThan(0).WithMessage("A valid Academic Year ID is required.");
            RuleFor(x => x.EnrollmentDate).NotEmpty().WithMessage("Enrollment date is required.");
            RuleFor(x => x.Status).NotEmpty().WithMessage("Status is required.")
                .MaximumLength(50).WithMessage("Status cannot exceed 50 characters.");
        }
    }
}
