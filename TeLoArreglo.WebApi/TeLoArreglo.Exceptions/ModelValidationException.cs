using System;
using System.Collections.Generic;

namespace TeLoArreglo.Exceptions
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
