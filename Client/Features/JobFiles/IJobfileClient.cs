using JobFileSystem.Shared.JobFiles;
using System.Net.Http.Json;
using System.Text.Json;

namespace JobFileSystem.Client.Features.JobFiles
{

    public static class JobFileRoutes
    {
        public static string GetAll(string baseUri) => $"{baseUri}odata/JobFilesOdata?$expand=Attachments";
        public static string Create => $"api/JobFiles/create";
        public static string Update => $"api/JobFiles/update";
        public static string Validate => $"api/JobFiles/validate";
        public static string Delete(string id) => $"api/JobFiles/delete?id={id}";
    }

    public interface IJobFileClient
    {
        Task<JobFileDto> CreateAsync(JobFileDto JobFile,
                                     CancellationToken token);
        Task<JobFileDto> UpdateAsync(JobFileDto JobFile,
                                     CancellationToken token);
        Task<bool> DeleteAsync(string id,
                                     CancellationToken token);
        Task<HttpResponseMessage> Validate(JobFileDto JobFileDto, CancellationToken token);
    }

    public class JobFileClient : IJobFileClient
    {
        private readonly HttpClient _client;

        public JobFileClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<HttpResponseMessage> Validate(JobFileDto JobFileDto, CancellationToken token)
        {
            return await _client.PostAsJsonAsync(JobFileRoutes.Validate, JobFileDto, token);
        }

        public async Task<JobFileDto> CreateAsync(JobFileDto JobFile,
                                                  CancellationToken token)
        {

            var result = await _client.PostAsJsonAsync(JobFileRoutes.Create, JobFile, token);

            result.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<JobFileDto>(await result.Content.ReadAsStringAsync());

        }
        public async Task<JobFileDto> UpdateAsync(JobFileDto JobFile,
                                          CancellationToken token)
        {

            var result = await _client.PutAsJsonAsync(JobFileRoutes.Update, JobFile, token);

            result.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<JobFileDto>(await result.Content.ReadAsStringAsync());

        }

        public async Task<bool> DeleteAsync(string id, CancellationToken token)
        {
            var result = await _client.DeleteAsync(JobFileRoutes.Delete(id), token);

            return result.IsSuccessStatusCode;
        }
    }
}
