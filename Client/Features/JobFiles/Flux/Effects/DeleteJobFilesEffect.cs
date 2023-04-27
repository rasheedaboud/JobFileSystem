using Fluxor;
using JobFileSystem.Client.Features.JobFiles;
using JobFileSystem.Shared.JobFiles;
using MudBlazor;

namespace Client.Features.JobFiles
{
    public class DeleteJobFilesEffect : Effect<DeleteItemAction<JobFileDto>>
    {

        private readonly IJobFileClient _client;
        private readonly ISnackbar _snackbar;

        public DeleteJobFilesEffect(IJobFileClient client, ISnackbar snackbar)
        {
            _client = client;
            _snackbar = snackbar;
        }

        public override async Task HandleAsync(DeleteItemAction<JobFileDto> action, IDispatcher dispatcher)
        {
            try
            {

                var result = await _client.DeleteAsync(action.Id, default);

                dispatcher.Dispatch(new DeleteItemSuccessAction<JobFileDto>(result));
                _snackbar.Add($"Job File removed.", Severity.Success);
            }
            catch (Exception e)
            {
                _snackbar.Add("Error processing request, please try again.", Severity.Error);
                dispatcher.Dispatch(new SetErrorAction(e));
            }

        }

    }
}
