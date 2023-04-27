using Client.Features;
using Client.Features.Contacts;
using Client.Features.Estimates;
using Fluxor;
using Fluxor.Blazor.Web.Components;
using JobFileSystem.Client.Features.Estimates.Flux.Actions;
using JobFileSystem.Client.JsInterop;
using JobFileSystem.Shared.Enums;
using JobFileSystem.Shared.Estimates;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Popups;
using System.Net;

namespace JobFileSystem.Client.Features.Estimates.Components
{
    public partial class Edit : FluxorComponent
    {
        [Inject] protected IState<EstimateState> State { get; set; }
        [Inject] protected NavigationManager NavigationManager { get; set; }
        [Inject] protected IDispatcher Dispatcher { get; set; }
        [Inject] protected IEstimateClient EstimateClient { get; set; }
        [Parameter] public EventCallback OnSucess { get; set; }
        [Parameter] public EventCallback OnCancell { get; set; }
        [Parameter] public EstimateDto SelectedItem { get; set; } = new();
        private SfDialog _modal { get; set; }
        private CustomValidation _customValidation;
        private SfDialog _purchaseOrderModal { get; set; }
        private string PurchaseOrder { get; set; } = "";
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

                var response = await EstimateClient.Validate(SelectedItem, default);


                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    await _customValidation.DisplayErrors(response);
                }
                else
                {
                    if (string.IsNullOrEmpty(SelectedItem.Id))
                    {
                        await CreateEstimate();

                    }
                    else
                    {
                        await UpdateEstimate();
                    }
                }
                await _modal.HideAsync();

            }

        }

        private async Task CreateEstimate()
        {
            Dispatcher.Dispatch(new AddItemAction<EstimateDto>(SelectedItem));
            SelectedItem = State.Value.SelectedItem;
            await OnSucess.InvokeAsync();
        }

        private async Task UpdateEstimate()
        {
            if(await IfEstimateIsAcceptedAskForPoNumber())
            {
                return;
            };
            Dispatcher.Dispatch(new EditEstimate(SelectedItem,PurchaseOrder));

            await _purchaseOrderModal.HideAsync();

            while (State.Value.IsLoading)
            {
                await Task.Delay(50);
            }
            
            SelectedItem = State.Value.SelectedItem;
            if(SelectedItem.Status == EstimateStatus.Accepted.Name)
            {
                await _modal.HideAsync();
            }
            PurchaseOrder = string.Empty;
            await OnSucess.InvokeAsync();


            async Task<bool> IfEstimateIsAcceptedAskForPoNumber()
            {
                if (SelectedItem.Status == EstimateStatus.Accepted.Name &&
                    string.IsNullOrEmpty(PurchaseOrder))
                {
                    await _purchaseOrderModal.ShowAsync();
                    return true;
                }
                return false;
            }
        }

        private async Task OnClickBackButton(MouseEventArgs e)
        {
            Dispatcher.Dispatch(new DeSelectItemAction<EstimateDto>());
            await _modal.HideAsync();
            await OnCancell.InvokeAsync();

        }
    }
}
