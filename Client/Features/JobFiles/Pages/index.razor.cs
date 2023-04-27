using Client.Features;
using Client.Features.JobFiles;
using Fluxor;
using Fluxor.Blazor.Web.Components;
using JobFileSystem.Client.Components;
using JobFileSystem.Shared.JobFiles;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Navigations;

namespace JobFileSystem.Client.Features.JobFiles.Pages
{
    public partial class Index : FluxorComponent
    {

        [Inject] protected NavigationManager NavigationManager { get; set; }
        [Inject] protected IState<JobFileState> State { get; set; }
        [Inject] protected IDispatcher Dispatcher { get; set; }
        private List<object> _toolBarEdit = new Toolbar()
        .Edit()
        .Refresh()
        .Build();

        private List<object> _toolBar = new Toolbar()
        .Refresh()
        .Build();

        private SfGrid<JobFileDto> _grid;

        private JobFileSystem.Client.Features.JobFiles.Components.Edit _edit { get; set; }
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
            Dispatcher.Dispatch(new DeleteItemAction<JobFileDto>(State.Value.SelectedItem.Id));
            _grid.Refresh();
            await _confirmDelete.Hide();

        }
        private async Task ActionCancelled()
        {
            await _grid.ClearSelectionAsync();
            await _confirmDelete.Hide();
        }


        private async Task OnBeginHandlerAsync(ActionEventArgs<JobFileDto> Args)
        {
            if (Args.RequestType == Syncfusion.Blazor.Grids.Action.Save)
            {

            }
            if (Args.RequestType == Syncfusion.Blazor.Grids.Action.BeginEdit)
            {
                Args.Cancel = true;
            }
            if (Args.RequestType == Syncfusion.Blazor.Grids.Action.Add)
            {
                await _grid.ClearRowSelection();
                Args.Cancel = true;
                await _edit.ShowModal();
            }


            if (Args.RequestType == Syncfusion.Blazor.Grids.Action.Delete)
            {
                Args.Cancel = true;
            }
        }

        private void RowSelected(RowSelectEventArgs<JobFileDto> args)
        {
            Dispatcher.Dispatch(new SelectItemAction<JobFileDto>(args.Data));
        }
        private void RowDeselected(RowDeselectEventArgs<JobFileDto> args)
        {
            Dispatcher.Dispatch(new DeSelectItemAction<JobFileDto>());
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

