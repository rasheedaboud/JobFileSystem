using Fluxor;
using JobFileSystem.Shared.Estimates;
using MudBlazor;

namespace Client.Features.Estimates
{
    public class CreateEstimatesEffect : Effect<AddItemAction<EstimateDto>>
    {

        private readonly IEstimateClient _client;
        private readonly ISnackbar _snackbar;

        public CreateEstimatesEffect(IEstimateClient client, ISnackbar snackbar)
        {
            _client = client;
            _snackbar = snackbar;
        }

        public override async Task HandleAsync(AddItemAction<EstimateDto> action, IDispatcher dispatcher)
        {
            try
            {

                var result = await _client.CreateAsync(action.Item, default);

                dispatcher.Dispatch(new AddItemSuccessAction<EstimateDto>(result));
                _snackbar.Add($"Estimate added.", Severity.Success);
            }
            catch (Exception ex)
            {
                _snackbar.Add("Error processing request, please try again.", Severity.Error);
                dispatcher.Dispatch(new SetErrorAction(ex));
            }

        }

    }
}
