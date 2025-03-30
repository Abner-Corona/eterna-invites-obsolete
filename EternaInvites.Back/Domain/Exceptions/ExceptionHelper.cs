using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class ExceptionHelper : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public ExceptionHelper(string message) : base(message)
        {

        }
        public ExceptionHelper(string message, Exception innerException) : base(message, innerException)
        {
        }
        public ExceptionHelper(string message, HttpStatusCode statusCode, Exception innerException) : base(message, innerException)
        {
            StatusCode = statusCode;
        }
        public ExceptionHelper(string message, HttpStatusCode statusCode) : base(message)
        {
            StatusCode = statusCode;
        }

    }
}