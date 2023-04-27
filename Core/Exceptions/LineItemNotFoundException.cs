using System.Runtime.Serialization;

namespace Core.Exceptions
{
    [Serializable]
    internal class LineItemNotFoundException : Exception
    {
        public LineItemNotFoundException()
        {
        }

        public LineItemNotFoundException(string? message) : base(message)
        {
        }

        public LineItemNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected LineItemNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}