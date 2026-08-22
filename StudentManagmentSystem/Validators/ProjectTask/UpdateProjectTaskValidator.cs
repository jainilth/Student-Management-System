using FluentValidation;
using StudentManagmentSystem.Dto;

namespace StudentManagmentSystem.Validators.ProjectTask
{
    public class UpdateProjectTaskValidator : AbstractValidator<UpdateProjectTaskDto>
    {
        public UpdateProjectTaskValidator()
        {
            RuleFor(x => x.ProjectAllocationId).GreaterThan(0).WithMessage("A valid Project Allocation ID is required.");
            RuleFor(x => x.TaskTitle).NotEmpty().WithMessage("Task title is required.")
                .MaximumLength(200).WithMessage("Task title cannot exceed 200 characters.");
            RuleFor(x => x.TaskDescription).MaximumLength(1000).WithMessage("Task description cannot exceed 1000 characters.");
            RuleFor(x => x.TaskStatus).NotEmpty().WithMessage("Task status is required.")
                .MaximumLength(50).WithMessage("Task status cannot exceed 50 characters.");
            RuleFor(x => x.AssignedScore).GreaterThanOrEqualTo(0).WithMessage("Assigned score cannot be negative.");
            RuleFor(x => x.EarnedScore)
                .GreaterThanOrEqualTo(0).WithMessage("Earned score cannot be negative.")
                .LessThanOrEqualTo(x => x.AssignedScore).WithMessage("Earned score cannot exceed assigned score.");
            RuleFor(x => x.StartDate).NotEmpty().WithMessage("Start date is required.");
            RuleFor(x => x.DueDate).NotEmpty().WithMessage("Due date is required.")
                .GreaterThanOrEqualTo(x => x.StartDate).WithMessage("Due date must be on or after start date.");
            RuleFor(x => x.CompletedDate)
                .GreaterThanOrEqualTo(x => x.StartDate).When(x => x.CompletedDate.HasValue)
                .WithMessage("Completed date must be on or after start date.");
            RuleFor(x => x.FacultyRemarks).MaximumLength(1000).WithMessage("Faculty remarks cannot exceed 1000 characters.");
            RuleFor(x => x.StudentRemarks).MaximumLength(1000).WithMessage("Student remarks cannot exceed 1000 characters.");
        }
    }
}
