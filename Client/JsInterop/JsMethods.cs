using Microsoft.JSInterop;

namespace JobFileSystem.Client.JsInterop
{
    public interface IJsMethods
    {
        Task Print(string content,string fileName);
        void Print(string name, string contentType, byte[] content);
        Task DownloandBlobLink(string link, string fileName);
        Task<string> GetFileData(object file);
    }

    public class JsMethods : IJsMethods
    {
        private readonly IJSUnmarshalledRuntime runtime;
        private readonly IJSRuntime _jSRuntime;

        public JsMethods(IJSUnmarshalledRuntime runtime, IJSRuntime jSRuntime)
        {
            this.runtime = runtime;
            _jSRuntime = jSRuntime;
        }
        private static string PrintEstimate => "printEstimate";
        private static string GetBase64 => "getBase64";
        private static string DownloadBase64 => "downloadBase64";
        private static string DownloadBlob => "downloadBlobLink";
        private static string DownloadFile => "fileDownload";

        public async Task<string> GetFileData(object file)
        {
            return await _jSRuntime.InvokeAsync<string>(GetBase64, file);
        }
        public async Task DownloandBlobLink(string link, string fileName)
        {
            if (string.IsNullOrEmpty(link) ||
               string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException("invalid file.");
           

            await _jSRuntime.InvokeVoidAsync(DownloadBlob, new object[] { link, fileName });
        }
        public void Print(string name, string contentType, byte[] content)
        {
            if(string.IsNullOrEmpty(name) || 
               string.IsNullOrEmpty(contentType) || 
               content is null || content.Length <=0) 
                throw new ArgumentNullException("invalid file.");

            runtime.InvokeUnmarshalled<string, string, byte[], bool>(DownloadFile, name, contentType, content);
        }

        async Task IJsMethods.Print(string content,string fileName)
        {
            await _jSRuntime.InvokeAsync<string>(PrintEstimate, new object[] {content,fileName });
        }
    }
}
