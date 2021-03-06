using BuildingBlocks.EventStore;
using DblDip.Core.DomainEvents;
using System;

namespace DblDip.Core.Models
{
    public class Discount: AggregateRoot
    {
        public Guid DiscountId { get; private set; }
        public DateTime? Deleted { get; private set; }
        public Discount()
        {

        }
        protected override void When(dynamic @event) => When(@event);

        public void When(DiscountCreated discountCreated)
        {
            DiscountId = discountCreated.DiscountId;
        }

        public static void When(DiscountUpdated discountUpdated)
        {
            throw new NotSupportedException();
        }

        public void When(DiscountRemoved discountRemoved)
        {
            Deleted = discountRemoved.Deleted;
        }

        protected override void EnsureValidState()
        {

        }

        public void Update()
        {
            Apply(new DiscountUpdated());
        }

        public void Remove(DateTime deleted)
        {
            Apply(new DiscountRemoved(deleted));
        }
    }
}
