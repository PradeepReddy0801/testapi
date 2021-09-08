using System;
using System.Runtime.Serialization;

namespace iHubAdminAPI.Controllers
{
    [Serializable]
    internal class DataFormatException : Exception
    {
        public DataFormatException()
        {
        }

        public DataFormatException(string message) : base(message)
        {
        }

        public DataFormatException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DataFormatException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}