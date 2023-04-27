using Fluxor;
using Fluxor.Blazor.Web.Components;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor;

namespace Client.Features
{
    public partial class ClientFeature<T> : FluxorComponent
    {
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public IState<T> State { get; set; }
        [Inject] public IDispatcher Dispatcher { get; set; }

    }
}
