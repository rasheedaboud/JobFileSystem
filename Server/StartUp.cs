using Core;
using JobFileSystem.Application;
using JobFileSystem.Application.Data;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using NdtTracking.Wasm.Server;
using System.Reflection;

public partial class Startup
{
    public IWebHostEnvironment _env { get; }
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration, IWebHostEnvironment env)
    {
        Configuration = configuration;
        _env = env;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        // Add services to the container.
        services.AddSingleton<IConfiguration>(Configuration);
        services.AddMediatR(typeof(Startup).GetTypeInfo().Assembly);

        services.AddApplicationServices();
        services.AddCoreServices();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApi(Configuration.GetSection("AzureAd"));

        services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                        x => x.MigrationsAssembly("Application").EnableRetryOnFailure()));


        services.AddControllers()
                .AddOData(option =>
                    option.AddRouteComponents("odata", EdmModel.BuildEdmModel())
                .EnableQueryFeatures(10000)); 

        services.AddRazorPages();
    }
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseWebAssemblyDebugging();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }
        app.UseHttpsRedirection();
        app.UseBlazorFrameworkFiles();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapFallbackToFile("index.html");
        });
    }
}





