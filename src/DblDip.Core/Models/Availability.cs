using BuildingBlocks.EventStore;
using DblDip.Core.DomainEvents;
using System;

namespace DblDip.Core.Models
{
    public class Availability : AggregateRoot
    {
        public Guid AvailabilityId { get; private set; }
        public DateTime? Deleted { get; private set; }

        public Availability()
        {
            Apply(new AvailabilityCreated(Guid.NewGuid()));
        }

        protected override void When(dynamic @event) => When(@event);

        public void When(AvailabilityCreated availabilityCreated)
        {
            AvailabilityId = availabilityCreated.AvailabilityId;
        }

        public void When(AvailabilityRemoved availabilityRemoved)
        {
            Deleted = availabilityRemoved.Deleted;
        }

        public static void When(AvailabilityUpdated availabilityUpdated)
        {
            throw new NotSupportedException();
        }

        protected override void EnsureValidState()
        {

        }

        public void Remove(DateTime deleted)
        {
            Apply(new AvailabilityRemoved(deleted));
        }

        public void Update()
        {
            throw new NotSupportedException();
        }
    }
}
