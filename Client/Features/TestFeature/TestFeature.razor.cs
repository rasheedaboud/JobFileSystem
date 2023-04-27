using Client.Features;
using Client.Features.Contacts;
using Fluxor;
using JobFileSystem.Client.Components;
using JobFileSystem.Client.Features.Contacts.Components;
using JobFileSystem.Shared.Contacts;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Navigations;

namespace JobFileSystem.Client.Features.TestFeature
{
    public partial class TestFeature
    {

        [Inject] protected NavigationManager NavigationManager { get; set; }
        [Inject] protected IState<ContactState> State { get; set; }
        [Inject] protected IDispatcher Dispatcher { get; set; }
        private List<object> _toolBarEdit =
            new Toolbar().Add()
                         .DefaultEdit()
                         .DefaultDelete()
                         .Cancel()
                         .Update()
                         .DefaultRefresh()
                         .Build();

        private List<object> _toolBar = new Toolbar().Add()
                                                     .Refresh()
                                                     .Build();
        private SfGrid<ContactDto> _grid;
        private SyncfusionValidator _syncfusionValidator;
        private async Task OnToolbarClickHandler(ClickEventArgs args)
        {
            if (args.Item.Text == Buttons.Refresh)
            {
                _grid.Refresh();
            }
        }
        private async Task OnActionCompleteHandler(ActionEventArgs<ContactDto> Args)
        {

            if (Args.RequestType == Syncfusion.Blazor.Grids.Action.Delete)
            {
                _grid.Refresh();
            }
        }
        private async Task OnActionBeginHandler(ActionEventArgs<ContactDto> args)
        {

            if (args.RequestType == Syncfusion.Blazor.Grids.Action.Delete)
            {
                try
                {
                    await _grid.DeleteRecordAsync();
                    _grid.Refresh();
                }
                catch (Exception)
                {

                    _grid.Refresh();
                }
                
            }
            if (args.RequestType == Syncfusion.Blazor.Grids.Action.Save)
            {
                //await _syncfusionValidator.ValidateAsync();
                if (_syncfusionValidator.HasValidationErrors)
                {
                    args.Cancel = true;
                }
            }
        }
    }
}

