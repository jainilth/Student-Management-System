using FluentValidation;
using StudentManagmentSystem.Dto;

namespace StudentManagmentSystem.Validators.FacultySubject
{
    public class UpdateFacultySubjectValidator : AbstractValidator<UpdateFacultySubjectDto>
    {
        public UpdateFacultySubjectValidator()
        {
            RuleFor(x => x.FacultyId).GreaterThan(0).WithMessage("A valid Faculty ID is required.");
            RuleFor(x => x.SemesterSubjectId).GreaterThan(0).WithMessage("A valid Semester Subject ID is required.");
            RuleFor(x => x.AcademicYearId).GreaterThan(0).WithMessage("A valid Academic Year ID is required.");
        }
    }
}
