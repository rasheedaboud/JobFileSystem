using Microsoft.AspNetCore.Components.Forms;

namespace JobFileSystem.Client.Utils
{
    public static class Files
    {

        public static async Task<string> ToBase64(IBrowserFile file)
        {

            var max  = 1024 *1024 *10;

            IBrowserFile imgFile = file;
            var buffers = new byte[imgFile.Size];
            await imgFile.OpenReadStream(max).ReadAsync(buffers);
            return Convert.ToBase64String(buffers);
        }
    }
}
