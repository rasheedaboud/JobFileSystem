using Client.Features;
using Client.Features.Contacts;
using Fluxor;
using JobFileSystem.Client.JsInterop;
using JobFileSystem.Shared.Contacts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Popups;
using System.Net;

namespace JobFileSystem.Client.Features.Contacts.Components
{
    public partial class Edit
    {
        [Inject] protected IState<ContactState> State { get; set; }
        [Inject] protected IDispatcher Dispatcher { get; set; }
        [Inject] protected IContactClient ContactClient { get; set; }
        [Inject] protected IJsMethods JsMethods { get; set; }
        [Parameter] public EventCallback OnSucess { get; set; }
        [Parameter] public EventCallback OnCancel { get; set; }
        [Parameter] public ContactDto SelectedItem { get; set; } = new();
        private SfDialog _modal { get; set; }
        private CustomValidation _customValidation;

        public async Task ShowModal() => await _modal.ShowAsync();

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            if (SelectedItem == null)
            {
                SelectedItem = new ContactDto();
            }
        }

        protected override async Task OnInitializedAsync()
        {
            if (SelectedItem == null)
            {
                SelectedItem = new ContactDto();
            }
        }

        private async Task HandleValidSubmit()
        {
            if (!State.Value.IsLoading)
            {
                _customValidation.ClearErrors();

                var response = await ContactClient.Validate(SelectedItem, default);


                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    await _customValidation.DisplayErrors(response);
                }
                else
                {
                    if (string.IsNullOrEmpty(SelectedItem.Id))
                    {

                        Dispatcher.Dispatch(new AddItemAction<ContactDto>(SelectedItem));
                        SelectedItem = new ContactDto(); ;
                        await OnSucess.InvokeAsync();
                        await _modal.Hide();

                    }
                    else
                    {
                        Dispatcher.Dispatch(new EditItemAction<ContactDto>(SelectedItem));
                        SelectedItem = new ContactDto(); ;
                        await OnSucess.InvokeAsync();
                        await _modal.Hide();
                    }
                }

            }

        }

        private async Task OnClickBackButton(MouseEventArgs e)
        {
            Dispatcher.Dispatch(new DeSelectItemAction<ContactDto>());
            await _modal.HideAsync();
            await OnCancel.InvokeAsync();
        }
    }
}
