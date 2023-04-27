using Client.Features;
using Client.Features.Contacts;
using Fluxor;
using JobFileSystem.Client.Components;
using JobFileSystem.Client.Features.Contacts.Components;
using JobFileSystem.Shared.Contacts;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Navigations;

namespace JobFileSystem.Client.Features.Contacts.Pages
{
    public partial class Index
    {

        [Inject] protected NavigationManager NavigationManager { get; set; }
        [Inject] protected IState<ContactState> State { get; set; }
        [Inject] protected IDispatcher Dispatcher { get; set; }
        private List<object> _toolBarEdit = new Toolbar().Add()
                                                     .Edit()
                                                     .Delete()
                                                     .Refresh()
                                                     .Build();

        private List<object> _toolBar = new Toolbar().Add()
                                                     .Refresh()
                                                     .Build();
        private SfGrid<ContactDto> _grid;

        private JobFileSystem.Client.Features.Contacts.Components.Edit _edit { get; set; }
        private ConfirmDelete _confirmDelete;
        private async Task OnToolbarClickHandler(ClickEventArgs args)
        {
            if (args.Item.Id == Buttons.Refresh)
            {
                _grid.Refresh();
            }
            if (args.Item.Id == Buttons.Edit)
            {
                args.Cancel = true;
                await _edit.ShowModal();
            }
            if (args.Item.Id == Buttons.Delete)
            {
                args.Cancel = true;
                await _confirmDelete.Show();
            }
        }
        private async Task Delete()
        {
            Dispatcher.Dispatch(new DeleteItemAction<ContactDto>(State.Value.SelectedItem.Id));
            _grid.Refresh();
            await _confirmDelete.Hide();

        }
        private async Task ActionCancelled()
        {
            await _grid.ClearSelectionAsync();
            await _confirmDelete.Hide();
        }


        private async Task OnBeginHandlerAsync(ActionEventArgs<ContactDto> Args)
        {
            if (Args.RequestType == Syncfusion.Blazor.Grids.Action.Save)
            {

            }
            if (Args.RequestType == Syncfusion.Blazor.Grids.Action.BeginEdit)
            {
                Args.Cancel = true;
                await _edit.ShowModal();
            }
            if (Args.RequestType == Syncfusion.Blazor.Grids.Action.Add)
            {
                Args.Cancel = true;
                await _grid.ClearRowSelection();
                await _edit.ShowModal();
            }


            if (Args.RequestType == Syncfusion.Blazor.Grids.Action.Delete)
            {
                Args.Cancel = true;
                await _confirmDelete.Show();
            }
        }

        private void RowSelected(RowSelectEventArgs<ContactDto> args)
        {
            Dispatcher.Dispatch(new SelectItemAction<ContactDto>(args.Data));
        }
        private void RowDeselected(RowDeselectEventArgs<ContactDto> args)
        {
            Dispatcher.Dispatch(new DeSelectItemAction<ContactDto>());
        }

        private async Task Refresh()
        {
            while (State.Value.IsLoading)
            {
                await Task.Delay(50);
            }
            _grid.Refresh();
        }
    }
}

