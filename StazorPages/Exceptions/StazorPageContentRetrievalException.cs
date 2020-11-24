using System;
using System.Runtime.Serialization;

namespace StazorPages.Exceptions
{
    public class StazorPageContentRetrievalException : Exception
    {
        public StazorPageContentRetrievalException()
        {
        }

        public StazorPageContentRetrievalException(string message) : base(message)
        {
        }

        public StazorPageContentRetrievalException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected StazorPageContentRetrievalException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
