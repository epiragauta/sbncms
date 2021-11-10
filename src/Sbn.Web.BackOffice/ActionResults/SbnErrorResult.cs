using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Sbn.Cms.Web.BackOffice.ActionResults
{
    public class SbnErrorResult : ObjectResult
    {
        public SbnErrorResult(HttpStatusCode statusCode, string message) : this (statusCode, new MessageWrapper(message))
        {
        }

        public SbnErrorResult(HttpStatusCode statusCode, object value) : base(value)
        {
            StatusCode = (int)statusCode;
        }

        private class MessageWrapper
        {
            public MessageWrapper(string message)
            {
                Message = message;
            }

            public string Message { get;}
        }
    }
}
