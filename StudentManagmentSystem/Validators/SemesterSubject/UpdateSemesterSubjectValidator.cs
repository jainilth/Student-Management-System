using FluentValidation;
using StudentManagmentSystem.Dto;

namespace StudentManagmentSystem.Validators.SemesterSubject
{
    public class UpdateSemesterSubjectValidator : AbstractValidator<UpdateSemesterSubjectDto>
    {
        public UpdateSemesterSubjectValidator()
        {
            RuleFor(x => x.ProgramId)
                .GreaterThan(0).WithMessage("A valid Program ID is required.");

            RuleFor(x => x.SemesterId)
                .GreaterThan(0).WithMessage("A valid Semester ID is required.");

            RuleFor(x => x.SubjectId)
                .GreaterThan(0).WithMessage("A valid Subject ID is required.");

            RuleFor(x => x.Credits)
                .GreaterThan(0).WithMessage("Credits must be greater than 0.");
        }
    }
}
