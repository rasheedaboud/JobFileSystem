using Client.Features.Contacts;
using Client.Features.Estimates;
using Client.Features.JobFiles;
using Client.Features.MaterialTestReports;
using Fluxor;
using JobFileSystem.Client;
using JobFileSystem.Client.Features.JobFiles;
using JobFileSystem.Client.JsInterop;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using MudBlazor;
using MudBlazor.Services;
using Syncfusion.Blazor;
using Syncfusion.Licensing;
using System.Globalization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

SyncfusionLicenseProvider.RegisterLicense("NjY3MjI4QDMyMzAyZTMyMmUzME9rTXF5TGpDeC9RU05aME84eHVzNzk5T0VoSS9aVjc2S0lsTm1hbm5vQ1k9");

builder.Services.AddSyncfusionBlazor();
var currentAssembly = typeof(Program).Assembly;
builder.Services.AddFluxor(options => options.ScanAssemblies(currentAssembly));

builder.Services.AddHttpClient("JobFileSystem.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("JobFileSystem.ServerAPI"));

builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;

    config.SnackbarConfiguration.PreventDuplicates = false;
    config.SnackbarConfiguration.NewestOnTop = true;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 5000;
});

builder.Services.AddMsalAuthentication(options =>
{
    builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
    options.ProviderOptions.DefaultAccessTokenScopes.Add("api://5f593524-b452-4f36-8ee3-cf39773a68be/API.Access");
});

builder.Services.AddSingleton(serviceProvider => (IJSInProcessRuntime)serviceProvider.GetRequiredService<IJSRuntime>());
builder.Services.AddSingleton(serviceProvider => (IJSUnmarshalledRuntime)serviceProvider.GetRequiredService<IJSRuntime>());

builder.Services.AddScoped<IJsMethods, JsMethods>();
builder.Services.AddScoped<IJobFileClient, JobFileClient>();
builder.Services.AddScoped<IContactClient, ContactClient>();
builder.Services.AddScoped<IEstimateClient, EstimateClient>();
builder.Services.AddScoped<IMaterialTestReportsClient, MaterialTestReportsClient>();

CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-CA");

await builder.Build().RunAsync();
