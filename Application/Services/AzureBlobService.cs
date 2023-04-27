using JobFileSystem.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Application.Services
{
    public class AzureBlobService : IAzureBlobService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<AzureBlobService> _logger;

        public AzureBlobService(IConfiguration config, ILogger<AzureBlobService> logger)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Stream> DownloadAsStream(string path)
        {
            var blockBlob = await ResolveCloudBlockBlob(path);
            var stream = new MemoryStream();

            await blockBlob.DownloadToStreamAsync(stream);
            Stream blobStream = blockBlob.OpenReadAsync().Result;
            return blobStream;
        }

        public async Task<string> DownloadAsURI(string path)
        {
            try
            {
                var blockBlob = await ResolveCloudBlockBlob(path);
                SharedAccessBlobPolicy policy = new SharedAccessBlobPolicy()
                {
                    Permissions = SharedAccessBlobPermissions.Read,
                    SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(60),
                };
                //Set content-disposition header for force download
                SharedAccessBlobHeaders headers = new SharedAccessBlobHeaders()
                {
                    ContentDisposition = string.Format("attachment;filename=\"{0}\"", path),
                };
                var sasToken = blockBlob.GetSharedAccessSignature(policy, headers);
                return blockBlob.Uri.AbsoluteUri + sasToken;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message, ex);
                throw;
            }

        }

        public async Task<string> UploadStreamToAzure(string path, Stream stream)
        {
            try
            {
                stream.Position = 0;
                var blockBlob = await ResolveCloudBlockBlob(path);
                await blockBlob.Container.CreateIfNotExistsAsync();
                await blockBlob.UploadFromStreamAsync(stream);
                return path;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message, ex);
                throw ex;
            }

        }

        public async Task<bool> RemoveBlob(string blobFileName)
        {
            try
            {
                if (string.IsNullOrEmpty(blobFileName)) return false;
                var blockBlob = await ResolveCloudBlockBlob(blobFileName);

                var sucess = await blockBlob.DeleteIfExistsAsync();

                return sucess;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message, ex);
                throw;
            }

        }

        private async Task<CloudBlockBlob> ResolveCloudBlockBlob(string blobFileName)
        {
            try
            {
                var container = await ResolveCloudBlobContainer();
                var blockBlob = container.GetBlockBlobReference(blobFileName);
                return blockBlob;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message, ex);
                throw;
            }

        }

        private async Task<CloudBlobContainer> ResolveCloudBlobContainer()
        {
            try
            {
                var containerName = _config.GetSection("ContainerName").Value;
                var storageAccount = GetCloudStorageAccount();
                var blobClient = storageAccount.CreateCloudBlobClient();
                var container = blobClient.GetContainerReference(containerName);
                await container.CreateIfNotExistsAsync();
                return container;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message, ex);
                throw;
            }

        }

        private CloudStorageAccount GetCloudStorageAccount()
        {
            try
            {
                if (CloudStorageAccount.TryParse(ResolveAzureStorageConnectionString(), out CloudStorageAccount account))
                {
                    return account;
                };
                throw new Exception($"No storage account with specified connection string: {ResolveAzureStorageConnectionString()} exists.");

            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message, ex);
                throw;
            }
        }

        private string ResolveAzureStorageConnectionString()
        {
            return _config.GetSection("StorageConnectionString").Value;
        }
    }
}
