using Fluxor;
using JobFileSystem.Client.Features.Estimates.Flux.Actions;
using JobFileSystem.Shared.Estimates;
using MudBlazor;

namespace Client.Features.Estimates
{
    public class EditEstimatesEffect : Effect<EditEstimate>
    {

        private readonly IEstimateClient _client;
        private readonly ISnackbar _snackbar;

        public EditEstimatesEffect(IEstimateClient client, ISnackbar snackbar)
        {
            _client = client;
            _snackbar = snackbar;
        }

        public override async Task HandleAsync(EditEstimate action, IDispatcher dispatcher)
        {
            try
            {
                var result = await _client.UpdateAsync(action.dto,action.purchaseOrderNumber, default);

                dispatcher.Dispatch(new EditEstimateSuccess(result));
                _snackbar.Add($"Estimate updated.", Severity.Success);
            }
            catch (Exception e)
            {
                _snackbar.Add("Error processing request, please try again.", Severity.Error);
                dispatcher.Dispatch(new SetErrorAction(e));
            }

        }

    }
}
