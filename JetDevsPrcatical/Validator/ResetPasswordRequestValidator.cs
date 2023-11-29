using FluentValidation;
using JetDevsPrcatical.Data.Request;

namespace JetDevsPrcatical.Validator
{
    public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
    {       
        public ResetPasswordRequestValidator()
        {

            RuleFor(user => user.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");
               

            RuleFor(user => user.OldPassword)
         .NotEmpty().WithMessage("FirstName is required.")
         .MaximumLength(50).WithMessage("FirstName must not exceed 50 characters.");

            RuleFor(user => user.NewPassword)
         .NotEmpty().WithMessage("Password is required.")
         .MaximumLength(50).WithMessage("Password must not exceed 50 characters.");
        }      
    }
}
