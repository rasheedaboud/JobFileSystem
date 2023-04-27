using JobFileSystem.Application.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Identity.Web;
using Microsoft.AspNetCore.Authentication.JwtBearer;

public class Program
{
    public static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            try
            {
                var env = services.GetRequiredService<IWebHostEnvironment>();

                if (env != null && env.IsDevelopment())
                {

                    var dbContext = services.GetRequiredService<ApplicationDbContext>();

                    var context = dbContext;

                    var created = context.Database.EnsureCreated();
                    //context.Database.EnsureDeleted();
                    context.Database.Migrate();


                    //await ApplicationDbContextSeed.SeedSampleDataAsync(factory);

                }

            }
            catch (Exception ex)
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

                logger.LogError(ex, "An error occurred while migrating or seeding the database.");

            }
        }

        await host.RunAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}





