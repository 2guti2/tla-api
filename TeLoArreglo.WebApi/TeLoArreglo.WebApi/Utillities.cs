using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using TeLoArreglo.Application.Exceptions;

namespace TeLoArreglo.WebApi
{
    public class Utillities
    {
        private const string TokenHeaderKey = "x-auth-token";

        public static string GetTokenFromRequest(HttpRequestMessage request)
        {
            try
            {
                IEnumerable<string> headerValues = request.Headers.GetValues(TokenHeaderKey);
                return headerValues.FirstOrDefault();
            }
            catch (InvalidOperationException)
            {
                throw new InvalidRequestException();
            }
        }

        public static string GetAuthTokenOrNullIfException(HttpRequestMessage request)
        {
            string token = null;
            try
            {
                token = GetTokenFromRequest(request);
            }
            catch (InvalidRequestException) { }

            return token;
        }
    }
}