using Fluxor;
using JobFileSystem.Shared.Contacts;
using MudBlazor;

namespace Client.Features.Contacts
{
    public class EditContactsEffect : Effect<EditItemAction<ContactDto>>
    {

        private readonly IContactClient _client;
        private readonly ISnackbar _snackbar;

        public EditContactsEffect(IContactClient client, ISnackbar snackbar)
        {
            _client = client;
            _snackbar = snackbar;
        }

        public override async Task HandleAsync(EditItemAction<ContactDto> action, IDispatcher dispatcher)
        {
            try
            {

                var result = await _client.UpdateAsync(action.Item, default);

                dispatcher.Dispatch(new EditItemSuccessAction<ContactDto>(result));
                _snackbar.Add($"Contact: {result.Name} updated.", Severity.Success);
            }
            catch (Exception e)
            {
                _snackbar.Add("Error processing request, please try again.", Severity.Error);
                dispatcher.Dispatch(new SetErrorAction(e));
            }

        }

    }
}
