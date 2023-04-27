using System.Runtime.Serialization;

namespace Core.Features.MaterialTestReports.Exceptions
{
    [Serializable]
    internal class MaterialTestReportNotFoundException : Exception
    {
        public MaterialTestReportNotFoundException()
        {
        }

        public MaterialTestReportNotFoundException(string? message) : base(message)
        {
        }

        public MaterialTestReportNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected MaterialTestReportNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}