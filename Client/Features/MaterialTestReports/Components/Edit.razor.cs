using Client.Features;
using Client.Features.MaterialTestReports;
using Fluxor;
using JobFileSystem.Shared.DTOs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Popups;
using System.Net;

namespace JobFileSystem.Client.Features.MaterialTestReports.Components
{
    public partial class Edit
    {
        [Inject] protected IState<MaterialTestReportState> State { get; set; }
        [Inject] protected NavigationManager NavigationManager { get; set; }
        [Inject] protected IDispatcher Dispatcher { get; set; }
        [Inject] protected IMaterialTestReportsClient MaterialTestReportsClient { get; set; }
        [Parameter] public EventCallback OnSucess { get; set; }
        [Parameter] public EventCallback OnCancel { get; set; }
        [Parameter] public MaterialTestReportDto SelectedItem { get; set; }

        private SfDialog _modal { get; set; }
        private CustomValidation _customValidation;

        public async Task ShowModal() => await _modal.ShowAsync();

        protected override async Task OnInitializedAsync()
        {
            if (SelectedItem is null)
            {
                SelectedItem = new();
            }
        }

        private async Task HandleValidSubmit()
        {
            if (!State.Value.IsLoading)
            {

                _customValidation.ClearErrors();

                var response = await MaterialTestReportsClient.Validate(SelectedItem, default);


                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    await _customValidation.DisplayErrors(response);
                }
                else
                {
                    if (string.IsNullOrEmpty(SelectedItem.Id))
                    {

                        Dispatcher.Dispatch(new AddItemAction<MaterialTestReportDto>(SelectedItem));
                        await WaitDispatchSuccess();
                    }
                    else
                    {
                        Dispatcher.Dispatch(new EditItemAction<MaterialTestReportDto>(SelectedItem));

                        await WaitDispatchSuccess();
                    }
                }

            }

        }

        private async Task WaitDispatchSuccess()
        {
            while (State.Value.IsLoading)
            {
                await Task.Delay(50);
            }
            SelectedItem = State.Value.SelectedItem ?? new();
            await OnSucess.InvokeAsync();
            StateHasChanged();
        }

        private async Task OnClickBackButton(MouseEventArgs e)
        {
            Dispatcher.Dispatch(new DeSelectItemAction<MaterialTestReportDto>());
            await _modal.HideAsync();
            await OnCancel.InvokeAsync();

        }
    }
}
