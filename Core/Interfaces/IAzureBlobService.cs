namespace JobFileSystem.Core.Interfaces
{
    public interface IAzureBlobService
    {
        Task<Stream> DownloadAsStream(string path);
        Task<string> DownloadAsURI(string path);
        Task<bool> RemoveBlob(string path);
        Task<string> UploadStreamToAzure(string bloblFileName, Stream stream);
    }
}
