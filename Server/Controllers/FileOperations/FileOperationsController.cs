using Application.Services;
using Core.Features.Contacts;
using Core.Features.Contacts.Commands;
using JobFileSystem.Shared.Contacts;
using JobFileSystem.Shared.Types;
using Microsoft.AspNetCore.Mvc;
using NdtTracking.Wasm.Server.Controllers;
using System.Text.Json;
using File = JobFileSystem.Shared.Types.File;

namespace JobFileSystem.Server.Controllers.Contacts
{
    public class FileOperationsController : BaseApiController
    {

        private readonly HttpClient _client;
        private readonly ISyncfusionFileManager _azureFileProvider;
        private readonly ILogger<FileOperationsController> _logger;

        public FileOperationsController(HttpClient httpClient,
                                        ISyncfusionFileManager azureFileProvider,
                                        ILogger<FileOperationsController> logger)
        {
            _client = httpClient;
            _azureFileProvider = azureFileProvider;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Run(string jobFileNumber)
        {
            var req = this.HttpContext.Request;
            if (!string.IsNullOrEmpty(req.Query["path"]))
            {
                var path = req.Query["path"];
                string startPath = "/";
                string originalPath = ("Files").Replace(startPath, "");
                path = (originalPath + path).Replace("//", "/");
                var image = await _azureFileProvider.GetImageAsync(path);
                return image;
            }
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var args = JsonSerializer.Deserialize<FileManagerDirectoryContent>(requestBody);
                string result;


                static string SubFolder(string path)
                {
                    if(string.IsNullOrEmpty(path)) return "/";
                    if (path.Replace("/", "").Length > 0)
                        return path;
                    else
                        return "/";
                } 

                args.Path = $"Files/{jobFileNumber}{SubFolder(args.Path)}";
                args.TargetPath = $"Files/{jobFileNumber}{SubFolder(args.TargetPath)}";

                switch (args.Action)
                {
                    case "read":
                        // Reads the file(s) or folder(s) from the given path.
                        var test = await _azureFileProvider.GetFilesAsync(args.Path, args.SearchString, args.Data);
                        result = CamelCaseSerializer.ToCamelCase(test);
                        return new OkObjectResult(result);
                    case "delete":
                        // Deletes the selected file(s) or folder(s) from the given path.
                        result = CamelCaseSerializer.ToCamelCase(await _azureFileProvider.RemoveAsync(args.Names, args.Path, args.Data));
                        return new OkObjectResult(result);
                    case "details":
                        // Gets the details of the selected file(s) or folder(s).
                        result = CamelCaseSerializer.ToCamelCase(await _azureFileProvider.GetDetailsAsync(args.Path, args.Names, args.Data));
                        return new OkObjectResult(result);
                    case "create":
                        // Creates a new folder in a given path.
                        result = CamelCaseSerializer.ToCamelCase(await _azureFileProvider.CreateAsync(args.Path, args.Name, args.Data));
                        return new OkObjectResult(result);
                    case "search":
                        // Gets the list of file(s) or folder(s) from a given path based on the searched key string.
                        result = CamelCaseSerializer.ToCamelCase(await _azureFileProvider.SearchAsync(args.Path, args.SearchString, args.CaseSensitive, args.Data));
                        return new OkObjectResult(result);
                    case "rename":
                        // Renames a file or folder.
                        result = CamelCaseSerializer.ToCamelCase(await _azureFileProvider.RenameAsync(args.Path, args.Name, args.NewName, args.Data));
                        return new OkObjectResult(result);
                    case "copy":
                        // Copies the selected file(s) or folder(s) from a path and then pastes them into a given target path.
                        result = CamelCaseSerializer.ToCamelCase(await _azureFileProvider.CopyToAsync(args.Path, args.TargetPath, args.Names, args.RenameFiles, args.Data));
                        return new OkObjectResult(result);
                    case "move":
                        // Cuts the selected file(s) or folder(s) from a path and then pastes them into a given target path.
                        result = CamelCaseSerializer.ToCamelCase(await _azureFileProvider.MoveToAsync(args.Path, args.TargetPath, args.Names, args.RenameFiles, args.Data));
                        return new OkObjectResult(result);

                }


                return new OkObjectResult("Response from function with injected dependencies.");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
                return new BadRequestObjectResult(ex);
            }

        }


        private static MemoryStream CopyToNew(IFormFile file)
        {
            var stream = new MemoryStream();
            file.CopyTo(stream);
            return stream;
        }

        [HttpPost("Upload")]
        public async Task<IActionResult> Upload(string jobFileNumber)
        {
            var form = await HttpContext.Request.ReadFormAsync();
            var action = form["action"];
            var path = form["path"];

            List<File> files() => 
                HttpContext.Request.Form.Files.Select(
                        x=> new File(CopyToNew(x), 0,x.Length,x.Name,x.FileName) ).ToList();




            if (path != "")
            {
                string startPath = "/";
                string originalPath = $"Files/{jobFileNumber}";
                path = (originalPath + path).Replace("//", "/");
                //----------------------
                //For example
                //string startPath = "https://azure_service_account.blob.core.windows.net/files/";
                //string originalPath = ("https://azure_service_account.blob.core.windows.net/files/Files").Replace(startPath, "");
                //args.Path = (originalPath + args.Path).Replace("//", "/");
                //----------------------
            }
            else
            {
                path = $"Files/{jobFileNumber}";
            }


            FileManagerResponse uploadResponse = await _azureFileProvider.UploadAsync(files(), action, path);
            if (uploadResponse.Error != null)
            {
                return new BadRequestObjectResult(uploadResponse.Error);
            }



            return new OkObjectResult(uploadResponse);

        }
        [HttpPost("Download")]
        public async Task<IActionResult> Download(string jobFileNumber)
        {
            try
            {

                var returnStream = new MemoryStream();


                var form = await Request.ReadFormAsync();
                var fileManagerContent = JsonSerializer.Deserialize<SyncfusionFileDownload>(form["downloadInput"]);

                var args = new List<SyncfusionFileDownload>() { fileManagerContent }.ToArray();

                var result = await _azureFileProvider.DownloadAsync(returnStream, fileManagerContent.Path, jobFileNumber, fileManagerContent.Names, args);
                return result;



            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
                return new BadRequestObjectResult(ex);
            }


        }
        public static class CamelCaseSerializer
        {
            public static string ToCamelCase(object data)
            {
                var serializeOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                };
                return JsonSerializer.Serialize(data, serializeOptions);

            }
        }
    }
}
