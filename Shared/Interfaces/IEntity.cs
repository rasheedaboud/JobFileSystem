using System;

namespace JobFileSystem.Shared.Interfaces
{

    public interface IEntity
    {
        public string Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? LastModified { get; set; }
    }
}
