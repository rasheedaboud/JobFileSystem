using Fluxor;
using JobFileSystem.Shared.Estimates;
using JobFileSystem.Shared.LineItems;
using MudBlazor;

namespace Client.Features.Estimates
{
    public class UpdateLineItemEffect : Effect<EditItemAction<LineItemDto>>
    {

        private readonly IEstimateClient _client;
        private readonly ISnackbar _snackbar;

        public UpdateLineItemEffect(IEstimateClient client, ISnackbar snackbar)
        {
            _client = client;
            _snackbar = snackbar;
        }

        public override async Task HandleAsync(EditItemAction<LineItemDto> action, IDispatcher dispatcher)
        {
            try
            {

                var result = await _client.UpdateLineItemAsync(action.ParentId, action.Item, default);

                dispatcher.Dispatch(new EditItemSuccessAction<LineItemDto>(result));
                _snackbar.Add($"Line Item updated.", Severity.Success);
            }
            catch (Exception ex)
            {
                _snackbar.Add("Error processing request, please try again.", Severity.Error);
                dispatcher.Dispatch(new SetErrorAction(ex));
            }

        }

    }
}
