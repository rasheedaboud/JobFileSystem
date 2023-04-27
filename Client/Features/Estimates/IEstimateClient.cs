using JobFileSystem.Shared.Estimates;
using JobFileSystem.Shared.LineItems;
using System.Net.Http.Json;
using System.Text.Json;

namespace Client.Features.Estimates
{

    public static class EstimateRoutes
    {
        public static string GetAll(string baseUri) => $"{baseUri}odata/EstimatesOdata?$expand=Client,LineItems($expand=Attachments),Attachments";
        public static string GetByIdAll(string estimateId) => $"api/Estimates/getbyid?estimateId={estimateId}";
        public static string Create => $"api/Estimates/create";
        public static string Update(string purchaseOrderNumber = null) => $"api/Estimates/update?purchaseOrderNumber={purchaseOrderNumber}";
        public static string Validate => $"api/Estimates/validate";
        public static string ValidateLineItem => $"api/Estimates/validatelineitem";
        public static string AddLineItem(string estimateId) => $"api/Estimates/addlineitem?estimateId={estimateId}";
        public static string DeleteLineItem(string estimateId, string lineItemId) => $"api/Estimates/deletelineitem?estimateId={estimateId}&lineItemId={lineItemId}";
        public static string UpdateLineItem(string estimateId) => $"api/Estimates/updatelineitem?estimateId={estimateId}";
        public static string Delete(string id) => $"api/Estimates/delete?id={id}";
        public static string RemoveAttachmentFromEstimate(string estimateId) => $"api/Estimates/removeFile?estimateId={estimateId}";
        public static string AddAttachmentToEstimate(string estimateId) => $"api/Estimates/uploadFile?estimateId={estimateId}";

        public static string RemoveAttachmentToLineItem(string estimateId, string lineItemId) => $"api/Estimates/removeFileLineItem?estimateId={estimateId}&lineItemId={lineItemId}";
        public static string AddAttachmentToLineItem(string lineItemId, string estimateId) => $"api/Estimates/uploadFileLineItem?lineItemId={lineItemId}&estimateId={estimateId}";
    }

    public interface IEstimateClient
    {
        Task<EstimateDto> CreateAsync(EstimateDto Estimate,
                                     CancellationToken token);
        Task<EstimateDto> GetByIdAsync(string estimateId,
                                     CancellationToken token);
        Task<LineItemDto> CreateLineItemAsync(string estimateId, LineItemDto lineItem,
                                     CancellationToken token);
        Task<LineItemDto> UpdateLineItemAsync(string estimateId, LineItemDto lineItem,
                                     CancellationToken token);
        Task<bool> DeleteLineItemAsync(string estimateId, string lineItemId,
                                     CancellationToken token);
        Task<EstimateDto> UpdateAsync(EstimateDto Estimate,
                                      string purchaseOrderNumber,
                                      CancellationToken token);
        Task<bool> DeleteAsync(string id,
                                     CancellationToken token);
        Task<HttpResponseMessage> Validate(EstimateDto EstimateDto, CancellationToken token);
        Task<HttpResponseMessage> ValidateLineItem(LineItemDto EstimateDto, CancellationToken token);
    }

    public class EstimateClient : IEstimateClient
    {
        private readonly HttpClient _client;

        public EstimateClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<HttpResponseMessage> Validate(EstimateDto EstimateDto, CancellationToken token)
        {
            return await _client.PostAsJsonAsync(EstimateRoutes.Validate, EstimateDto, token);
        }

        public async Task<EstimateDto> CreateAsync(EstimateDto Estimate,
                                                  CancellationToken token)
        {

            var result = await _client.PostAsJsonAsync(EstimateRoutes.Create, Estimate, token);

            result.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<EstimateDto>(await result.Content.ReadAsStringAsync());

        }
        public async Task<EstimateDto> UpdateAsync(EstimateDto Estimate,
                                                   string purchaseOrderNumber,
                                                   CancellationToken token)
        {

            var result = await _client.PutAsJsonAsync(EstimateRoutes.Update(purchaseOrderNumber), Estimate, token);

            result.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<EstimateDto>(await result.Content.ReadAsStringAsync());

        }

        public async Task<bool> DeleteAsync(string id, CancellationToken token)
        {
            var result = await _client.DeleteAsync(EstimateRoutes.Delete(id), token);

            return result.IsSuccessStatusCode;
        }

        public async Task<HttpResponseMessage> ValidateLineItem(LineItemDto dto, CancellationToken token)
        {
            return await _client.PostAsJsonAsync(EstimateRoutes.ValidateLineItem, dto, token);
        }

        public async Task<LineItemDto> CreateLineItemAsync(string estimateId, LineItemDto lineItem, CancellationToken token)
        {
            var result = await _client.PostAsJsonAsync(EstimateRoutes.AddLineItem(estimateId), lineItem, token);

            result.EnsureSuccessStatusCode();

            var test = await result.Content.ReadAsStringAsync();

            var item = JsonSerializer.Deserialize<LineItemDto>(test);

            return item;

        }

        public async Task<LineItemDto> UpdateLineItemAsync(string estimateId, LineItemDto lineItem, CancellationToken token)
        {
            var result = await _client.PutAsJsonAsync(EstimateRoutes.UpdateLineItem(estimateId), lineItem, token);

            result.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<LineItemDto>(await result.Content.ReadAsStringAsync());
        }

        public async Task<bool> DeleteLineItemAsync(string estimateId, string lineItemId, CancellationToken token)
        {
            var result = await _client.DeleteAsync(EstimateRoutes.DeleteLineItem(estimateId, lineItemId), token);

            result.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<bool>(await result.Content.ReadAsStringAsync());
        }

        public async Task<EstimateDto> GetByIdAsync(string estimateId, CancellationToken token)
        {
            var result = await _client.GetFromJsonAsync<EstimateDto>(EstimateRoutes.GetByIdAll(estimateId), token);

            if (result == null) throw new NotFoundException();

            return result;
        }
    }
}
