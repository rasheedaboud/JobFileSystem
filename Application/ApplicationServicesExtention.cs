using Application.Services;
using Core.Entities;
using FluentValidation;
using JobFileSystem.Application.Data.Repositories;
using JobFileSystem.Core.Features.Common.Behaviours;
using JobFileSystem.Core.Interfaces;
using JobFileSystem.Shared.Interfaces;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace JobFileSystem.Application
{
    public static class ApplicationServicesExtention
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));

            //services.AddTransient(typeof(INotificationHandler<NdtItemAddedEvent>), typeof(NdtItemAddedEventHandler));

            services.AddTransient<HttpClient>();
            services.AddHttpContextAccessor();

            //READ WRITE REPOSITORIES
            services.AddScoped<IRepository<JobFile>, EfRepository<JobFile>>();
            services.AddScoped<IRepository<Contact>, EfRepository<Contact>>();
            services.AddScoped<IRepository<Estimate>, EfRepository<Estimate>>();
            services.AddScoped<IRepository<LineItem>, EfRepository<LineItem>>();
            services.AddScoped<IRepository<MaterialTestReport>, EfRepository<MaterialTestReport>>();
            services.AddScoped<ISyncfusionFileManager, SyncfusionFileManager>();
            //READ ONLY REPOSITORIES


            //SERVICES
            services.AddScoped<IAzureBlobService, AzureBlobService>();
            services.AddTransient<ICurrentUserService, CurrentUserService>();

            //Validators

        }
    }
}
