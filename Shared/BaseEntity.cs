using JobFileSystem.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace JobFileSystem.Shared
{
    // This can be modified to BaseEntity<TId> to support multiple key types (e.g. Guid)
    public abstract class BaseEntity : IEntity
    {
        [NotMapped]
        private List<BaseDomainEvent> _events = new List<BaseDomainEvent>();
        [NotMapped]
        public IReadOnlyList<BaseDomainEvent> Events => _events;

        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string CreatedBy { get; set; } = "";
        public bool IsDeleted { get ; set; }
        public DateTime? LastModified { get; set; } 
        public string LastModifiedBy { get; set; } = "";
        public DateTime? CreatedOn { get; set; }

        public virtual void AddEvent(BaseDomainEvent baseDomainEvent)
        {
            _events.Add(baseDomainEvent);
        }
        public virtual void ClearEvents()
        {
            _events.Clear();
        }
    }
}