using MediatR;
using Microsoft.Extensions.Logging;
using JobFileSystem.Core.Interfaces;
using System.Diagnostics;


namespace JobFileSystem.Core.Features.Common.Behaviours
{
    public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly Stopwatch _timer;
        private readonly ILogger<TRequest> _logger;
        private readonly ICurrentUserService _currentUserService;

        public PerformanceBehaviour(
            ILogger<TRequest> logger,
            ICurrentUserService currentUserService)
        {
            _timer = new Stopwatch();

            _logger = logger;
            _currentUserService = currentUserService;

        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            _timer.Start();

            var response = await next();

            _timer.Stop();

            var elapsedMilliseconds = _timer.ElapsedMilliseconds;

            if (elapsedMilliseconds > 500)
            {
                var requestName = typeof(TRequest).Name;
                var identityName = _currentUserService.GetIdentityName;

                _logger.LogWarning("Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@UserId} {@Request}",
                    requestName, elapsedMilliseconds, identityName, request);
            }

            return response;
        }
    }
}
