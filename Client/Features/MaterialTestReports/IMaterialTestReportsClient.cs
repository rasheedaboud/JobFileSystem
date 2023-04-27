using JobFileSystem.Shared.DTOs;
using JobFileSystem.Shared.LineItems;
using System.Net.Http.Json;
using System.Text.Json;

namespace Client.Features.MaterialTestReports
{

    public static class MaterialTestReportRoutes
    {
        public static string GetAll(string baseUri) => $"{baseUri}odata/MaterialTestReportsOdata?$expand=Attachments";
        public static string GetByIdAll(string MaterialTestReportId) => $"api/MaterialTestReports/getbyid?materialTestReportId={MaterialTestReportId}";
        public static string Create => $"api/MaterialTestReports/create";
        public static string Update => $"api/MaterialTestReports/update";
        public static string Validate => $"api/MaterialTestReports/validate";
        public static string Delete(string id) => $"api/MaterialTestReports/delete?id={id}";
        public static string RemoveAttachmentFromMaterialTestReport(string MaterialTestReportId) => $"api/MaterialTestReports/removeFile?materialTestReportId={MaterialTestReportId}";
        public static string AddAttachmentToMaterialTestReport(string MaterialTestReportId) => $"api/MaterialTestReports/uploadFile?materialTestReportId={MaterialTestReportId}";

    }

    public interface IMaterialTestReportsClient
    {
        Task<MaterialTestReportDto> CreateAsync(MaterialTestReportDto MaterialTestReport,
                                     CancellationToken token);
        Task<MaterialTestReportDto> GetByIdAsync(string MaterialTestReportId,
                                     CancellationToken token);

        Task<MaterialTestReportDto> UpdateAsync(MaterialTestReportDto MaterialTestReport,
                                     CancellationToken token);
        Task<bool> DeleteAsync(string id,
                                     CancellationToken token);
        Task<HttpResponseMessage> Validate(MaterialTestReportDto MaterialTestReportDto, CancellationToken token);
    }

    public class MaterialTestReportsClient : IMaterialTestReportsClient
    {
        private readonly HttpClient _client;

        public MaterialTestReportsClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<HttpResponseMessage> Validate(MaterialTestReportDto MaterialTestReportDto, CancellationToken token)
        {
            return await _client.PostAsJsonAsync(MaterialTestReportRoutes.Validate, MaterialTestReportDto, token);
        }

        public async Task<MaterialTestReportDto> CreateAsync(MaterialTestReportDto MaterialTestReport,
                                                  CancellationToken token)
        {

            var result = await _client.PostAsJsonAsync(MaterialTestReportRoutes.Create, MaterialTestReport, token);

            result.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<MaterialTestReportDto>(await result.Content.ReadAsStringAsync());

        }
        public async Task<MaterialTestReportDto> UpdateAsync(MaterialTestReportDto MaterialTestReport,
                                          CancellationToken token)
        {

            var result = await _client.PutAsJsonAsync(MaterialTestReportRoutes.Update, MaterialTestReport, token);

            result.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<MaterialTestReportDto>(await result.Content.ReadAsStringAsync());

        }

        public async Task<bool> DeleteAsync(string id, CancellationToken token)
        {
            var result = await _client.DeleteAsync(MaterialTestReportRoutes.Delete(id), token);

            return result.IsSuccessStatusCode;
        }

       

        public async Task<MaterialTestReportDto> GetByIdAsync(string MaterialTestReportId, CancellationToken token)
        {
            var result = await _client.GetFromJsonAsync<MaterialTestReportDto>(MaterialTestReportRoutes.GetByIdAll(MaterialTestReportId), token);

            if (result == null) throw new NotFoundException();

            return result;
        }
    }
}
