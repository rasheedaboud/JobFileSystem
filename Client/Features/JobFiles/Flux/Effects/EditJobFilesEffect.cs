using Fluxor;
using JobFileSystem.Client.Features.JobFiles;
using JobFileSystem.Shared.JobFiles;
using MudBlazor;

namespace Client.Features.JobFiles
{
    public class EditJobFilesEffect : Effect<EditItemAction<JobFileDto>>
    {

        private readonly IJobFileClient _client;
        private readonly ISnackbar _snackbar;

        public EditJobFilesEffect(IJobFileClient client, ISnackbar snackbar)
        {
            _client = client;
            _snackbar = snackbar;
        }

        public override async Task HandleAsync(EditItemAction<JobFileDto> action, IDispatcher dispatcher)
        {
            try
            {

                var result = await _client.UpdateAsync(action.Item, default);

                dispatcher.Dispatch(new EditItemSuccessAction<JobFileDto>(result));
                _snackbar.Add($"Job File: {result.Number} updated.", Severity.Success);
            }
            catch (Exception e)
            {
                _snackbar.Add("Error processing request, please try again.", Severity.Error);
                dispatcher.Dispatch(new SetErrorAction(e));
            }

        }

    }
}
