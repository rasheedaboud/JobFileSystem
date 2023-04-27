using Ardalis.GuardClauses;
using Core.Utils;
using Microsoft.AspNetCore.StaticFiles;

namespace Core.Entities
{
    public class Attachment 
    {
        public Attachment() {  }
        /// <summary>
        /// Create a file attachment. Path is used to simulate folder structure in blob storage
        /// </summary>
        /// <param name="fileName">File name with extention.</param>
        /// <param name="path">Path where attachment should live in blob storage.</param>
        public Attachment(string fileName, string path)
        {

            Guard.Against.NullOrWhiteSpace(fileName, nameof(fileName));

            var nameWithoutExtention = Path.GetFileNameWithoutExtension(fileName).SanitizeAllSpecials(); 
                    
            var fileExtention = Path.GetExtension(fileName);
            var mimeType = new FileExtensionContentTypeProvider().TryGetContentType(fileName, out var _mimeType) ? _mimeType : "application/octet-stream";

            FileName = $"{nameWithoutExtention}{fileExtention}";
            FileExtention = fileExtention;
            ContentType = mimeType;
            BlobPath = path;
        }
        public string Id { get; init; } = Guid.NewGuid().ToString();
        public string FileName { get; init; }
        public string FileExtention { get; init; }
        public string ContentType { get; init; }
        public string BlobPath { get; init; }
    }
}
