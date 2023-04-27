using MediatR;
using System;
using System.Threading.Tasks;

namespace JobFileSystem.Shared
{
    public abstract class BaseDomainEvent : INotification
    {
        public static Func<IMediator> Mediator { get; set; }
        public DateTime DateOccurred { get; protected set; } = DateTime.UtcNow;

        public static async Task Rasie<T>(T args) where T : INotification
        {
            var _mediator = Mediator.Invoke();
            await _mediator.Publish<T>(args);
        }
    }
}