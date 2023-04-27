using Fluxor;
using JobFileSystem.Shared.Estimates;
using JobFileSystem.Shared.LineItems;
using MudBlazor;

namespace Client.Features.Estimates
{
    public class CreateLineItemEffect : Effect<AddItemAction<LineItemDto>>
    {

        private readonly IEstimateClient _client;
        private readonly ISnackbar _snackbar;

        public CreateLineItemEffect(IEstimateClient client, ISnackbar snackbar)
        {
            _client = client;
            _snackbar = snackbar;
        }

        public override async Task HandleAsync(AddItemAction<LineItemDto> action, IDispatcher dispatcher)
        {
            try
            {

                var result = await _client.CreateLineItemAsync(action.ParentId, action.Item, default);

                dispatcher.Dispatch(new AddItemSuccessAction<LineItemDto>(result));
                _snackbar.Add($"Line Item added.", Severity.Success);
            }
            catch (Exception ex)
            {
                _snackbar.Add("Error processing request, please try again.", Severity.Error);
                dispatcher.Dispatch(new SetErrorAction(ex));
            }

        }

    }
}
