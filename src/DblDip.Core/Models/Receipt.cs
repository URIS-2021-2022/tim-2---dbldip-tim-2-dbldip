using BuildingBlocks.EventStore;
using DblDip.Core.DomainEvents;
using System;

namespace DblDip.Core.Models
{
    public class Receipt : AggregateRoot
    {
        public Guid ReceiptId { get; private set; }
        public Guid DigitalAssetId { get; private set; }
        public DateTime? Deleted { get; private set; }
        public Receipt()
        {
            Apply(new ReceiptCreated(Guid.NewGuid()));
        }
        protected override void When(dynamic @event) => When(@event);

        public void When(ReceiptCreated receiptCreated)
        {
            ReceiptId = receiptCreated.ReceiptId;
        }

        public void When(ReceiptRemoved receiptRemoved)
        {
            Deleted = receiptRemoved.Deleted;
        }

        public static void When(ReceiptUpdated receiptUpdated)
        {
            //do nothing
        }

        protected override void EnsureValidState()
        {

        }
        public void Remove(DateTime deleted)
        {
            Apply(new ReceiptRemoved(deleted));
        }

        public void Update()
        {
            Apply(new ReceiptUpdated());
        }
    }
}
