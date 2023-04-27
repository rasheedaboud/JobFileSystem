using Fluxor;
using Fluxor.Blazor.Web.Components;
using Microsoft.AspNetCore.Components;

namespace NdtTracking.WebComponents.Features
{
    public class BaseFeature<T> : FluxorComponent
    {
        [Inject] protected NavigationManager NavigationManager { get; set; }
        [Inject]protected IState<T> State { get; set; }
        [Inject] protected IDispatcher Dispatcher { get; set; }

    }
}
