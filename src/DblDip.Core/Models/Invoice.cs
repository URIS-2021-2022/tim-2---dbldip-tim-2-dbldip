using BuildingBlocks.EventStore;
using DblDip.Core.DomainEvents;
using System;

namespace DblDip.Core.Models
{
    public class Invoice : AggregateRoot
    {

        public Guid InvoiceId { get; private set; }
        public DateTime? Deleted { get; private set; }
        protected override void When(dynamic @event) => When(@event);

        public Invoice()
        {
            Apply(new InvoiceCreated(Guid.NewGuid()));
        }
        public void When(InvoiceCreated invoiceCreated)
        {
            InvoiceId = invoiceCreated.InvoiceId;
        }

        public void When(InvoiceRemoved invoiceRemoved)
        {
            Deleted = invoiceRemoved.Deleted;
        }

        public static void When(InvoiceUpdated invoiceUpdated)
        {
            throw new NotSupportedException();
        }

        protected override void EnsureValidState()
        {

        }

        public void Remove(DateTime deleted)
        {
            Apply(new InvoiceRemoved(deleted));
        }

        public void Update()
        {
            Apply(new InvoiceUpdated());
        }

    }
}
