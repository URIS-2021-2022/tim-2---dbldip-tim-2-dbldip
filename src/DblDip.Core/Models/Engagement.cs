using BuildingBlocks.EventStore;
using DblDip.Core.DomainEvents;
using System;

namespace DblDip.Core.Models
{
    public class Engagement : Service
    {
        public Guid EngagementId { get; private set; }
        protected override void When(dynamic @event) => When(@event);

        public Engagement()
        {

        }

        public void When(EngagementUpdated engagementUpdated)
        {

        }

        protected override void EnsureValidState()
        {
            throw new NotSupportedException();
        }

        public void Update()
        {
            Apply(new EngagementUpdated());
        }

    }
}
