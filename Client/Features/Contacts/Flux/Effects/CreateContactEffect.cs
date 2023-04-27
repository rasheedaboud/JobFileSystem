using Fluxor;
using JobFileSystem.Shared.Contacts;
using MudBlazor;

namespace Client.Features.Contacts
{
    public class CreateContactsEffect : Effect<AddItemAction<ContactDto>>
    {

        private readonly IContactClient _client;
        private readonly ISnackbar _snackbar;

        public CreateContactsEffect(IContactClient client, ISnackbar snackbar)
        {
            _client = client;
            _snackbar = snackbar;
        }

        public override async Task HandleAsync(AddItemAction<ContactDto> action, IDispatcher dispatcher)
        {
            try
            {

                var result = await _client.CreateAsync(action.Item, default);

                dispatcher.Dispatch(new AddItemSuccessAction<ContactDto>(result));
                _snackbar.Add($"Contact: {result.Name} added.", Severity.Success);
            }
            catch (Exception ex)
            {
                _snackbar.Add("Error processing request, please try again.", Severity.Error);
                dispatcher.Dispatch(new SetErrorAction(ex));
            }

        }

    }
}
