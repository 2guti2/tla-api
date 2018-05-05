using FluentValidation;
using TeLoArreglo.Logic.Entities;

namespace TeLoArreglo.Logic.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(user => user.Username).NotEmpty().WithMessage("Username cannot be empty");
            RuleFor(user => user.Password).NotEmpty().WithMessage("Password cannot be empty");
        }
    }
}
