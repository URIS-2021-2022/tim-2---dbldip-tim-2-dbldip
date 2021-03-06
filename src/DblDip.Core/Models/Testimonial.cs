using BuildingBlocks.EventStore;
using DblDip.Core.DomainEvents;
using System;

namespace DblDip.Core.Models
{
    public class Testimonial : AggregateRoot
    {
        public Guid TestimonialId { get; private set; }
        public Guid PhotographerId { get; private set; }
        public Guid ClientId { get; private set; }
        public string Description { get; private set; }
        public DateTime? Deleted { get; private set; }
        protected override void When(dynamic @event) => When(@event);

        public Testimonial()
        {
            Apply(new TestimonialCreated(Guid.NewGuid()));
        }
        public static void When(TestimonialCreated testimonialCreated)
        {

        }

        public static void When(TestimonialRemoved testimonialRemoved)
        {

        }

        public static void When(TestimonialUpdated testimonialUpdated)
        {

        }

        protected override void EnsureValidState()
        {

        }

        public void Remove(DateTime deleted)
        {
            Apply(new TestimonialRemoved(deleted));
        }

        public void Update()
        {

        }
    }
}
