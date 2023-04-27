using Client.Features;
using Client.Features.MaterialTestReports;
using Fluxor;
using JobFileSystem.Client.Components;
using JobFileSystem.Client.JsInterop;
using JobFileSystem.Shared.DTOs;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Navigations;
using System.Net.Http.Json;

namespace JobFileSystem.Client.Features.MaterialTestReports.Pages
{
    public partial class Index
    {

        [Inject] protected NavigationManager NavigationManager { get; set; }
        [Inject] protected IState<MaterialTestReportState> State { get; set; }
        [Inject] protected IJsMethods IJsMethods { get; set; }
        [Inject] protected HttpClient HttpClient { get; set; }
        [Inject] protected IDispatcher Dispatcher { get; set; }
        private List<object> _toolBarEdit = new Toolbar().Add()
                                                     .Edit()
                                                     .Print()
                                                     .Delete()
                                                     .Refresh()
                                                     .Build();

        private List<object> _toolBar = new Toolbar().Add()
                                                     .Refresh()
                                                     .Build();
        private SfGrid<MaterialTestReportDto> _grid;

        private Components.Edit _edit { get; set; }
        private ConfirmDelete _confirmDelete;


        private async Task OnToolbarClickHandler(ClickEventArgs args)
        {
            if (args.Item.Id == Buttons.Refresh)
            {
                _grid.Refresh();
            }
            if (args.Item.Id == Buttons.Add)
            {
                args.Cancel = true;
                await AddItem();
            }
            if (args.Item.Id == Buttons.Edit)
            {
                args.Cancel = true;
                await EditItem();
            }
            if (args.Item.Id == Buttons.Delete)
            {
                args.Cancel = true;
                await _confirmDelete.Show();
            }
            if (args.Item.Id == Buttons.Print)
            {
                args.Cancel = true;
            }
        }

        private async Task EditItem()
        {
            if (State.Value.SelectedItem is null)
            {
                return;
            }
            await _edit.ShowModal();
        }
        private async Task AddItem()
        {
            await _edit.ShowModal();
        }

        private async Task Delete()
        {
            Dispatcher.Dispatch(new DeleteItemAction<MaterialTestReportDto>(State.Value.SelectedItem.Id));

            while (State.Value.IsLoading)
            {
                await Task.Delay(50);
            }
            
            _grid.Refresh();
            await _confirmDelete.Hide();

        }
        private async Task ActionCancelled()
        {
            await _grid.ClearSelectionAsync();
            await _confirmDelete.Hide();
        }


        private async Task OnBeginHandlerAsync(ActionEventArgs<MaterialTestReportDto> Args)
        {
            if (Args.RequestType == Syncfusion.Blazor.Grids.Action.Save)
            {

            }
            if (Args.RequestType == Syncfusion.Blazor.Grids.Action.BeginEdit)
            {
                Args.Cancel = true;
                await EditItem();
            }
            if (Args.RequestType == Syncfusion.Blazor.Grids.Action.Add)
            {
                Args.Cancel = true;
                await AddItem();
            }


            if (Args.RequestType == Syncfusion.Blazor.Grids.Action.Delete)
            {
                Args.Cancel = true;
                await _confirmDelete.Show();
            }
        }

        private void RowSelected(RowSelectEventArgs<MaterialTestReportDto> args)
        {
            Dispatcher.Dispatch(new SelectItemAction<MaterialTestReportDto>(args.Data));
        }
        private void RowDeselected(RowDeselectEventArgs<MaterialTestReportDto> args)
        {
            Dispatcher.Dispatch(new DeSelectItemAction<MaterialTestReportDto>());
        }

        private async Task Refresh()
        {
            while (State.Value.IsLoading)
            {
                await Task.Delay(50);
            }
            await _grid.ClearSelectionAsync();    
            _grid.Refresh();
        }
    }
}

