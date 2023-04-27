using JobFileSystem.Shared.Contacts;
using System.Net.Http.Json;
using System.Text.Json;

namespace Client.Features.Contacts
{

    public static class ContactRoutes
    {
        public static string GetAll(string baseUri) => $"{baseUri}odata/ContactsOdata";
        public static string Create => $"api/Contacts/create";
        public static string Update => $"api/Contacts/update";
        public static string Validate => $"api/Contacts/validate";
        public static string Delete(string id) => $"api/Contacts/delete?id={id}";
    }

    public interface IContactClient
    {
        Task<ContactDto> CreateAsync(ContactDto Contact,
                                     CancellationToken token);
        Task<ContactDto> UpdateAsync(ContactDto Contact,
                                     CancellationToken token);
        Task<bool> DeleteAsync(string id,
                                     CancellationToken token);
        Task<HttpResponseMessage> Validate(ContactDto ContactDto, CancellationToken token);
    }

    public class ContactClient : IContactClient
    {
        private readonly HttpClient _client;

        public ContactClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<HttpResponseMessage> Validate(ContactDto ContactDto, CancellationToken token)
        {
            return await _client.PostAsJsonAsync(ContactRoutes.Validate, ContactDto, token);
        }

        public async Task<ContactDto> CreateAsync(ContactDto Contact,
                                                  CancellationToken token)
        {
            
            var result = await _client.PostAsJsonAsync(ContactRoutes.Create,Contact,token);

            result.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<ContactDto>(await result.Content.ReadAsStringAsync());
            
        }
        public async Task<ContactDto> UpdateAsync(ContactDto Contact,
                                          CancellationToken token)
        {

            var result = await _client.PutAsJsonAsync(ContactRoutes.Update, Contact, token);

            result.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<ContactDto>(await result.Content.ReadAsStringAsync());

        }

        public async Task<bool> DeleteAsync(string id, CancellationToken token)
        {
            var result = await _client.DeleteAsync(ContactRoutes.Delete(id), token);

            return result.IsSuccessStatusCode;
        }
    }
}
