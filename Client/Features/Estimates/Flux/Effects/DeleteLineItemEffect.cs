using Fluxor;
using JobFileSystem.Shared.LineItems;
using MudBlazor;

namespace Client.Features.Estimates
{
    public class DeleteLineItemEffect : Effect<DeleteItemAction<LineItemDto>>
    {

        private readonly IEstimateClient _client;
        private readonly ISnackbar _snackbar;

        public DeleteLineItemEffect(IEstimateClient client, ISnackbar snackbar)
        {
            _client = client;
            _snackbar = snackbar;
        }

        public override async Task HandleAsync(DeleteItemAction<LineItemDto> action, IDispatcher dispatcher)
        {
            try
            {

                var result = await _client.DeleteLineItemAsync(action.ParentId,action.Id, default);

                dispatcher.Dispatch(new DeleteItemSuccessAction<LineItemDto>(result, action.Id));
                _snackbar.Add($"Line Item was removed.", Severity.Success);
            }
            catch (Exception e)
            {
                _snackbar.Add("Error processing request, please try again.", Severity.Error);
                dispatcher.Dispatch(new SetErrorAction(e));
            }

        }

    }
}
