using Client.Features.MaterialTestReports;
using Fluxor;
using JobFileSystem.Shared.DTOs;
using MudBlazor;

namespace Client.Features.MaterialTestReportss
{
    public class CreateMaterialTestReportssEffect : Effect<AddItemAction<MaterialTestReportDto>>
    {

        private readonly IMaterialTestReportsClient _client;
        private readonly ISnackbar _snackbar;

        public CreateMaterialTestReportssEffect(IMaterialTestReportsClient client, ISnackbar snackbar)
        {
            _client = client;
            _snackbar = snackbar;
        }

        public override async Task HandleAsync(AddItemAction<MaterialTestReportDto> action, IDispatcher dispatcher)
        {
            try
            {

                var result = await _client.CreateAsync(action.Item, default);

                dispatcher.Dispatch(new AddItemSuccessAction<MaterialTestReportDto>(result));
                _snackbar.Add($"MRT was added.", Severity.Success);
            }
            catch (Exception ex)
            {
                _snackbar.Add("Error processing request, please try again.", Severity.Error);
                dispatcher.Dispatch(new SetErrorAction(ex));
            }

        }

    }
}
