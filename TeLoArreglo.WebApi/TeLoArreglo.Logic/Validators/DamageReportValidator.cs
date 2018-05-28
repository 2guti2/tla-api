using FluentValidation;
using TeLoArreglo.Logic.Entities;

namespace TeLoArreglo.Logic.Validators
{
    public class DamageReportValidator : AbstractValidator<DamageReport>
    {
        public DamageReportValidator()
        {
            RuleFor(report => report.Description).NotEmpty().WithMessage("Description cannot be empty");
        }
    }
}
