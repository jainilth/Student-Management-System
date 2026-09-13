using FluentValidation;
using StudentManagmentSystem.Dto;

namespace StudentManagmentSystem.Validators.SubjectResult
{
    public class UpdateSubjectResultValidator : AbstractValidator<UpdateSubjectResultDto>
    {
        public UpdateSubjectResultValidator()
        {
            RuleFor(x => x.StudentSemesterId).GreaterThan(0).WithMessage("A valid Student Semester ID is required.");
            RuleFor(x => x.SemesterSubjectId).GreaterThan(0).WithMessage("A valid Semester Subject ID is required.");
            RuleFor(x => x.InternalMarks).InclusiveBetween(0, 30).WithMessage("Internal marks must be in the range of 0 to 30.");
            RuleFor(x => x.ExternalMarks).InclusiveBetween(0, 50).WithMessage("External marks must be in the range of 0 to 50.");
            RuleFor(x => x.PracticalMarks).InclusiveBetween(0, 20).WithMessage("Practical marks must be in the range of 0 to 20.");
        }
    }
}
