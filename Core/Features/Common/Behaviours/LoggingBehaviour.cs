using JobFileSystem.Core.Interfaces;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace JobFileSystem.Core.Features.Common.Behaviours
{
    public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest>
    {
        private readonly ILogger _logger;
        private readonly ICurrentUserService _currentUserService;


        public LoggingBehaviour(ILogger<TRequest> logger, ICurrentUserService currentUserService)
        {
            _logger = logger;
            _currentUserService = currentUserService;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task Process(TRequest request, CancellationToken cancellationToken)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            var requestName = typeof(TRequest).Name;
            var identityName = _currentUserService.GetIdentityName;

            string message = $"Request: {requestName} {identityName} { request}";
            _logger.LogInformation(message);
        }
    }
}
