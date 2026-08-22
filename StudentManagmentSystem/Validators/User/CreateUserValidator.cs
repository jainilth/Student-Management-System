using FluentValidation;
using StudentManagmentSystem.Dto;

namespace StudentManagmentSystem.Validators.User
{
    public class CreateUserValidator:AbstractValidator<CreateUserDto>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username is required.")
                .MaximumLength(100).WithMessage("Username cannot exceed 100 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email address is required.")
                .MaximumLength(150).WithMessage("Email cannot exceed 150 characters.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MaximumLength(255).WithMessage("Password cannot exceed 255 characters.");

            RuleFor(x => x.Address)
                .MaximumLength(500).WithMessage("Address cannot exceed 500 characters.");

            RuleFor(x => x.Mobilenumber)
                .MaximumLength(15).WithMessage("Mobile number cannot exceed 15 characters.");

            RuleFor(x => x.ProfilePhoto)
                .MaximumLength(500).WithMessage("Profile photo URL cannot exceed 500 characters.");

            RuleFor(x => x.RoleId)
                .GreaterThan(0).WithMessage("A valid Role ID is required.");
        }
    }
}
