using Client.Features.MaterialTestReports;
using Fluxor;
using JobFileSystem.Shared.DTOs;
using MudBlazor;

namespace Client.Features.MaterialTestReportss
{
    public class DeleteMaterialTestReportssEffect : Effect<DeleteItemAction<MaterialTestReportDto>>
    {

        private readonly IMaterialTestReportsClient _client;
        private readonly ISnackbar _snackbar;

        public DeleteMaterialTestReportssEffect(IMaterialTestReportsClient client, ISnackbar snackbar)
        {
            _client = client;
            _snackbar = snackbar;
        }

        public override async Task HandleAsync(DeleteItemAction<MaterialTestReportDto> action, IDispatcher dispatcher)
        {
            try
            {

                var result = await _client.DeleteAsync(action.Id, default);

                dispatcher.Dispatch(new DeleteItemSuccessAction<MaterialTestReportDto>(result));
                _snackbar.Add($"MTR was removed.", Severity.Success);
            }
            catch (Exception e)
            {
                _snackbar.Add("Error processing request, please try again.", Severity.Error);
                dispatcher.Dispatch(new SetErrorAction(e));
            }

        }

    }
}
