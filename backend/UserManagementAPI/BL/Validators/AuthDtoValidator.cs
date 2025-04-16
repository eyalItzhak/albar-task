using FluentValidation;
using UserManagementAPI.DAL.Dtos.Auth;

namespace UserManagementAPI.BL.Validators
{
    public class AuthDtoValidator : AbstractValidator<SignInUpRequestDto>
    {

        public AuthDtoValidator()
        {
            RuleFor(x => x.Username)
                 .NotEmpty().WithMessage("Username name is required.")
                 .MaximumLength(100).WithMessage("Product name cannot exceed 100 characters.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
        }


    }
}
