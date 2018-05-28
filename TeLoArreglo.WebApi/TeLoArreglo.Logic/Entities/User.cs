using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using Abp.Domain.Entities;
using FluentValidation.Results;
using TeLoArreglo.Logic.Common;
using TeLoArreglo.Logic.Validators;

namespace TeLoArreglo.Logic.Entities
{
    public class User : Entity
    {
        [StringLength(450)]
        [Index(IsUnique = true)]
        public string Username { get; set; }

        public string Password { get; set; }
        public bool IsBlocked { get; set; } = false;
        public List<DamageReport> DamageReports { get; set; } = new List<DamageReport>();

        public virtual List<Action> PermittedActions => new List<Action>{ Action.ReportDamage, Action.QueryDamages };

        public static Expression<Func<User, bool>> EqualityExpression(User toCompare)
        {
            return u => u.Username.Equals(toCompare.Username) && u.Password.Equals(toCompare.Password);
        }

        [NotMapped]
        private FluentValidation.Results.ValidationResult _validationResult;

        public bool IsValid()
        {
            var validator = new UserValidator();
            _validationResult = validator.Validate(this);

            return _validationResult.IsValid;
        }

        public List<ValidationError> GetValidationErrors()
        {
            List<ValidationFailure> errorsInValidatorDatatype = _validationResult.Errors.ToList();

            var errors = new List<ValidationError>();

            errorsInValidatorDatatype.ForEach(eivd => errors.Add(new ValidationError(eivd.PropertyName, eivd.ErrorMessage)));

            return errors;
        }
    }
}
