using BuildingBlocks.EventStore;
using DblDip.Core.DomainEvents;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DblDip.Core.Models
{
    public class Role : AggregateRoot
    {
        public Guid RoleId { get; private set; }
        public string Name { get; private set; }
        public DateTime? Deleted { get; private set; }
        public List<Privilege> Privileges()
        {
            return _privileges.ToList();
        }

        private List<Privilege> _privileges;

        protected Role()
        {

        }

        protected override void When(dynamic @event) => When(@event);

        public Role(string name)
        {
            Apply(new RoleCreated(Guid.NewGuid(), name));
        }

        public Role(Guid roleId, string name)
        {
            Apply(new RoleCreated(roleId, name));
        }
        public void When(RoleCreated roleCreated)
        {
            RoleId = roleCreated.RoleId;
            Name = roleCreated.Name;
            _privileges = new List<Privilege>();
        }

        public void When(RoleRemoved roleRemoved)
        {
            Deleted = roleRemoved.Removed;
        }

        public void When(PrivilegesUpdated privilegesUpdated)
        {
            _privileges = privilegesUpdated.Privileges.ToList();
        }

        protected override void EnsureValidState()
        {

        }

        public void Remove(DateTime removed)
        {
            Apply(new RoleRemoved(removed));
        }

        public void UpdatePrivileges(ICollection<Privilege> privileges)
        {
            Apply(new PrivilegesUpdated(privileges));
        }
    }
}
