using Client.Features.MaterialTestReports;
using Fluxor;
using JobFileSystem.Shared.DTOs;
using MudBlazor;

namespace Client.Features.MaterialTestReportss
{
    public class EditMaterialTestReportssEffect : Effect<EditItemAction<MaterialTestReportDto>>
    {

        private readonly IMaterialTestReportsClient _client;
        private readonly ISnackbar _snackbar;

        public EditMaterialTestReportssEffect(IMaterialTestReportsClient client, ISnackbar snackbar)
        {
            _client = client;
            _snackbar = snackbar;
        }

        public override async Task HandleAsync(EditItemAction<MaterialTestReportDto> action, IDispatcher dispatcher)
        {
            try
            {

                var result = await _client.UpdateAsync(action.Item, default);

                dispatcher.Dispatch(new EditItemSuccessAction<MaterialTestReportDto>(result));
                _snackbar.Add($"MTR updated.", Severity.Success);
            }
            catch (Exception e)
            {
                _snackbar.Add("Error processing request, please try again.", Severity.Error);
                dispatcher.Dispatch(new SetErrorAction(e));
            }

        }

    }
}
