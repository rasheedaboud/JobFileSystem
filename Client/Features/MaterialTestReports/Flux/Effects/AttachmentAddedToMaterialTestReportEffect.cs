using Client.Features.MaterialTestReports;
using Fluxor;
using JobFileSystem.Client.Features.Attachments.Actions;
using JobFileSystem.Shared.DTOs;
using MudBlazor;

namespace Client.Features.MaterialTestReportss
{
    public class AttachmentAddedToMaterialTestReportsEffect : Effect<AttachmentAddedAction<MaterialTestReportDto>>
    {

        private readonly IMaterialTestReportsClient _client;
        private readonly ISnackbar _snackbar;

        public AttachmentAddedToMaterialTestReportsEffect(IMaterialTestReportsClient client, ISnackbar snackbar)
        {
            _client = client;
            _snackbar = snackbar;
        }

        public override async Task HandleAsync(AttachmentAddedAction<MaterialTestReportDto> action, IDispatcher dispatcher)
        {
            try
            {

                var result = await _client.GetByIdAsync(action.Id, default);

                dispatcher.Dispatch(new AttachmentAddedSuccessAction<MaterialTestReportDto>(result));
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
