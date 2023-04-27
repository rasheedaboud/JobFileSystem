using Fluxor;
using JobFileSystem.Client.Features.Attachments.Actions;
using JobFileSystem.Shared.Estimates;
using MudBlazor;

namespace Client.Features.Estimates
{
    public class AttachmentAddedToLineItemEffect : Effect<AttachmentAddedToLineItem>
    {

        private readonly IEstimateClient _client;
        private readonly ISnackbar _snackbar;

        public AttachmentAddedToLineItemEffect(IEstimateClient client, ISnackbar snackbar)
        {
            _client = client;
            _snackbar = snackbar;
        }

        public override async Task HandleAsync(AttachmentAddedToLineItem action, IDispatcher dispatcher)
        {
            try
            {

                var result = await _client.GetByIdAsync(action.estimateId, default);

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
