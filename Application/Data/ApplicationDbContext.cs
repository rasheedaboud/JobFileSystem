using Application.Data.Configurations;
using Core.Entities;
using JobFileSystem.Core.Interfaces;
using JobFileSystem.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace JobFileSystem.Application.Data
{
    public class ApplicationDbContext : DbContext
    {

        private readonly ICurrentUserService _currentUserService;
        private readonly IMediator _mediator;


        public ApplicationDbContext(
            DbContextOptions options,
            ICurrentUserService currentUserService,
            IMediator mediator) : base(options)
        {
            _currentUserService = currentUserService;
            _mediator = mediator;
        }

        public DbSet<JobFile> JobFiles { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Estimate> Estimates { get; set; }
        public DbSet<MaterialTestReport> MaterialTestReports { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(JobFileConfiguration).Assembly);

            base.OnModelCreating(builder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {

            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = _currentUserService.GetIdentityName ?? "";
                        entry.Entity.CreatedOn = DateTime.UtcNow;

                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedBy = _currentUserService.GetIdentityName ?? "";
                        entry.Entity.LastModified = DateTime.UtcNow;
                        break;
                }
            }

            var result = await base.SaveChangesAsync(cancellationToken);
            // ignore events if no dispatcher provided
            if (_mediator == null)
            {
                return result;
            }

            // dispatch events only if save was successful
            var entitiesWithEvents = ChangeTracker.Entries<BaseEntity>()
                .Select(e => e.Entity)
                .Where(e => e.Events.Any())
                .ToArray();

            foreach (var entity in entitiesWithEvents)
            {
                var events = entity.Events.ToArray();
                entity.ClearEvents();
                foreach (var domainEvent in events)
                {
                    await _mediator.Publish(domainEvent, cancellationToken);
                }
            }

            return result;
        }

    }
}
