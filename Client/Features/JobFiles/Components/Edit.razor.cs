using Client.Features;
using Client.Features.JobFiles;
using Fluxor;
using Fluxor.Blazor.Web.Components;
using JobFileSystem.Client.JsInterop;
using JobFileSystem.Client.Utils;
using JobFileSystem.Shared.JobFiles;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Popups;
using System.Net;

namespace JobFileSystem.Client.Features.JobFiles.Components
{
    public partial class Edit : FluxorComponent
    {
        [Inject] protected IState<JobFileState> State { get; set; }
        [Inject] protected IDispatcher Dispatcher { get; set; }
        [Inject] protected NavigationManager NavigationManager { get; set; }
        [Inject] protected IJobFileClient JobFileClient { get; set; }
        [Inject] protected IJsMethods JsMethods { get; set; }
        [Parameter] public EventCallback OnSucess { get; set; }
        [Parameter] public JobFileDto JobFile { get; set; } = new();
        private SfDialog _modal { get; set; }
        private CustomValidation _customValidation;

        public async Task ShowModal() => await _modal.ShowAsync();

        protected override async Task OnInitializedAsync()
        {
            if (JobFile is null)
            {
                JobFile = new();
            }
        }

        private async Task HandleValidSubmit()
        {
            if (!State.Value.IsLoading)
            {
                _customValidation.ClearErrors();

                var response = await JobFileClient.Validate(JobFile, default);


                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    await _customValidation.DisplayErrors(response);
                }
                else
                {
                    if (string.IsNullOrEmpty(JobFile.Id))
                    {

                        Dispatcher.Dispatch(new AddItemAction<JobFileDto>(JobFile));
                        JobFile = new JobFileDto(); ;
                        await OnSucess.InvokeAsync();
                        await _modal.Hide();

                    }
                    else
                    {
                        Dispatcher.Dispatch(new EditItemAction<JobFileDto>(JobFile));
                        JobFile = new JobFileDto(); ;
                        await OnSucess.InvokeAsync();
                        await _modal.Hide();
                    }
                }

            }

        }

        private async Task OnClickBackButton(MouseEventArgs e)
        {
            Dispatcher.Dispatch(new DeSelectItemAction<JobFileDto>());
            await _modal.HideAsync();

        }

        private async Task UploadFiles(InputFileChangeEventArgs args)
        {
            foreach (var file in args.GetMultipleFiles())
            {
                var base64 = await Files.ToBase64(file);
                JobFile.Attachments.Add(new()
                {
                    //Base64EncodedFile = base64,
                    ContentType = file.ContentType,
                    FileExtention = Path.GetFileNameWithoutExtension(file.Name),
                    FileName=file.Name,                    
                });
            }
            
        }


    }
}
