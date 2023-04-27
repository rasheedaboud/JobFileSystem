using Fluxor;
using JobFileSystem.Shared.Estimates;
using MudBlazor;

namespace Client.Features.Estimates
{
    public class DeleteEstimatesEffect : Effect<DeleteItemAction<EstimateDto>>
    {

        private readonly IEstimateClient _client;
        private readonly ISnackbar _snackbar;

        public DeleteEstimatesEffect(IEstimateClient client, ISnackbar snackbar)
        {
            _client = client;
            _snackbar = snackbar;
        }

        public override async Task HandleAsync(DeleteItemAction<EstimateDto> action, IDispatcher dispatcher)
        {
            try
            {

                var result = await _client.DeleteAsync(action.Id, default);

                dispatcher.Dispatch(new DeleteItemSuccessAction<EstimateDto>(result));
                _snackbar.Add($"Estimate was removed.", Severity.Success);
            }
            catch (Exception e)
            {
                _snackbar.Add("Error processing request, please try again.", Severity.Error);
                dispatcher.Dispatch(new SetErrorAction(e));
            }

        }

    }
}
