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

        public static void When(EngagementUpdated engagementUpdated)
        {
            throw new NotSupportedException();
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
