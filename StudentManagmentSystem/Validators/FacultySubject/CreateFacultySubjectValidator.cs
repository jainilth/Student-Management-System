using FluentValidation;
using StudentManagmentSystem.Dto;

namespace StudentManagmentSystem.Validators.FacultySubject
{
    public class CreateFacultySubjectValidator : AbstractValidator<CreateFacultySubjectDto>
    {
        public CreateFacultySubjectValidator()
        {
            RuleFor(x => x.FacultyId).GreaterThan(0).WithMessage("A valid Faculty ID is required.");
            RuleFor(x => x.SemesterSubjectId).GreaterThan(0).WithMessage("A valid Semester Subject ID is required.");
            RuleFor(x => x.AcademicYearId).GreaterThan(0).WithMessage("A valid Academic Year ID is required.");
        }
    }
}
