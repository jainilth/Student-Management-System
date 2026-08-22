using FluentValidation;
using StudentManagmentSystem.Dto;

namespace StudentManagmentSystem.Validators.Material
{
    public class CreateMaterialValidator : AbstractValidator<CreateMaterialDto>
    {
        public CreateMaterialValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(200).WithMessage("Title cannot exceed 200 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.");

            RuleFor(x => x.SemesterSubjectId)
                .GreaterThan(0).WithMessage("A valid Semester Subject ID is required.");

            RuleFor(x => x.UploadedBy)
                .GreaterThan(0).WithMessage("A valid uploader User ID is required.");

            RuleFor(x => x.MaterialType)
                .NotEmpty().WithMessage("Material type is required.")
                .MaximumLength(50).WithMessage("Material type cannot exceed 50 characters.");

            RuleFor(x => x.FileName)
                .MaximumLength(255).WithMessage("File name cannot exceed 255 characters.");

            RuleFor(x => x.FileUrl)
                .MaximumLength(500).WithMessage("File URL cannot exceed 500 characters.");

            RuleFor(x => x.FileSize)
                .GreaterThan(0).WithMessage("File size must be greater than 0.");
        }
    }
}
