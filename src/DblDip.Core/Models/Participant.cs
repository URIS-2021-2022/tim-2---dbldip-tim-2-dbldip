using BuildingBlocks.EventStore;
using DblDip.Core.DomainEvents;
using System;

namespace DblDip.Core.Models
{
    public class Participant : AggregateRoot
    {
        public Guid ParticipantId { get; private set; }
        public DateTime? Deleted { get; private set; }
        public Participant()
        {
            Apply(new ParticipantCreated(Guid.NewGuid()));
        }

        protected override void When(dynamic @event) => When(@event);

        public void When(ParticipantUpdated participantUpdated)
        {
            throw new NotSupportedException();
        }

        public void When(ParticipantCreated participantCreated)
        {
            ParticipantId = participantCreated.ParticipantId;
        }

        public void When(ParticipantRemoved participantRemoved)
        {
            Deleted = participantRemoved.Deleted;
        }

        protected override void EnsureValidState()
        {

        }

        public void Update()
        {
            Apply(new ParticipantUpdated());
        }

        public void Remove(DateTime deleted)
        {
            Apply(new ParticipantRemoved(deleted));
        }
    }
}
