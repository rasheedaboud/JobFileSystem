using Client.Features;
using Fluxor;
using JobFileSystem.Client.Components;
using JobFileSystem.Client.Features.Attachments.Actions;
using JobFileSystem.Client.JsInterop;
using JobFileSystem.Shared.Attachments;
using JobFileSystem.Shared.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using Syncfusion.Blazor.Navigations;
using Syncfusion.Blazor.Popups;
using System.Net.Http.Json;

namespace JobFileSystem.Client.Features.Attachments.Components
{
    public partial class Attachments<TValue,TState> 
        where TValue : class, IId, IAttachment, new()
        where TState : BaseState<TValue>
    {

        [Inject] protected NavigationManager NavigationManager { get; set; }
        [Inject] protected HttpClient HttpClient { get; set; }
        [Inject] protected IJsMethods IJsMethods { get; set; }
        [Inject] protected IDispatcher Dispatcher { get; set; }
        [Inject] protected IState<TState> State { get; set; }
        [Inject] protected ISnackbar snackbar { get; set; }
        [Parameter] public string AddUrl { get; set; }
        [Parameter] public string ParentId { get; set; }
        [Parameter] public bool OwnedEntity { get; set; } = false;
        [Parameter] public string DeleteUrl { get; set; }
        [Parameter] public bool IsEnabled { get; set; }
        [Parameter] public EventCallback<AttachmentDto> OnSucess { get; set; }

        private List<object> _toolBarEdit = new Toolbar().Add()
                                                     .Delete()
                                                     .Download()
                                                     .Refresh()
                                                     .Build();

        private List<object> _toolBar = new Toolbar().Add()
                                                     .Refresh()
                                                     .Build();
        private List<object> _toolBarDownload = new Toolbar()
                                                     .Download()
                                                     .Refresh()
                                                     .Build();
        private SfGrid<AttachmentDto> _grid;

        private ConfirmDelete _confirmDelete;
        private AttachmentDto _attachment;
        private SfUploader _sfUploader;
        private SfDialog _attachments;
        private List<object> ContextMenu()
        {
            if (IsEnabled)
            {
                return new List<object>() { "Add", "Copy", "Edit", "Delete", "Refresh", "ExcelExport" };
            }
            return new();
        }
        private List<object> ToolBar()
        {
            if(!IsEnabled ) return new Toolbar().Build();

            if (IsEnabled && _attachment == null) return _toolBar;

            if (IsEnabled && _attachment != null) return _toolBarEdit;

            if (!IsEnabled && _attachment != null) return _toolBarDownload;

            return new List<object>();
        }

        private string _deleteUrl => $"{DeleteUrl}&fileId={_attachment.Id}";
        public void ActionFailureHandler(Syncfusion.Blazor.Grids.FailureEventArgs args)
        {
            return;
        }
        private async Task UploadSuccess(SuccessEventArgs args)
        {
            if (OwnedEntity)
            {
                await OnSucess.InvokeAsync();
            }
            else
            {
                Dispatcher.Dispatch(new AttachmentAddedAction<TValue>(State.Value.SelectedItem.Id));

                while (State.Value.IsLoading)
                {
                    await Task.Delay(50);
                }
            }


            await _attachments.HideAsync();
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
            if (args.Item.Id == Buttons.Download)
            {
                args.Cancel = true;
                await DownloadItem();
            }
            if (args.Item.Id == Buttons.Edit)
            {
                args.Cancel = true;

                if(_attachment is null) { return; }

                await IJsMethods.DownloandBlobLink(_attachment.Link,_attachment.FileName);
            }
            if (args.Item.Id == Buttons.Delete)
            {
                args.Cancel = true;
                if(_attachment ==null) { return; }
                await _confirmDelete.Show();
            }
        }

        private async Task AddItem()
        {
            await _attachments.ShowAsync();
        }
        private async Task DownloadItem()
        {
            try
            {
                if (_attachment is null) { return; }

                await IJsMethods.DownloandBlobLink(_attachment.Link, _attachment.FileName);
            }
            catch (Exception)
            {

                snackbar.Add($"Attachment could not be downloaded.", Severity.Error);
            }
            
        }
        private async Task Delete()
        {
            var response = await HttpClient.DeleteAsync(_deleteUrl,default);

            if (response.IsSuccessStatusCode)
            {
                State.Value.SelectedItem.Attachments.Remove(_attachment);
                snackbar.Add($"Attachment removed.", Severity.Success);
            }
            else
            {
                snackbar.Add($"Attachment could not be deleted.", Severity.Error);
            }

            _grid.Refresh();
            await _confirmDelete.Hide();

        }
        private async Task ActionCancelled()
        {
            await _grid.ClearSelectionAsync();
            await _confirmDelete.Hide();
        }


        private async Task OnBeginHandlerAsync(ActionEventArgs<AttachmentDto> Args)
        {
            if (Args.RequestType == Syncfusion.Blazor.Grids.Action.Save)
            {

            }
            if (Args.RequestType == Syncfusion.Blazor.Grids.Action.BeginEdit)
            {
                Args.Cancel = true;
                DownloadItem();
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

        private void RowSelected(RowSelectEventArgs<AttachmentDto> args)
        {
            _attachment = args.Data;
        }
        private void RowDeselected(RowDeselectEventArgs<AttachmentDto> args)
        {
            _attachment = null;
        }

        private async Task Refresh()
        {
            _grid.Refresh();
        }


    }
}

