using System;
using System.Runtime.Serialization;

namespace Sbn.Cms.Web.Common.Exceptions
{
    /// <summary>
    /// Exception that occurs when an Sbn form route string is invalid
    /// </summary>
    [Serializable]
    public sealed class HttpSbnFormRouteStringException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpSbnFormRouteStringException" /> class.
        /// </summary>
        public HttpSbnFormRouteStringException()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpSbnFormRouteStringException" /> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that holds the contextual information about the source or destination.</param>
        private HttpSbnFormRouteStringException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpSbnFormRouteStringException" /> class.
        /// </summary>
        /// <param name="message">The error message displayed to the client when the exception is thrown.</param>
        public HttpSbnFormRouteStringException(string message)
            : base(message)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpSbnFormRouteStringException" /> class.
        /// </summary>
        /// <param name="message">The error message displayed to the client when the exception is thrown.</param>
        /// <param name="innerException">The <see cref="P:System.Exception.InnerException" />, if any, that threw the current exception.</param>
        public HttpSbnFormRouteStringException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
