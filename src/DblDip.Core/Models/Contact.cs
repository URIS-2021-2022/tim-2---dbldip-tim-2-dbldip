using BuildingBlocks.EventStore;
using DblDip.Core.DomainEvents;
using System;

namespace DblDip.Core.Models
{
    public class Contact : AggregateRoot
    {
        public Guid ContactId { get; private set; }
        public DateTime? Deleted { get; private set; }
        public Contact()
        {
            Apply(new ContactCreated(Guid.NewGuid()));
        }
        protected override void When(dynamic @event) => When(@event);

        public void When(ContactCreated contactCreated)
        {
            ContactId = contactCreated.ContactId;
        }

        public void When(ContactRemoved contactRemoved)
        {
            Deleted = contactRemoved.Deleted;
        }

        public static void When(ContactUpdated contactUpdated)
        {

        }

        protected override void EnsureValidState()
        {

        }

        public void Remove(DateTime deleted)
        {
            Apply(new ContactRemoved(deleted));
        }

        public void Update()
        {
            throw new NotSupportedException();
        }
    }
}
