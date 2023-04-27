using Client.Features;
using Client.Features.Estimates;
using Fluxor;
using Fluxor.Blazor.Web.Components;
using JobFileSystem.Client.Components;
using JobFileSystem.Client.Features.Attachments.Actions;
using JobFileSystem.Shared.Enums;
using JobFileSystem.Shared.LineItems;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Navigations;

namespace JobFileSystem.Client.Features.Estimates.Components
{
    public partial class LineItems_Index : FluxorComponent
    {

        [Inject] protected NavigationManager NavigationManager { get; set; }
        [Inject] protected IState<EstimateState> EstimateState { get; set; }
        [Inject] protected IState<LineItemState> LineItemState { get; set; }

        [Inject] protected IDispatcher Dispatcher { get; set; }
        private List<object> _toolBarEdit = new Toolbar().Add()
                                                     .Edit()
                                                     .Delete()
                                                     .Refresh()
                                                     .Build();

        private List<object> _toolBar = new Toolbar().Add()
                                                     .Refresh()
                                                     .Build();

        private SfGrid<LineItemDto> _grid;

        private Edit_LineItem _edit { get; set; }
        private ConfirmDelete _confirmDelete;
        public void ActionFailureHandler(Syncfusion.Blazor.Grids.FailureEventArgs args)
        {
            return;
        }
        private List<object> ContextMenu()
        {
            if (EstimateState.Value.SelectedItem.CanEdit)
            {
                return new List<object>() { "Add", "Copy", "Edit", "Delete", "Refresh", "ExcelExport" };
            }
            return new();
        }
        private List<object> ToolBar()
        {
            if (EstimateState.Value.SelectedItem.CanEdit)
            {
                if (LineItemState.Value.SelectedItem != null &&
                    EstimateState.Value.SelectedItem != null)
                {
                    return _toolBarEdit;
                }
                else if (LineItemState.Value.SelectedItem == null &&
                         EstimateState.Value.SelectedItem != null)
                {
                    return _toolBar;
                }
            }
            
            return new List<object>();
        }

        public async Task OnAttachmentAdded()
        {
            Dispatcher.Dispatch(new AttachmentAddedToLineItem(EstimateState.Value.SelectedItem.Id));

            while (EstimateState.Value.IsLoading)
            {
                await Task.Delay(50);
            }
            _grid.Refresh();
        }

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
        }
        private async Task EditItem()
        {
            if (LineItemState.Value.SelectedItem is null)
            {
                return;
            }
            await _edit.ShowModal();
        }
        private async Task AddItem()
        {
            Dispatcher.Dispatch(new SelectItemAction<LineItemDto>(new LineItemDto()));
            await _edit.ShowModal();
        }

        private async Task Delete()
        {
            Dispatcher.Dispatch(new DeleteItemAction<LineItemDto>(LineItemState.Value.SelectedItem.Id,
                                                                  EstimateState.Value.SelectedItem.Id));

            while (LineItemState.Value.IsLoading)
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


        private async Task OnBeginHandlerAsync(ActionEventArgs<LineItemDto> Args)
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

        private void RowSelected(RowSelectEventArgs<LineItemDto> args)
        {
            Dispatcher.Dispatch(new SelectItemAction<LineItemDto>(args.Data));
        }
        private async Task RowDeselected(RowDeselectEventArgs<LineItemDto> args)
        {
            Dispatcher.Dispatch(new DeSelectItemAction<LineItemDto>());
            await _grid.CollapseAllDetailRowAsync();
        }

        private async Task Refresh()
        {
            _grid.Refresh();
        }


    }
}

