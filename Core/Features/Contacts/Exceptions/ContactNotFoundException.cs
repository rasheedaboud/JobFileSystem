using System.Runtime.Serialization;

namespace Core.Features.Contacts.Exceptions

{
    [Serializable]
    internal class ContactNotFoundException : Exception
    {
        public ContactNotFoundException()
        {
        }

        public ContactNotFoundException(string? message) : base(message)
        {
        }

        public ContactNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ContactNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}