using System;
using System.Runtime.Serialization;

namespace CleanIoc
{
    [Serializable]
    public class BadConfigurationException : Exception
    {
        public BadConfigurationException()
        {
        }

        public BadConfigurationException(string message)
            : base(message)
        {
        }

        public BadConfigurationException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected BadConfigurationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
