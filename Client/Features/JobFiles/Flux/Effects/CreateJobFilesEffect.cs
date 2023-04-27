using Fluxor;
using JobFileSystem.Client.Features.JobFiles;
using JobFileSystem.Shared.JobFiles;
using MudBlazor;

namespace Client.Features.JobFiles
{
    public class CreateJobFilesEffect : Effect<AddItemAction<JobFileDto>>
    {

        private readonly IJobFileClient _client;
        private readonly ISnackbar _snackbar;

        public CreateJobFilesEffect(IJobFileClient client, ISnackbar snackbar)
        {
            _client = client;
            _snackbar = snackbar;
        }

        public override async Task HandleAsync(AddItemAction<JobFileDto> action, IDispatcher dispatcher)
        {
            try
            {

                var result = await _client.CreateAsync(action.Item, default);

                dispatcher.Dispatch(new AddItemSuccessAction<JobFileDto>(result));
                _snackbar.Add($"Job File: {result.Number} added.", Severity.Success);
            }
            catch (Exception ex)
            {
                _snackbar.Add("Error processing request, please try again.", Severity.Error);
                dispatcher.Dispatch(new SetErrorAction(ex));
            }

        }

    }
}
