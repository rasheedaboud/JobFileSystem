using Fluxor;
using JobFileSystem.Shared.Estimates;
using MudBlazor;

namespace Client.Features.Estimates
{
    public class AttachmentAddedToEstimateEffect : Effect<AttachmentAddedAction<EstimateDto>>
    {

        private readonly IEstimateClient _client;
        private readonly ISnackbar _snackbar;

        public AttachmentAddedToEstimateEffect(IEstimateClient client, ISnackbar snackbar)
        {
            _client = client;
            _snackbar = snackbar;
        }

        public override async Task HandleAsync(AttachmentAddedAction<EstimateDto> action, IDispatcher dispatcher)
        {
            try
            {

                var result = await _client.GetByIdAsync(action.Id, default);

                dispatcher.Dispatch(new AttachmentAddedSuccessAction<EstimateDto>(result));
                _snackbar.Add($"Attachment added.", Severity.Success);
            }
            catch (Exception ex)
            {
                _snackbar.Add("Error processing request, please try again.", Severity.Error);
                dispatcher.Dispatch(new SetErrorAction(ex));
            }

        }

    }
}
