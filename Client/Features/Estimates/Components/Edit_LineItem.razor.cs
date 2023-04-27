using Client.Features;
using Client.Features.Estimates;
using Fluxor;
using Fluxor.Blazor.Web.Components;
using JobFileSystem.Client.JsInterop;
using JobFileSystem.Shared.Estimates;
using JobFileSystem.Shared.LineItems;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Popups;
using System.Net;

namespace JobFileSystem.Client.Features.Estimates.Components
{
    public partial class Edit_LineItem : FluxorComponent
    {
        [Inject] protected IState<EstimateState> State { get; set; }
        [Inject] protected NavigationManager NavigationManager { get; set; }
        [Inject] protected IDispatcher Dispatcher { get; set; }
        [Inject] protected IEstimateClient EstimateClient { get; set; }
        [Inject] protected IJsMethods JsMethods { get; set; }
        [Parameter] public EventCallback OnSucess { get; set; }
        [Parameter] public EventCallback OnCancell { get; set; }
        [Parameter] public LineItemDto SelectedItem { get; set; } = new();
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

                var response = await EstimateClient.ValidateLineItem(SelectedItem, default);


                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    await _customValidation.DisplayErrors(response);
                }
                else
                {
                    if (string.IsNullOrEmpty(SelectedItem.Id))
                    {

                        Dispatcher.Dispatch(new AddItemAction<LineItemDto>(SelectedItem,State.Value.SelectedItem.Id));

                        while (State.Value.IsLoading)
                        {
                            await Task.Delay(50);
                        }

                        SelectedItem = new LineItemDto(); ;
                        await OnSucess.InvokeAsync();
                        await _modal.Hide();
                    }
                    else
                    {
                        Dispatcher.Dispatch(new EditItemAction<LineItemDto>(SelectedItem,State.Value.SelectedItem.Id));
                        while (State.Value.IsLoading)
                        {
                            await Task.Delay(50);
                        }                        
                        SelectedItem = new LineItemDto(); ;
                        await OnSucess.InvokeAsync();
                        await _modal.Hide();
                    }
                }

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
