﻿using System;
using System.Net;

namespace TeLoArreglo.Exceptions
{
    public class CommonErrorException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public ErrorDto Error { get; set; }

        public CommonErrorException(HttpStatusCode statusCode, string customMessage)
        {
            StatusCode = statusCode;
            Error = new ErrorDto(customMessage);
        }

        public CommonErrorException()
        {
            
        }
    }
}
