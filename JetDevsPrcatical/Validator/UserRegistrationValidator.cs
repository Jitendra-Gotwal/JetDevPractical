using FluentValidation;
using JetDevsPrcatical.Data.Request;
using JetDevsPrcatical.Service.Abstract;

namespace JetDevsPrcatical.Validator
{
    public class UserRegistrationValidator : AbstractValidator<UserRegisterRequest>
    {
        private readonly IUserService _userService;
        public UserRegistrationValidator(IUserService userMasterService)
        {
            _userService = userMasterService;

            RuleFor(user => user.FirstName)
          .NotEmpty().WithMessage("FirstName is required.")
          .MaximumLength(50).WithMessage("FirstName must not exceed 50 characters.");

            RuleFor(user => user.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.").
                Must((o, user) => { return IsEmailExist(o.Email); }).WithMessage("Email already exists.");

            RuleFor(user => user.FirstName)
         .NotEmpty().WithMessage("FirstName is required.")
         .MaximumLength(50).WithMessage("FirstName must not exceed 50 characters.");

            RuleFor(user => user.Password)
         .NotEmpty().WithMessage("Password is required.")
         .MaximumLength(50).WithMessage("Password must not exceed 50 characters.");
        }

        private bool IsEmailExist(string email)
        {
            var isEmailExist = _userService.IsEmailExist(email).Result;
            if (isEmailExist)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
