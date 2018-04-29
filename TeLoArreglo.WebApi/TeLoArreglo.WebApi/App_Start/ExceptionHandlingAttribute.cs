using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;
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
            if (actionExecutedContext.Exception is NotLoggedInException)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized)
                {
                    Content = new StringContent("Not Logged In.")
                });
            }

            if (actionExecutedContext.Exception is ForbiddenAccessException)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden)
                {
                    Content = new StringContent("You don't have the sufficient privileges to perform this action.")
                });
            }

            if (actionExecutedContext.Exception is UnauthorizedAccessException)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("Incorrect username or password.")
                });
            }

            if (actionExecutedContext.Exception is InvalidRequestException)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Api key not found.")
                });
            }

            if (actionExecutedContext.Exception != null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("Something went wrong. Please try again later.")
                });
            }
        }
    }
}