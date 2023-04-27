using Fluxor;
using JobFileSystem.Shared.Contacts;
using MudBlazor;

namespace Client.Features.Contacts
{
    public class DeleteContactsEffect : Effect<DeleteItemAction<ContactDto>>
    {

        private readonly IContactClient _client;
        private readonly ISnackbar _snackbar;

        public DeleteContactsEffect(IContactClient client, ISnackbar snackbar)
        {
            _client = client;
            _snackbar = snackbar;
        }

        public override async Task HandleAsync(DeleteItemAction<ContactDto> action, IDispatcher dispatcher)
        {
            try
            {

                var result = await _client.DeleteAsync(action.Id, default);

                dispatcher.Dispatch(new DeleteItemSuccessAction<ContactDto>(result));
                _snackbar.Add($"Contact was removed.", Severity.Success);
            }
            catch (Exception e)
            {
                _snackbar.Add("Error processing request, please try again.", Severity.Error);
                dispatcher.Dispatch(new SetErrorAction(e));
            }

        }

    }
}
