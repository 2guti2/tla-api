using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Filters;
using TeLoArreglo.Application.Dtos.Error;
using TeLoArreglo.Application.Exceptions;

namespace TeLoArreglo.WebApi
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class ExceptionHandlingAttribute : ExceptionFilterAttribute
    {
        public readonly bool Bypass;

        public ExceptionHandlingAttribute(bool bypass = false)
        {
            Bypass = bypass;
        }

        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            var formatter = new JsonMediaTypeFormatter();

            if (actionExecutedContext.Exception is UnauthorizedAccessException)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new ObjectContent<ErrorDto>(new ErrorDto("Incorrect username or password."), formatter)
                });
            }

            if (actionExecutedContext.Exception is ModelValidationException)
            {
                var typedException = actionExecutedContext.Exception as ModelValidationException;

                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new ObjectContent<List<ValidationErrorDto>>(typedException.Errors, formatter)
                });
            }

            if (actionExecutedContext.Exception is CommonErrorException)
            {
                var typedException = actionExecutedContext.Exception as CommonErrorException;

                throw new HttpResponseException(new HttpResponseMessage(typedException.StatusCode)
                {
                    Content = new ObjectContent<ErrorDto>(typedException.Error, formatter)
                });
            }

            if (actionExecutedContext.Exception != null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new ObjectContent<ErrorDto>(new ErrorDto("Something went wrong. Please try again later."), formatter)
                });
            }
        }
    }
}