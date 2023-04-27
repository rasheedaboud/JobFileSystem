using System.Runtime.Serialization;

namespace Core.Exceptions
{
    [Serializable]
    internal class AttachmentNotFoundException : Exception
    {
        public AttachmentNotFoundException()
        {
        }

        public AttachmentNotFoundException(string? message) : base(message)
        {
        }

        public AttachmentNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected AttachmentNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}