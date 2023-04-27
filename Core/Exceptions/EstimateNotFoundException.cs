using System.Runtime.Serialization;

namespace Core.Features.Estimates.Exceptions
{
    [Serializable]
    internal class EstimateNotFoundException : Exception
    {
        public EstimateNotFoundException()
        {
        }

        public EstimateNotFoundException(string? message) : base(message)
        {
        }

        public EstimateNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected EstimateNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}