using JobFileSystem.Shared.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO.Compression;
using System.Text.RegularExpressions;

namespace Application.Services
{
    public interface ISyncfusionFileManager
    {
        Task<FileManagerResponse> CopyToAsync(string path, string targetPath, string[] names, string[] renamedFiles = null, params TargetData[] data);
        Task<FileManagerResponse> CreateAsync(string path, string name, params TargetData[] selectedItems);
        Task<FileStreamResult> DownloadAsync(MemoryStream stream, string path, string jobFileNumber, string[] names = null, params SyncfusionFileDownload[] selectedItems);
        Task GetAllFiles(string path, string searchString, FileManagerResponse data);
        Task<FileManagerResponse> GetDetailsAsync(string path, string[] names, IEnumerable<TargetData> selectedItems = null);
        Task<FileManagerResponse> GetFilesAsync(string path, string searchString, TargetData[] selectedItems);
        Task<FileStreamResult> GetImageAsync(string path);
        Task<FileManagerResponse> MoveToAsync(string path, string targetPath, string[] names, string[] renamedFiles = null, params TargetData[] data);
        Task<FileManagerResponse> RemoveAsync(string[] names, string path, params TargetData[] selectedItems);
        Task<FileManagerResponse> RenameAsync(string path, string oldName, string newName, params TargetData[] selectedItems);
        Task<FileManagerResponse> SearchAsync(string path, string searchString, bool caseSensitive, params TargetData[] data);
        Task<FileManagerResponse> UploadAsync(IEnumerable<IFile> files, string action, string path, IEnumerable<object> selectedItems = null);
    }

    public class SyncfusionFileManager : ISyncfusionFileManager
    {
        private readonly IConfiguration _config;
        private readonly ILogger<SyncfusionFileManager> _logger;
        private List<FileManagerDirectoryContent> directoryContentItems = new List<FileManagerDirectoryContent>();
        private readonly string blobPath;
        private long size;
        private string rootPath;
        private List<string> existFiles = new List<string>();
        private bool isFolderAvailable = false;
        private DateTime lastUpdated = DateTime.MinValue;
        private DateTime prevUpdated = DateTime.MinValue;


        public SyncfusionFileManager(IConfiguration config, ILogger<SyncfusionFileManager> logger)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            blobPath = _config.GetSection("ContainerName").Value;
            rootPath = "Files".Replace(blobPath, "");

        }



        // Performs files operations
        private protected async Task<BlobResultSegment> AsyncReadCall(string path, string action)
        {
            var container = await ResolveCloudBlobContainer();
            CloudBlobDirectory sampleDirectory = container.GetDirectoryReference(path);
            BlobRequestOptions options = new BlobRequestOptions();
            OperationContext context = new OperationContext();
            dynamic Asyncitem = null;
            if (action == "Read") Asyncitem = await sampleDirectory.ListBlobsSegmentedAsync(false, BlobListingDetails.Metadata, null, null, options, context);
            if (action == "Paste") Asyncitem = await sampleDirectory.ListBlobsSegmentedAsync(false, BlobListingDetails.None, null, null, options, context);
            if (action == "Rename") Asyncitem = await sampleDirectory.ListBlobsSegmentedAsync(true, BlobListingDetails.Metadata, null, null, options, context);
            if (action == "Remove") Asyncitem = await sampleDirectory.ListBlobsSegmentedAsync(true, BlobListingDetails.None, null, null, options, context);
            if (action == "HasChild") Asyncitem = await sampleDirectory.ListBlobsSegmentedAsync(false, BlobListingDetails.None, null, null, options, context);
            // Return Asyncitem;
            return await Task.Run(() =>
            {
                return Asyncitem;
            });
        }


        public async Task<FileManagerResponse> GetFilesAsync(string path, string searchString, TargetData[] selectedItems)
        {

            var container = await ResolveCloudBlobContainer();
            FileManagerResponse readResponse = new FileManagerResponse();
            List<FileManagerDirectoryContent> details = new List<FileManagerDirectoryContent>();
            FileManagerDirectoryContent cwd = new FileManagerDirectoryContent();
            try
            {
                if (selectedItems.Length <= 0 || selectedItems[0] == null)
                    selectedItems = Array.Empty<TargetData>();

                var filter = "*.*";
                string[] extensions = ((filter.Replace(" ", "")) ?? "*").Split(",|;".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);
                //if (!string.IsNullOrEmpty(searchString))
                //{
                //    extensions = ((searchString.Replace(" ", "")) ?? "*").Split(",|;".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);
                //}

                CloudBlobDirectory sampleDirectory = container.GetDirectoryReference(path);
                cwd.Name = sampleDirectory.Prefix.Split(sampleDirectory.Parent.Prefix)[sampleDirectory.Prefix.Split(sampleDirectory.Parent.Prefix).Length - 1].Replace("/", "");
                cwd.Type = "File Folder";
                cwd.FilterPath = selectedItems.Length > 0 ? selectedItems[0].FilterPath : "";
                cwd.Size = 0;
                cwd.HasChild = await HasChildDirectory(path);
                readResponse.CWD = cwd;
                BlobResultSegment items = await AsyncReadCall(path, "Read");
                foreach (IListBlobItem item in items.Results)
                {
                    bool includeItem = true;
                    var itemType = item.GetType();
                    if (extensions.Length > 0 && !(extensions[0].Equals("*.*") || extensions[0].Equals("*")) && item.GetType() == typeof(CloudBlockBlob))
                    {
                        CloudBlockBlob file = (CloudBlockBlob)item;
                        if (!(Array.IndexOf(extensions, "*." + file.Name.ToString().Trim().Split('.')[file.Name.ToString().Trim().Split('.').Count() - 1]) >= 0))
                            includeItem = false;
                    }
                    if (includeItem)
                    {
                        FileManagerDirectoryContent entry = new FileManagerDirectoryContent();
                        if (item.GetType() == typeof(CloudBlockBlob))
                        {
                            CloudBlockBlob file = (CloudBlockBlob)item;
                            if (!file.Name.Contains("About.txt"))
                            {
                                entry.Name = file.Name.Replace(path, "");
                                entry.Type = System.IO.Path.GetExtension(file.Name.Replace(path, ""));
                                entry.IsFile = true;
                                entry.Size = file.Properties.Length;
                                entry.DateModified = file.Properties.LastModified.Value.LocalDateTime;
                                entry.HasChild = false;
                                entry.FilterPath = selectedItems.Length > 0 ? path.Replace(this.rootPath, "") : "/";
                                details.Add(entry);
                            }

                        }
                        else if (item.GetType() == typeof(CloudBlobDirectory))
                        {
                            CloudBlobDirectory directory = (CloudBlobDirectory)item;
                            entry.Name = directory.Prefix.Replace(path, "").Replace("/", "");
                            entry.Type = "Directory";
                            entry.IsFile = false;
                            entry.Size = 0;
                            entry.HasChild = await HasChildDirectory(directory.Prefix);
                            entry.FilterPath = selectedItems.Length > 0 ? path.Replace(this.rootPath, "") : "/";
                            entry.DateModified = await DirectoryLastModified(directory.Prefix);
                            lastUpdated = prevUpdated = DateTime.MinValue;
                            details.Add(entry);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return readResponse;
            }
            readResponse.Files = details;
            return readResponse;
        }


        // Returns the last modified date for directories
        private protected async Task<DateTime> DirectoryLastModified(string path)
        {
            BlobResultSegment items = await AsyncReadCall(path, "Read");
            // Checks the corresponding folder's last modified date of recent updated file from any of its sub folders. 
            if (items.Results.Where(x => x.GetType() == typeof(CloudBlobDirectory)).Select(x => x).ToList().Count > 0)
            {
                List<IListBlobItem> folderItems = items.Results.Where(x => x.GetType() == typeof(CloudBlobDirectory)).Select(x => x).ToList();
                foreach (IListBlobItem item in folderItems)
                {
                    DateTime checkFolderModified = DirectoryLastModified(((CloudBlobDirectory)item).Prefix).Result;
                    lastUpdated = prevUpdated = (prevUpdated < checkFolderModified) ? checkFolderModified : prevUpdated;
                }
            }
            // Checks the corresponding folder's last modified date of recent updated file
            if (items.Results.Where(x => x.GetType() == typeof(CloudBlockBlob)).Select(x => x).ToList().Count > 0)
            {
                DateTime checkFileModified = ((CloudBlockBlob)items.Results.Where(x => x.GetType() == typeof(CloudBlockBlob)).Select(x => x).ToList().OrderByDescending(m => ((CloudBlockBlob)m).Properties.LastModified).ToList().First()).Properties.LastModified.Value.LocalDateTime;
                lastUpdated = prevUpdated = prevUpdated < checkFileModified ? checkFileModified : prevUpdated;
            }
            return lastUpdated;
        }
        // Converts the byte size value to appropriate value
        private protected string ByteConversion(long fileSize)
        {
            try
            {
                string[] index = { "B", "KB", "MB", "GB", "TB", "PB", "EB" }; // Longs run out around EB
                if (fileSize == 0)
                {
                    return "0 " + index[0];
                }
                int value = Convert.ToInt32(Math.Floor(Math.Log(Math.Abs(fileSize), 1024)));
                return (Math.Sign(fileSize) * Math.Round(Math.Abs(fileSize) / Math.Pow(1024, value), 1)).ToString() + " " + index[value];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Gets the size value of the directory
        private protected async Task<long> GetSizeValue(string directory)
        {
            var container = await ResolveCloudBlobContainer();
            BlobResultSegment items = await AsyncReadCall(directory, "Read");
            foreach (IListBlobItem item in items.Results)
            {
                if (item is CloudBlockBlob blockBlob)
                {
                    CloudBlockBlob blob = container.GetBlockBlobReference(new CloudBlockBlob(item.Uri).Name);
                    await blob.FetchAttributesAsync();
                    size = size + blob.Properties.Length;
                }
                else if (item is CloudBlobDirectory blobDirectory)
                {
                    // Set your download target path as below methods parameter
                    await GetSizeValue(item.Uri.ToString().Replace(blobPath, ""));
                }
            }
            return size;
        }
        // Gets details of the files

        public async Task<FileManagerResponse> GetDetailsAsync(string path, string[] names, IEnumerable<TargetData> selectedItems = null)
        {
            var container = await ResolveCloudBlobContainer();
            bool isVariousFolders = false;
            string previousPath = "";
            string previousName = "";
            FileManagerResponse detailsResponse = new FileManagerResponse();
            try
            {
                bool isFile = false;
                bool namesAvailable = names.Length > 0 ? true : false;
                if (names.Length == 0 && selectedItems != null)
                {
                    List<string> values = new List<string>();
                    foreach (var item in selectedItems)
                    {
                        values.Add(item.Name);
                    }
                    names = values.ToArray();
                }
                FileDetails fileDetails = new FileDetails();
                long multipleSize = 0;
                if (selectedItems != null)
                {
                    foreach (var fileItem in selectedItems)
                    {

                        if (names.Length == 1)
                        {
                            if (fileItem.IsFile)
                            {
                                var blob = container.GetBlockBlobReference(rootPath + fileItem.FilterPath + fileItem.Name);
                                isFile = fileItem.IsFile;
                                fileDetails.IsFile = isFile;
                                await blob.FetchAttributesAsync();
                                fileDetails.Name = fileItem.Name;
                                fileDetails.Location = ((namesAvailable ? (rootPath + fileItem.FilterPath + fileItem.Name) : path)).Replace("/", @"\");
                                fileDetails.Size = ByteConversion(blob.Properties.Length);
                                fileDetails.Modified = blob.Properties.LastModified.Value.LocalDateTime;
                                detailsResponse.Details = fileDetails;
                            }

                            else
                            {
                                CloudBlobDirectory sampleDirectory = container.GetDirectoryReference(rootPath + fileItem.FilterPath + fileItem.Name);
                                long sizeValue = GetSizeValue((namesAvailable ? rootPath + fileItem.FilterPath + fileItem.Name : "")).Result;
                                isFile = false;
                                fileDetails.Name = fileItem.Name;
                                fileDetails.Location = ((namesAvailable ? rootPath + fileItem.FilterPath + fileItem.Name : path.Substring(0, path.Length - 1))).Replace("/", @"\");
                                fileDetails.Size = ByteConversion(sizeValue);
                                fileDetails.Modified = await DirectoryLastModified(path);
                                detailsResponse.Details = fileDetails;
                            }
                        }
                        else
                        {
                            multipleSize = multipleSize + (fileItem.IsFile ? fileItem.Size : GetSizeValue((namesAvailable ? rootPath + fileItem.FilterPath + fileItem.Name : path)).Result);
                            size = 0;
                            fileDetails.Name = previousName == "" ? previousName = fileItem.Name : previousName + ", " + fileItem.Name;
                            previousPath = previousPath == "" ? rootPath + fileItem.FilterPath : previousPath;
                            if (previousPath == rootPath + fileItem.FilterPath && !isVariousFolders)
                            {
                                previousPath = rootPath + fileItem.FilterPath;
                                fileDetails.Location = ((rootPath + fileItem.FilterPath).Replace("/", @"\")).Substring(0, ((rootPath + fileItem.FilterPath).Replace("/", @"\")).Length - 1);
                            }
                            else
                            {
                                isVariousFolders = true;
                                fileDetails.Location = "Various Folders";
                            }
                            fileDetails.Size = ByteConversion(multipleSize);
                            fileDetails.MultipleFiles = true;
                            detailsResponse.Details = fileDetails;
                        }

                    }
                }
                return await Task.Run(() =>
                {
                    size = 0;
                    return detailsResponse;
                });
            }
            catch (Exception ex) { throw ex; }
        }
        // Creates a new folder
        public async Task<FileManagerResponse> CreateAsync(string path, string name, params TargetData[] selectedItems)
        {
            this.isFolderAvailable = false;
            await CreateFolderAsync(path, name, selectedItems);
            FileManagerResponse createResponse = new FileManagerResponse();
            if (!this.isFolderAvailable)
            {
                FileManagerDirectoryContent content = new FileManagerDirectoryContent();
                content.Name = name;
                FileManagerDirectoryContent[] directories = new[] { content };
                createResponse.Files = (IEnumerable<FileManagerDirectoryContent>)directories;
            }
            else
            {
                ErrorDetails error = new ErrorDetails();
                error.FileExists = existFiles;
                error.Code = "400";
                error.Message = "Folder Already Already Exists";
                createResponse.Error = error;
            }
            return createResponse;
        }
        // Creates a new folder
        protected async Task CreateFolderAsync(string path, string name, IEnumerable<object> selectedItems = null)
        {
            var container = await ResolveCloudBlobContainer();
            BlobResultSegment items = await AsyncReadCall(path, "Read");
            string checkName = name.Contains(" ") ? name.Replace(" ", "%20") : name;
            if (await IsFolderExists(path + name) || (items.Results.Where(x => x.Uri.Segments.Last().Replace("/", "").ToLower() == checkName.ToLower()).Select(i => i).ToArray().Length > 0))
            {
                this.isFolderAvailable = true;
            }
            else
            {
                CloudBlockBlob blob = container.GetBlockBlobReference(path + name + "/About.txt");
                blob.Properties.ContentType = "text/plain";
                await blob.UploadTextAsync("This is a auto generated file");
            }
        }
        // Renames file(s) or folder(s)


        public async Task<FileManagerResponse> RenameAsync(string path, string oldName, string newName, params TargetData[] selectedItems)
        {
            var container = await ResolveCloudBlobContainer();
            FileManagerResponse renameResponse = new FileManagerResponse();
            List<FileManagerDirectoryContent> details = new List<FileManagerDirectoryContent>();
            FileManagerDirectoryContent entry = new FileManagerDirectoryContent();
            bool isAlreadyAvailable = false;
            bool isFile = false;
            foreach (var fileItem in selectedItems)
            {
                TargetData directoryContent = fileItem;
                isFile = directoryContent.IsFile;
                if (isFile)
                {
                    isAlreadyAvailable = await IsFileExists(path + newName);
                }
                else
                {
                    isAlreadyAvailable = await IsFolderExists(path + newName);
                }
                entry.Name = newName;
                entry.Type = directoryContent.Type;
                entry.IsFile = isFile;
                entry.Size = directoryContent.Size;
                entry.HasChild = directoryContent.HasChild;
                entry.FilterPath = path;
                details.Add(entry);
                break;
            }
            if (!isAlreadyAvailable)
            {
                if (isFile)
                {
                    CloudBlob existBlob = container.GetBlobReference(path + oldName);
                    await (container.GetBlobReference(path + newName)).StartCopyAsync(existBlob.Uri);
                    await existBlob.DeleteIfExistsAsync();
                }
                else
                {
                    CloudBlobDirectory sampleDirectory = container.GetDirectoryReference(path + oldName);
                    BlobResultSegment items = await AsyncReadCall(path + oldName, "Rename");
                    foreach (IListBlobItem item in items.Results)
                    {
                        string name = item.Uri.AbsolutePath.Replace(sampleDirectory.Uri.AbsolutePath, "").Replace("%20", " ");
                        await (container.GetBlobReference(path + newName + "/" + name)).StartCopyAsync(item.Uri);
                        await container.GetBlobReference(path + oldName + "/" + name).DeleteAsync();
                    }

                }
                renameResponse.Files = (IEnumerable<FileManagerDirectoryContent>)details;
            }
            else
            {
                ErrorDetails error = new ErrorDetails();
                error.FileExists = existFiles;
                error.Code = "400";
                error.Message = "File or Folder Already Already Exists";
                renameResponse.Error = error;
            }
            return renameResponse;
        }

        // Deletes file(s) or folder(s)
        public async Task<FileManagerResponse> RemoveAsync(string[] names, string path, params TargetData[] selectedItems)
        {
            var container = await ResolveCloudBlobContainer();
            FileManagerResponse removeResponse = new FileManagerResponse();
            List<FileManagerDirectoryContent> details = new List<FileManagerDirectoryContent>();
            FileManagerDirectoryContent entry = new FileManagerDirectoryContent();
            foreach (var fileItem in selectedItems)
            {
                if (fileItem.IsFile)
                {
                    CloudBlockBlob blockBlob = container.GetBlockBlobReference(path + fileItem.Name);
                    await blockBlob.DeleteAsync();
                    string absoluteFilePath = Path.Combine(Path.GetTempPath(), fileItem.Name);
                    var tempDirectory = new DirectoryInfo(Path.GetTempPath());
                    foreach (var file in Directory.GetFiles(tempDirectory.ToString()))
                    {
                        if (file.ToString() == absoluteFilePath)
                        {
                            System.IO.File.Delete(file);
                        }
                    }
                    entry.Name = fileItem.Name;
                    entry.Type = fileItem.Type;
                    entry.IsFile = fileItem.IsFile;
                    entry.Size = fileItem.Size;
                    entry.HasChild = fileItem.HasChild;
                    entry.FilterPath = path;
                    details.Add(entry);
                }
                else
                {
                    path = path.Replace(this.blobPath, "") + fileItem.FilterPath;
                    CloudBlobDirectory subDirectory = container.GetDirectoryReference(path + fileItem.Name);
                    BlobResultSegment items = await AsyncReadCall(path + fileItem.Name, "Remove");
                    foreach (IListBlobItem item in items.Results)
                    {
                        CloudBlockBlob blockBlob = container.GetBlockBlobReference(path + fileItem.Name + "/" + item.Uri.ToString().Replace(subDirectory.Uri.ToString(), ""));
                        await blockBlob.DeleteAsync();
                        entry.Name = fileItem.Name;
                        entry.Type = fileItem.Type;
                        entry.IsFile = fileItem.IsFile;
                        entry.Size = fileItem.Size;
                        entry.HasChild = fileItem.HasChild;
                        entry.FilterPath = path;
                        details.Add(entry);
                    }
                }
            }
            removeResponse.Files = (IEnumerable<FileManagerDirectoryContent>)details;
            return removeResponse;
        }

        // Upload file(s) to the storage
        public async Task<FileManagerResponse> UploadAsync(IEnumerable<IFile> files, string action, string path, IEnumerable<object> selectedItems = null)
        {
            var container = await ResolveCloudBlobContainer();
            FileManagerResponse uploadResponse = new FileManagerResponse();
            try
            {
                foreach (IFile file in files)
                {
                    if (files != null)
                    {
                        CloudBlockBlob blockBlob = container.GetBlockBlobReference(path.Replace(this.blobPath, "") + file.FileName);
                        blockBlob.Properties.ContentType = file.ContentType;
                        string fileName = file.FileName;
                        string absoluteFilePath = Path.Combine(path, fileName);
                        if (action == "save")
                        {
                            if (!await IsFileExists(absoluteFilePath))
                            {
                                await blockBlob.UploadFromStreamAsync(file.OpenReadStream());
                            }
                            else
                            {
                                existFiles.Add(fileName);
                            }
                        }
                        else if (action == "replace")
                        {
                            if (await IsFileExists(absoluteFilePath))
                            {
                                await blockBlob.DeleteAsync();
                            }

                            await blockBlob.UploadFromStreamAsync(file.OpenReadStream());
                        }
                        else if (action == "keepboth")
                        {
                            string newAbsoluteFilePath = absoluteFilePath;
                            string newFileName = file.FileName;
                            int index = absoluteFilePath.LastIndexOf(".");
                            int indexValue = newFileName.LastIndexOf(".");
                            if (index >= 0)
                            {
                                newAbsoluteFilePath = absoluteFilePath.Substring(0, index);
                                newFileName = newFileName.Substring(0, indexValue);
                            }
                            int fileCount = 0;
                            while (await IsFileExists(newAbsoluteFilePath + (fileCount > 0 ? "(" + fileCount.ToString() + ")" + Path.GetExtension(fileName) : Path.GetExtension(fileName))))
                            {
                                fileCount++;
                            }

                            newAbsoluteFilePath = newFileName + (fileCount > 0 ? "(" + fileCount.ToString() + ")" : "") + Path.GetExtension(fileName);

                            CloudBlockBlob newBlob = container.GetBlockBlobReference(path.Replace(this.blobPath, "") + newAbsoluteFilePath);
                            newBlob.Properties.ContentType = file.ContentType;
                            await newBlob.UploadFromStreamAsync(file.OpenReadStream());
                        }
                    }
                }
                if (existFiles.Count != 0)
                {
                    ErrorDetails error = new ErrorDetails();
                    error.FileExists = existFiles;
                    error.Code = "400";
                    error.Message = "File Already Exists";
                    uploadResponse.Error = error;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return uploadResponse;
        }
        protected async Task CopyFileToTemp(string path, CloudBlockBlob blockBlob)
        {
            using (FileStream fileStream = System.IO.File.Create(path))
            {
                await blockBlob.DownloadToStreamAsync(fileStream);
                fileStream.Close();
            }
        }

        // Download file(s) from the storage
        public async Task<FileStreamResult> DownloadAsync(MemoryStream returnStream, string path, string jobFileNumber, string[] names = null, params SyncfusionFileDownload[] content)
        {
            var container = await ResolveCloudBlobContainer();
            var blob = container.GetBlockBlobReference("temp.zip");
            await container.CreateIfNotExistsAsync();


            if (content.First().Data.First().IsFile)
            {
                using (var stream = await blob.OpenWriteAsync())
                {

                    using (var zip = new ZipArchive(stream, ZipArchiveMode.Create))
                    {
                        foreach (var name in names)
                        {
                            var filePath = $"Files/{jobFileNumber}{path}{name}";
                            CloudBlockBlob blockBlob = container.GetBlockBlobReference(filePath);
                            var entry = zip.CreateEntry(name, CompressionLevel.Optimal);

                            using (var innerFile = entry.Open())
                            {

                                await blockBlob.DownloadToStreamAsync(innerFile);
                            }


                        }
                    }

                    await blob.DownloadToStreamAsync(returnStream);
                    returnStream.Position = 0;
                    FileStreamResult fileStreamResult = new FileStreamResult(returnStream, "application/zip");
                    fileStreamResult.FileDownloadName = "Files.zip";
                    await blob.DeleteAsync().ConfigureAwait(false);
                    return fileStreamResult;


                }
            }
            else
            {
                using (var stream = await blob.OpenWriteAsync())
                {

                    using (var zip = new ZipArchive(stream, ZipArchiveMode.Create))
                    {
                        var folderPath = string.Empty;
                        if (names != null && names.Length > 0 && names.First() != jobFileNumber)
                        {
                            folderPath = $"Files/{jobFileNumber}/{names.First()}";
                        }
                        else if (names != null && names.Length > 0 && names.First() == jobFileNumber)
                        {
                            folderPath = $"Files/{jobFileNumber}";
                        }

                        await DownloadFolder(folderPath, zip);
                    }

                    await blob.DownloadToStreamAsync(returnStream);
                    returnStream.Position = 0;
                    FileStreamResult fileStreamResult = new FileStreamResult(returnStream, "application/zip");
                    fileStreamResult.FileDownloadName = "Files.zip";
                    await blob.DeleteAsync().ConfigureAwait(false);
                    return fileStreamResult;


                }

            }
            return null;
        }

        // Download folder(s) from the storage
        private async Task DownloadFolder(string path, ZipArchive archive)
        {
            var container = await ResolveCloudBlobContainer();


            BlobContinuationToken continuationToken = null;

            //Use maxResultsPerQuery to limit the number of results per query as desired. `null` will have the query return the entire contents of the blob container
            int? maxResultsPerQuery = null;

            do
            {
                var response = await container.ListBlobsSegmentedAsync(string.Empty, true, BlobListingDetails.None, maxResultsPerQuery, continuationToken, null, null);
                continuationToken = response.ContinuationToken;

                foreach (var blob in response.Results.OfType<CloudBlockBlob>())
                {
                    if (blob.Name.Contains(path))
                    {
                        var entry = archive.CreateEntry(blob.Name, CompressionLevel.Optimal);

                        using (var innerFile = entry.Open())
                        {

                            await blob.DownloadToStreamAsync(innerFile);
                        }
                    }

                }
            } while (continuationToken != null);





        }
        // Check whether the directory has child
        private async Task<bool> HasChildDirectory(string path)
        {
            BlobResultSegment items = await AsyncReadCall(path, "HasChild");
            foreach (IListBlobItem item in items.Results)
            {
                if (item.GetType() == typeof(CloudBlobDirectory))
                {
                    return true;
                }
            }
            return false;
        }

        // To get the file details
        private FileManagerDirectoryContent GetFileDetails(string targetPath, TargetData fileDetails)
        {
            FileManagerDirectoryContent entry = new FileManagerDirectoryContent();
            entry.Name = fileDetails.Name;
            entry.Type = fileDetails.Type;
            entry.IsFile = fileDetails.IsFile;
            entry.Size = fileDetails.Size;
            entry.HasChild = fileDetails.HasChild;
            entry.FilterPath = targetPath;
            return entry;
        }

        // To check if folder exists
        private async Task<bool> IsFolderExists(string path)
        {
            BlobResultSegment items = await AsyncReadCall(path, "Paste");
            return await Task.Run(() =>
            {
                return items.Results.Count<IListBlobItem>() > 0;
            });
        }

        // To check if file exists
        private async Task<bool> IsFileExists(string path)
        {
            var container = await ResolveCloudBlobContainer();
            CloudBlob newBlob = container.GetBlobReference(path);
            return await newBlob.ExistsAsync();
        }



        public async Task<FileManagerResponse> CopyToAsync(string path, string targetPath, string[] names, string[] renamedFiles = null, params TargetData[] data)
        {
            FileManagerResponse copyResponse = new FileManagerResponse();
            List<FileManagerDirectoryContent> copiedFiles = new List<FileManagerDirectoryContent>();

            var container = await ResolveCloudBlobContainer();
            try
            {
                renamedFiles = renamedFiles ?? new string[0];
                foreach (var item in data)
                {
                    path = Path.Combine(path, item.Name);
                    var newFilePath = Path.Combine(targetPath, item.Name);
                    var source = container.GetBlockBlobReference(path);
                    var target = container.GetBlockBlobReference(newFilePath);

                    if (await source.ExistsAsync())
                    {
                        await target.StartCopyAsync(source.Uri);

                        while (target.CopyState.Status == CopyStatus.Pending)
                            await Task.Delay(100);

                        if (target.CopyState.Status != CopyStatus.Success)
                            throw new Exception("Rename failed: " + target.CopyState.Status);
                        copiedFiles.Add(GetFileDetails(targetPath, item));
                    }

                }
                copyResponse.Files = copiedFiles;


                return copyResponse;
            }
            catch (Exception e)
            {
                ErrorDetails error = new ErrorDetails();
                error.Code = "404";
                error.Message = e.Message.ToString();
                error.FileExists = copyResponse.Error?.FileExists;
                copyResponse.Error = error;
                return copyResponse;
            }
        }

        // To iterate and copy subfolder
        private async void CopySubFolder(TargetData subFolder, string targetPath)
        {
            BlobResultSegment items = await AsyncReadCall(subFolder.Path, "Paste");
            targetPath = targetPath + subFolder.Name + "/";
            foreach (IListBlobItem item in items.Results)
            {
                if (item.GetType() == typeof(CloudBlockBlob))
                {
                    CloudBlockBlob BlobItem = (CloudBlockBlob)item;
                    string name = BlobItem.Name.Replace(subFolder.Path + "/", "");
                    string sourcePath = BlobItem.Name.Replace(name, "");
                    await CopyItems(sourcePath, targetPath, name, null);
                }
                if (item.GetType() == typeof(CloudBlobDirectory))
                {
                    CloudBlobDirectory BlobItem = (CloudBlobDirectory)item;
                    var itemDetail = new TargetData();
                    itemDetail.Name = BlobItem.Prefix.Replace(subFolder.Path, "").Replace("/", "");
                    itemDetail.Path = subFolder.Path + "/" + itemDetail.Name;
                    CopySubFolder(itemDetail, targetPath);
                }
            }
        }

        // To iterate and copy files
        private async Task CopyItems(string sourcePath, string targetPath, string name, string newName)
        {
            var container = await ResolveCloudBlobContainer();
            if (newName == null) { newName = name; }
            CloudBlob existBlob = container.GetBlobReference(sourcePath + name);
            CloudBlob newBlob = container.GetBlobReference(targetPath + newName);
            await newBlob.StartCopyAsync(existBlob.Uri);
        }

        // To rename files incase of duplicates
        private async Task<string> FileRename(string newPath, string fileName)
        {
            int index = fileName.LastIndexOf(".");
            string nameNotExist = string.Empty;
            nameNotExist = index >= 0 ? fileName.Substring(0, index) : fileName;
            int fileCount = 0;
            while (index > -1 ? await IsFileExists(newPath + nameNotExist + (fileCount > 0 ? "(" + fileCount.ToString() + ")" + Path.GetExtension(fileName) : Path.GetExtension(fileName))) : await IsFolderExists(newPath + nameNotExist + (fileCount > 0 ? "(" + fileCount.ToString() + ")" + Path.GetExtension(fileName) : Path.GetExtension(fileName))))
            {
                fileCount++;
            }
            fileName = nameNotExist + (fileCount > 0 ? "(" + fileCount.ToString() + ")" : "") + Path.GetExtension(fileName);
            return await Task.Run(() =>
            {
                return fileName;
            });
        }

        // Returns the image 
        public async Task<FileStreamResult> GetImageAsync(string path)
        {
            try
            {
                var container = await ResolveCloudBlobContainer();
                var blockBlob = container.GetBlockBlobReference(path);
                var stream = new MemoryStream();

                await blockBlob.DownloadToStreamAsync(stream);
                Stream blobStream = await blockBlob.OpenReadAsync();
                return new FileStreamResult(blobStream, "APPLICATION/octet-stream");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        private async Task MoveItems(string sourcePath, string targetPath, string name, string newName)
        {
            var container = await ResolveCloudBlobContainer();
            CloudBlob existBlob = container.GetBlobReference(sourcePath + name);
            await CopyItems(sourcePath, targetPath, name, newName);
            await existBlob.DeleteIfExistsAsync();
        }

        private async void MoveSubFolder(TargetData subFolder, string targetPath)
        {
            BlobResultSegment items = await AsyncReadCall(subFolder.Path, "Paste");
            targetPath = targetPath + subFolder.Name + "/";
            foreach (IListBlobItem item in items.Results)
            {
                if (item.GetType() == typeof(CloudBlockBlob))
                {
                    CloudBlockBlob BlobItem = (CloudBlockBlob)item;
                    string name = BlobItem.Name.Replace(subFolder.Path + "/", "");
                    string sourcePath = BlobItem.Name.Replace(name, "");
                    await MoveItems(sourcePath, targetPath, name, null);
                }
                if (item.GetType() == typeof(CloudBlobDirectory))
                {
                    CloudBlobDirectory BlobItem = (CloudBlobDirectory)item;
                    var itemDetail = new TargetData();
                    itemDetail.Name = BlobItem.Prefix.Replace(subFolder.Path, "").Replace("/", "");
                    itemDetail.Path = subFolder.Path + "/" + itemDetail.Name;
                    CopySubFolder(itemDetail, targetPath);
                }
            }
        }

        public async Task<FileManagerResponse> MoveToAsync(string path, string target, string[] names, string[] renamedFiles = null, params TargetData[] data)
        {
            FileManagerResponse moveResponse = new FileManagerResponse();
            try
            {
                foreach (var name in names)
                {
                    var sourcePath = Path.Combine(path, name);
                    var targetPath = Path.Combine(target, name);

                    var container = await ResolveCloudBlobContainer();
                    var sourceBlob = container.GetBlockBlobReference(sourcePath);

                    if (await sourceBlob.ExistsAsync())
                    {
                        var targetBlob = container.GetBlockBlobReference(targetPath);

                        await targetBlob.StartCopyAsync(sourceBlob.Uri);

                        while (targetBlob.CopyState.Status == CopyStatus.Pending)
                            await Task.Delay(100);

                        if (targetBlob.CopyState.Status != CopyStatus.Success)
                            throw new Exception("Rename failed: " + targetBlob.CopyState.Status);

                        await sourceBlob.DeleteAsync();
                    }


                }


                return moveResponse;
            }
            catch (Exception e)
            {
                ErrorDetails error = new ErrorDetails();
                error.Code = "404";
                error.Message = e.Message.ToString();
                error.FileExists = moveResponse.Error?.FileExists;
                moveResponse.Error = error;
                return moveResponse;
            }
        }

        // Search for file(s) or folders
        public async Task<FileManagerResponse> SearchAsync(string path, string searchString, bool caseSensitive, params TargetData[] data)
        {
            directoryContentItems.Clear();
            FileManagerResponse searchResponse = await GetFilesAsync(path, searchString, data);
            directoryContentItems.AddRange(searchResponse.Files);
            await GetAllFiles(path, searchString, searchResponse);
            searchResponse.Files = directoryContentItems.Where(item => new Regex(WildcardToRegex(searchString), (caseSensitive ? RegexOptions.None : RegexOptions.IgnoreCase)).IsMatch(item.Name));
            return searchResponse;
        }

        // Gets all files
        public async Task GetAllFiles(string path, string searchString, FileManagerResponse data)
        {
            FileManagerResponse directoryList = new FileManagerResponse();
            directoryList.Files = data.Files.Where(item => item.IsFile == false).Select(x => x).AsEnumerable();

            var dataFiles = directoryList.Files.SelectMany(x => x.Data).ToList();

            for (int i = 0; i < directoryList.Files.Count(); i++)
            {
                var file = dataFiles[i];
                FileManagerResponse innerData = await GetFilesAsync(path + directoryList.Files.ElementAt(i).Name + "/", searchString, new TargetData[] { file });
                innerData.Files = innerData.Files.Select(file => new FileManagerDirectoryContent
                {
                    Name = file.Name,
                    Type = file.Type,
                    IsFile = file.IsFile,
                    Size = file.Size,
                    HasChild = file.HasChild,
                    FilterPath = (file.FilterPath)
                });
                directoryContentItems.AddRange(innerData.Files);
                await GetAllFiles(path + directoryList.Files.ElementAt(i).Name + "/", searchString, innerData);
            }
        }

        private protected virtual string WildcardToRegex(string pattern)
        {
            return "^" + Regex.Escape(pattern).Replace(@"\*", ".*").Replace(@"\?", ".") + "$";
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
