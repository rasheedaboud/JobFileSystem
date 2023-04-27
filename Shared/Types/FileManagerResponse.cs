using System.Text.Json.Serialization;

namespace JobFileSystem.Shared.Types
{
    public interface IFile
    {
        string ContentDisposition { get; }
        string ContentType { get; }
        string FileName { get; }
        long Length { get; }
        string Name { get; }

        Task CopyToAsync(Stream target, CancellationToken cancellationToken = default);
        Stream OpenReadStream();
    }

    public class File : IFile
    {
        // Stream.CopyTo method uses 80KB as the default buffer size.
        private const int DefaultBufferSize = 80 * 1024;

        private readonly Stream _baseStream;
        private readonly long _baseStreamOffset;

        /// <summary>
        /// Initializes a new instance of <see cref="FormFile"/>.
        /// </summary>
        /// <param name="baseStream">The <see cref="Stream"/> containing the form file.</param>
        /// <param name="baseStreamOffset">The offset at which the form file begins.</param>
        /// <param name="length">The length of the form file.</param>
        /// <param name="name">The name of the form file from the <c>Content-Disposition</c> header.</param>
        /// <param name="fileName">The file name from the <c>Content-Disposition</c> header.</param>
        public File(Stream baseStream, long baseStreamOffset, long length, string name, string fileName)
        {
            _baseStream = baseStream;
            _baseStreamOffset = baseStreamOffset;
            Length = length;
            Name = name;
            FileName = fileName;
        }

        /// <summary>
        /// Gets the raw Content-Disposition header of the uploaded file.
        /// </summary>
        public string ContentDisposition { get; }

        /// <summary>
        /// Gets the raw Content-Type header of the uploaded file.
        /// </summary>
        public string ContentType { get; }


        /// <summary>
        /// Gets the file length in bytes.
        /// </summary>
        public long Length { get; }

        /// <summary>
        /// Gets the name from the Content-Disposition header.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the file name from the Content-Disposition header.
        /// </summary>
        public string FileName { get; }

        /// <summary>
        /// Opens the request stream for reading the uploaded file.
        /// </summary>
        public Stream OpenReadStream()
        {
            return new ReferenceReadStream(_baseStream, _baseStreamOffset, Length);
        }    

        /// <summary>
        /// Asynchronously copies the contents of the uploaded file to the <paramref name="target"/> stream.
        /// </summary>
        /// <param name="target">The stream to copy the file contents to.</param>
        /// <param name="cancellationToken"></param>
        public async Task CopyToAsync(Stream target, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            using (var readStream = OpenReadStream())
            {
                await readStream.CopyToAsync(target, DefaultBufferSize, cancellationToken);
            }
        }
    }
    
    public class FileManagerDirectoryContent
    {
        [JsonPropertyName("path")]
        public string Path { get; set; }

        [JsonPropertyName("action")]
        public string Action { get; set; }

        [JsonPropertyName("newName")]
        public string NewName { get; set; }

        [JsonPropertyName("names")]
        public string[] Names { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("size")]
        public long Size { get; set; }

        [JsonPropertyName("previousName")]
        public string PreviousName { get; set; }

        [JsonPropertyName("dateModified")]
        public DateTime DateModified { get; set; }

        [JsonPropertyName("dateCreated")]
        public DateTime DateCreated { get; set; }

        [JsonPropertyName("hasChild")]
        public bool HasChild { get; set; }

        [JsonPropertyName("isFile")]
        public bool IsFile { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("filterPath")]
        public string FilterPath { get; set; }

        [JsonPropertyName("filterId")]
        public string FilterId { get; set; }

        [JsonPropertyName("targetPath")]
        public string TargetPath { get; set; }

        [JsonPropertyName("renameFiles")]
        public string[] RenameFiles { get; set; }

        [JsonPropertyName("uploadFiles")]
        public IList<IFile> UploadFiles { get; set; }

        [JsonPropertyName("caseSensitive")]
        public bool CaseSensitive { get; set; }

        [JsonPropertyName("searchString")]
        public string SearchString { get; set; }

        [JsonPropertyName("showHiddenItems")]
        public bool ShowHiddenItems { get; set; }

        [JsonPropertyName("data")]
        public TargetData[] Data { get; set; }

        [JsonPropertyName("targetData")]
        public TargetData TargetData { get; set; }

        [JsonPropertyName("permission")]
        public AccessPermission Permission { get; set; }
    }
    public class TargetData
    {
        public string Path { get; set; }

        public string Action { get; set; }

        public string NewName { get; set; }

        public string[] Names { get; set; }

        public string Name { get; set; }

        public long Size { get; set; }

        public string PreviousName { get; set; }

        public DateTime DateModified { get; set; }

        public DateTime DateCreated { get; set; }

        public bool HasChild { get; set; }

        public bool IsFile { get; set; }

        public string Type { get; set; }

        public string Id { get; set; }

        public string FilterPath { get; set; }

        public string FilterId { get; set; }

        public string TargetPath { get; set; }

        public string[] RenameFiles { get; set; }

        public IList<IFile> UploadFiles { get; set; }

        public bool CaseSensitive { get; set; }

        public string SearchString { get; set; }

        public bool ShowHiddenItems { get; set; }
        public TargetData[] Data { get; set; }

        public AccessPermission Permission { get; set; }
    }
    public class FileDetails
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public bool IsFile { get; set; }
        public string Size { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public bool MultipleFiles { get; set; }
        public AccessPermission Permission { get; set; }
    }
    public class ErrorDetails
    {

        public string Code { get; set; }

        public string Message { get; set; }

        public IEnumerable<string> FileExists { get; set; }
    }
    public class AccessPermission
    {
        [JsonPropertyName("copy")]
        public bool Copy { get; set; } = true;
        [JsonPropertyName("copy")]
        public bool Download { get; set; } = true;
        [JsonPropertyName("edit")]
        public bool Edit { get; set; } = true;
        [JsonPropertyName("editContents")]
        public bool EditContents { get; set; } = true;
        [JsonPropertyName("read")]
        public bool Read { get; set; } = true;
        [JsonPropertyName("upload")]
        public bool Upload { get; set; } = true;
    }
    public class FileManagerResponse
    {
        [JsonPropertyName("CWD")]
        public FileManagerDirectoryContent CWD { get; set; }
        [JsonPropertyName("Files")]
        public IEnumerable<FileManagerDirectoryContent> Files { get; set; }

        [JsonPropertyName("error")]
        public ErrorDetails Error { get; set; }

        [JsonPropertyName("details")]
        public FileDetails Details { get; set; }

    }

    /// <summary>
    /// A Stream that wraps another stream starting at a certain offset and reading for the given length.
    /// </summary>
    internal sealed class ReferenceReadStream : Stream
    {
        private readonly Stream _inner;
        private readonly long _innerOffset;
        private readonly long _length;
        private long _position;

        private bool _disposed;

        public ReferenceReadStream(Stream inner, long offset, long length)
        {
            if (inner == null)
            {
                throw new ArgumentNullException(nameof(inner));
            }

            _inner = inner;
            _innerOffset = offset;
            _length = length;
            _inner.Position = offset;
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return _inner.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override long Length
        {
            get { return _length; }
        }

        public override long Position
        {
            get { return _position; }
            set
            {
                ThrowIfDisposed();
                if (value < 0 || value > Length)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), value, $"The Position must be within the length of the Stream: {Length}");
                }
                VerifyPosition();
                _position = value;
                _inner.Position = _innerOffset + _position;
            }
        }

        // Throws if the position in the underlying stream has changed without our knowledge, indicating someone else is trying
        // to use the stream at the same time which could lead to data corruption.
        private void VerifyPosition()
        {
            if (_inner.Position != _innerOffset + _position)
            {
                throw new InvalidOperationException("The inner stream position has changed unexpectedly.");
            }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            if (origin == SeekOrigin.Begin)
            {
                Position = offset;
            }
            else if (origin == SeekOrigin.End)
            {
                Position = Length + offset;
            }
            else // if (origin == SeekOrigin.Current)
            {
                Position = Position + offset;
            }
            return Position;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            ThrowIfDisposed();
            VerifyPosition();
            var toRead = Math.Min(count, _length - _position);
            var read = _inner.Read(buffer, offset, (int)toRead);
            _position += read;
            return read;
        }

        public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
            => ReadAsync(buffer.AsMemory(offset, count), cancellationToken).AsTask();

        public override async ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            VerifyPosition();
            var toRead = (int)Math.Min(buffer.Length, _length - _position);
            var read = await _inner.ReadAsync(buffer.Slice(0, toRead), cancellationToken);
            _position += read;
            return read;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }
        public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }

        public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken)
            => throw new NotSupportedException();

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Flush()
        {
        }

        public override Task FlushAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _disposed = true;
            }
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(ReferenceReadStream));
            }
        }
    }
}
