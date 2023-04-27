namespace JobFileSystem.Core.Interfaces
{
    public interface ICurrentUserService
    {
        public string GetIdentityName { get;  }
        public string GetFullName { get;  }
    }
}
