using BuildingBlocks.EventStore;
using DblDip.Core.DomainEvents;
using System;

namespace DblDip.Core.Models
{
    public class Payment : AggregateRoot
    {
        public Guid PaymentId { get; private set; }
        public DateTime? Deleted { get; private set; }
        public Payment()
        {
            Apply(new PaymentCreated(Guid.NewGuid()));
        }
        protected override void When(dynamic @event) => When(@event);

        public void When(PaymentCreated paymentCreated)
        {
            PaymentId = paymentCreated.PaymentId;
        }

        public static void When(PaymentUpdated paymentUpdated)
        {
            throw new NotSupportedException();
        }

        public void When(PaymentRemoved paymentRemoved)
        {
            Deleted = paymentRemoved.Deleted;
        }

        protected override void EnsureValidState()
        {

        }

        public void Update()
        {
            Apply(new PaymentUpdated());
        }

        public void Remove(DateTime deleted)
        {
            Apply(new PaymentRemoved(deleted));
        }
    }
}
