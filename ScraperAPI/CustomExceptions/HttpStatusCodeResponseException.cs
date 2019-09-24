using ScraperAPI.ErrorModels;
using System;
using System.Net;

namespace ScraperAPI.CustomExceptions
{
    public class HttpStatusCodeResponseException : Exception
    {
        public InnerError InnerError { get; }
        public string CustomMessage { get; }
        public HttpStatusCode HttpStatusCode { get; }

        public HttpStatusCodeResponseException(HttpStatusCode statusCode)
        {
            HttpStatusCode = statusCode;
        }

        public HttpStatusCodeResponseException(HttpStatusCode statusCode, string customMessage)
        {
            HttpStatusCode = statusCode;
            CustomMessage = customMessage;
        }

        public HttpStatusCodeResponseException(HttpStatusCode statusCode, InnerError customError)
        {
            HttpStatusCode = statusCode;
            InnerError = customError;
        }

        public HttpStatusCodeResponseException(HttpStatusCode statusCode, InnerError customError, string customMessage = null)
        {
            HttpStatusCode = statusCode;
            InnerError = customError;
            CustomMessage = customMessage;
        }
    }
}
