using System;
using System.Collections.Generic;
using TeLoArreglo.Application.Dtos.Error;

namespace TeLoArreglo.Application.Exceptions
{
    public class ModelValidationException : Exception
    {
        public List<ValidationErrorDto> Errors { get; set; }

        public ModelValidationException()
        {
            
        }

        public ModelValidationException(List<ValidationErrorDto> errors)
        {
            Errors = errors;
        }
    }
}
