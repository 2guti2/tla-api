using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Abp.Domain.Entities;
using FluentValidation.Results;
using TeLoArreglo.Logic.Common;

namespace TeLoArreglo.Logic.Entities
{
    public class Validable : Entity
    {
        [NotMapped]
        protected ValidationResult _validationResult;

        public List<ValidationError> GetValidationErrors()
        {
            List<ValidationFailure> errorsInValidatorDatatype = _validationResult.Errors.ToList();

            var errors = new List<ValidationError>();

            errorsInValidatorDatatype.ForEach(eivd => errors.Add(new ValidationError(eivd.PropertyName, eivd.ErrorMessage)));

            return errors;
        }
    }
}
