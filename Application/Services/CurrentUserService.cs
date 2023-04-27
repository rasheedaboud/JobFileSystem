using JobFileSystem.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));

        }

        public string GetIdentityName => _httpContextAccessor.HttpContext?.User?.Identities.First().Name;
        public string GetFullName => _httpContextAccessor.HttpContext?.User?.FindFirstValue("name");
    }
}
